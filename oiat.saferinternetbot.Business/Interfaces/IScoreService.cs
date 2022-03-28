using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using oiat.saferinternetbot.Business.Dtos;

namespace oiat.saferinternetbot.Business.Interfaces
{
    public interface IScoreService : IService
    {
        Task<ScoreDto> GetScore(string query);
    }
}
