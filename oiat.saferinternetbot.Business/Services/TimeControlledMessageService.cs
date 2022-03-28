using AutoMapper;
using mbit.common.cache;
using mbit.common.dal.Repositories;
using mbit.common.dal.UnitOfWork;
using oiat.saferinternetbot.Business.Dtos;
using oiat.saferinternetbot.Business.Interfaces;
using oiat.saferinternetbot.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace oiat.saferinternetbot.Business.Services
{
    public class TimeControlledMessageService : ITimeControlledMessageService
    {
        public const string TIMECONTROLLEDMESSAGECACHEKEY = "TimeControlledMessages";
        private const string TIMECONTROLLMESSAGEINFOSCACHEKEY = "AllMessageInfos";

        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<TimeControlledMessage> _messageRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public TimeControlledMessageService(IRepository<TimeControlledMessage> messageRepository, IMapper mapper, IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task AddMessage(TimeControlledMessageDto message)
        {
            var entity = _mapper.Map<TimeControlledMessage>(message);
            _messageRepository.Add(entity);
            await _unitOfWork.SaveAsync();
            _cacheService.InvalidateItem(TIMECONTROLLEDMESSAGECACHEKEY, TIMECONTROLLMESSAGEINFOSCACHEKEY);
        }

        public async Task UpdateMessage(TimeControlledMessageDto message)
        {
            var entity = await _messageRepository.GetByIdAsync(message.Id);
            _mapper.Map(message, entity);
            _messageRepository.Update(entity);
            await _unitOfWork.SaveAsync();
            _cacheService.InvalidateItem(TIMECONTROLLEDMESSAGECACHEKEY, TIMECONTROLLMESSAGEINFOSCACHEKEY);
        }

        public async Task DeleteMessage(Guid id)
        {
            var entity = await _messageRepository.GetByIdAsync(id);
            _messageRepository.Delete(entity);
            await _unitOfWork.SaveAsync();
            _cacheService.InvalidateItem(TIMECONTROLLEDMESSAGECACHEKEY, TIMECONTROLLMESSAGEINFOSCACHEKEY);
        }

        public async Task<IEnumerable<(Guid Id, string Name, bool Enabled)>> GetMessageInfos()
        {
            return await _cacheService.GetItemAsync(async () =>
           {
               var entities = await _messageRepository.GetAllAsync();
               return entities.Select(x => (x.Id, x.Name, x.Enabled));
           }, TIMECONTROLLEDMESSAGECACHEKEY, TIMECONTROLLMESSAGEINFOSCACHEKEY);
        }

        public async Task<TimeControlledMessageDto> GetMessageByID(Guid id)
        {
            var entity = await _messageRepository.GetByIdAsync(id);
            return _mapper.Map<TimeControlledMessageDto>(entity);
        }

        public async Task<IEnumerable<TimeControlledMessageDto>> GetMessagesForTimestamp(DateTimeOffset timestamp)
        {
            var entities = await _messageRepository.GetAsync(x => x.Enabled);

            var items = entities.Where(x =>
           ((x.StartTime.TimeOfDay < x.EndTime.TimeOfDay && x.StartTime.TimeOfDay < timestamp.TimeOfDay && x.EndTime.TimeOfDay > timestamp.TimeOfDay) ||
            (x.StartTime.TimeOfDay > x.EndTime.TimeOfDay && (x.StartTime.TimeOfDay < timestamp.TimeOfDay || x.EndTime.TimeOfDay > timestamp.TimeOfDay)))).ToList();

            return _mapper.Map<IEnumerable<TimeControlledMessageDto>>(items);
        }
    }
}
