using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using mbit.common.cache;
using mbit.common.dal.Repositories;
using oiat.saferinternetbot.Business.Dtos;
using oiat.saferinternetbot.Business.Interfaces;
using oiat.saferinternetbot.DataAccess.Entities;
using oiat.saferinternetbot.LuisApi.ApiClient;

namespace oiat.saferinternetbot.Business.Services
{
    public class IntentService : IScoreService, IIntentService
    {
        private const string IntentCacheKey = "Intents";

        private readonly ILuisApiClient _client;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;
        private readonly IRepository<Answer> _answerRepository;

        public IntentService(ILuisApiClient client, IMapper mapper, ICacheService cache, IRepository<Answer> answerRepository)
        {
            _client = client;
            _mapper = mapper;
            _cache = cache;
            _answerRepository = answerRepository;
        }

        public async Task<ScoreDto> GetScore(string query)
        {
            var score = await _client.GetScore(query);
            var intent = await GetByName(score.Prediction.TopIntent);

            if (intent == null)
            {
                return ScoreDto.Default;
            }

            return new ScoreDto
            {
                IntentId = intent.Id
            };
        }

        public async Task<ICollection<IntentDto>> GetAll()
        {
            return await _cache.GetItemAsync(async () =>
            {
                var items = await _client.GetAllIntents();
                var answers = await _answerRepository.GetAllAsync();

                return items.OrderBy(x => x.Name).Select(x =>
                {
                    var item = _mapper.Map<IntentDto>(x);
                    item.CountAnswers = answers.Count(y => y.IntentId == x.Id);
                    return item;
                }).ToList();
            }, IntentCacheKey);
        }

        public async Task<IntentDto> GetById(Guid id)
        {
            var items = await GetAll();
            return items.SingleOrDefault(x => x.Id == id);
        }

        public async Task<IntentDto> GetByName(string name)
        {
            var items = await GetAll();
            return items.SingleOrDefault(x => x.Name == name);
        }
    }
}
