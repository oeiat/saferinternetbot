using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using mbit.common.cache;
using mbit.common.dal.Repositories;
using mbit.common.dal.UnitOfWork;
using oiat.saferinternetbot.Business.Dtos;
using oiat.saferinternetbot.Business.Extensions;
using oiat.saferinternetbot.Business.Interfaces;
using oiat.saferinternetbot.DataAccess.Entities;
using oiat.saferinternetbot.DataAccess.Enums;

namespace oiat.saferinternetbot.Business.Services
{
    public class AnswerService : IAnswerService
    {
        private const string AnswersCacheKey = "Answers";

        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Answer> _answerRepository;
        private readonly IRepository<DefaultAnswer> _defaultAnswerRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;

        public AnswerService(IUnitOfWork unitOfWork, IRepository<Answer> answerRepository, IMapper mapper, ICacheService cache, IRepository<DefaultAnswer> defaultAnswerRepository)
        {
            _unitOfWork = unitOfWork;
            _answerRepository = answerRepository;
            _mapper = mapper;
            _cache = cache;
            _defaultAnswerRepository = defaultAnswerRepository;
        }

        public async Task<AnswerDto> GetRandomByIntent(Guid intentId)
        {
            if (intentId == Guid.Empty)
            {
                return await GetByNoAnswerAvailable();
            }

            var items = await GetByIntentId(intentId);
            return !items.Any() ? await GetByNoAnswerAvailable() : items.Random();
        }

        public async Task<AnswerDto> GetByInvalidMessage()
        {
            var items = await GetDefaultByType(DefaultAnswerType.InvalidMessage);
            return items.Random();
        }

        public async Task<AnswerDto> GetByNoAnswerAvailable()
        {
            var items = await GetDefaultByType(DefaultAnswerType.NoAnswerAvailable);
            return items.Random();
        }

        public async Task<ICollection<AnswerDto>> GetByIntentId(Guid intentId)
        {
            return await _cache.GetItemAsync(async () =>
            {
                var items = await _answerRepository.GetAsync(x => x.IntentId == intentId, o => o.OrderBy(x => x.Text));
                return _mapper.Map<ICollection<AnswerDto>>(items);
            }, AnswersCacheKey, intentId);
        }

        public async Task<AnswerDto> GetById(Guid id)
        {
            var item = await _answerRepository.GetByIdAsync(id);
            return _mapper.Map<AnswerDto>(item);
        }

        public async Task<AnswerDto> Add(AnswerDto model)
        {
            var entity = _mapper.Map<Answer>(model);
            _answerRepository.Add(entity);
            await _unitOfWork.SaveAsync();
            _cache.InvalidateItem(AnswersCacheKey, model.IntentId);
            return model;
        }

        public async Task<AnswerDto> Update(Guid answerId, AnswerDto model)
        {
            var entity = await _answerRepository.GetByIdAsync(answerId);
            _mapper.Map(model, entity);
            _answerRepository.Update(entity);
            await _unitOfWork.SaveAsync();
            _cache.InvalidateItem(AnswersCacheKey, model.IntentId);
            return model;
        }

        public async Task Delete(Guid answerId)
        {
            var entity = await _answerRepository.GetByIdAsync(answerId);
            _answerRepository.Delete(answerId);
            await _unitOfWork.SaveAsync();
            _cache.InvalidateItem(AnswersCacheKey, entity.IntentId);
        }

        public async Task<ICollection<AnswerDto>> GetByType(DefaultAnswerType type)
        {
            return await GetDefaultByType(type);
        }



        public async Task<AnswerDto> GetDefaultById(Guid id)
        {
            var item = await _defaultAnswerRepository.GetByIdAsync(id);
            return _mapper.Map<AnswerDto>(item);
        }

        public async Task<AnswerDto> AddDefault(DefaultAnswerType type, AnswerDto model, Guid? timeControlledMessageId = null)
        {
            var entity = _mapper.Map<DefaultAnswer>(model);
            entity.Type = type;
            entity.TimeControlledMessageId = timeControlledMessageId;
            _defaultAnswerRepository.Add(entity);
            await _unitOfWork.SaveAsync();
            _cache.InvalidateItem(AnswersCacheKey, type);
            return model;
        }

        public async Task<AnswerDto> UpdateDefault(Guid answerId, AnswerDto model)
        {
            var entity = await _defaultAnswerRepository.GetByIdAsync(answerId);
            _mapper.Map(model, entity);
            _defaultAnswerRepository.Update(entity);
            await _unitOfWork.SaveAsync();
            _cache.InvalidateItem(AnswersCacheKey, entity.Type);
            return model;
        }

        public async Task DeleteDefault(Guid answerId)
        {
            var entity = await _defaultAnswerRepository.GetByIdAsync(answerId);
            _defaultAnswerRepository.Delete(answerId);
            await _unitOfWork.SaveAsync();
            _cache.InvalidateItem(AnswersCacheKey, entity.Type);
        }

        private async Task<ICollection<AnswerDto>> GetDefaultByType(DefaultAnswerType type)
        {
            return await _cache.GetItemAsync(async () =>
            {
                var items = await _defaultAnswerRepository.GetAsync(x => x.Type == type);
                return _mapper.Map<ICollection<AnswerDto>>(items);
            }, AnswersCacheKey, type);
        }

        public async Task<ICollection<AnswerDto>> GetByType(DefaultAnswerType type, Guid id)
        {
            var items = await _defaultAnswerRepository.GetAsync(x => x.Type == type && x.TimeControlledMessageId == id);
            return _mapper.Map<ICollection<AnswerDto>>(items);
        }

        public async Task<AnswerDto> GetRandomTimeControlledMessage(Guid timeControlledMessageId)
        {
            var items = await GetByType(DefaultAnswerType.TimeRestrictedMessage, timeControlledMessageId);
            return items.Random();
        }
    }
}
