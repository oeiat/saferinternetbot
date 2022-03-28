using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using oiat.saferinternetbot.LuisApi.Models;

namespace oiat.saferinternetbot.LuisApi.ApiClient
{
    public class MockLuisApiClient : ILuisApiClient
    {
        public Task<ScoreApiModel> GetScore(string query)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<IntentApiModel>> GetAllIntents()
        {
            return await Task.FromResult(new List<IntentApiModel>
            {
                new IntentApiModel {Id = Guid.Parse("de632330-3424-4de6-8fb9-c7ae004daae7"), Name = "bye"},
                new IntentApiModel {Id = Guid.Parse("e2ecf1eb-b628-4720-8c0b-fe7fda2111f4"), Name = "chainletter-clown"},
            });
        }
    }
}
