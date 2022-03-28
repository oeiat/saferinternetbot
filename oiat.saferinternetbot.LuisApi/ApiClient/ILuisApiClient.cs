using System.Collections.Generic;
using System.Threading.Tasks;
using oiat.saferinternetbot.LuisApi.Models;

namespace oiat.saferinternetbot.LuisApi.ApiClient
{
    public interface ILuisApiClient
    {
        Task<ScoreApiModel> GetScore(string query);
        Task<IEnumerable<IntentApiModel>> GetAllIntents();
    }
}
