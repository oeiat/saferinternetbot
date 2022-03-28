using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using oiat.saferinternetbot.Business.Interfaces;
using oiat.saferinternetbot.web.Models;

namespace oiat.saferinternetbot.web.Controllers
{
    [Authorize]
    public class IntentController : BaseController
    {
        private readonly IIntentService _intentService;
        private readonly IMapper _mapper;

        public IntentController(IIntentService intentService, IMapper mapper)
        {
            _intentService = intentService;
            _mapper = mapper;
        }

        public async Task<ActionResult> Index()
        {
            var items = await _intentService.GetAll();
            return View(_mapper.Map<IEnumerable<IntentViewModel>>(items));
        }
    }
}