using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using mbit.common.logging;
using oiat.saferinternetbot.Business.Dtos;
using oiat.saferinternetbot.Business.Interfaces;
using oiat.saferinternetbot.web.Models;

namespace oiat.saferinternetbot.web.Controllers
{
    [Authorize]
    public class AnswerController : BaseController
    {
        private static readonly IMcLogger _logger = McLogFactory.GetCurrentLogger();

        private readonly IAnswerService _answerService;
        private readonly IIntentService _intentService;
        private readonly IMapper _mapper;

        public AnswerController(IAnswerService answerService, IMapper mapper, IIntentService intentService)
        {
            _answerService = answerService;
            _mapper = mapper;
            _intentService = intentService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var intent = await _intentService.GetById(id);
            var items = await _answerService.GetByIntentId(id);

            var model = new AnswerListViewModel
            {
                IntentId = intent.Id,
                IntentName = intent.Name,
                Items = _mapper.Map<IEnumerable<AnswerListItemViewModel>>(items)
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult Create(Guid id)
        {
            return View(new AnswerEditViewModel { IntentId = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Guid id, AnswerEditViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    PushWarning("Antwort erstellen", "Bitte Eingaben überprüfen");
                    return View();
                }

                var dto = new AnswerDto
                {
                    IntentId = id,
                    Text = model.Text
                };
                await _answerService.Add(dto);

                PushSuccess("Antwort erstellen", "Antwort erfolgreich erstellt");
                return RedirectToAction("Index", new { id = id });
            }
            catch (Exception ex)
            {
                PushError("Antwort erstellen", "Fehler beim Erstellen der Antwort");
                _logger.Error(ex, "Error while creating answer");
                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id)
        {
            var answer = await _answerService.GetById(id);
            return View(_mapper.Map<AnswerEditViewModel>(answer));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, AnswerEditViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    PushWarning("Antwort bearbeiten", "Bitte Eingaben überprüfen");
                    return View();
                }

                var answer = await _answerService.GetById(id);
                answer.Text = model.Text;
                await _answerService.Update(id, answer);

                PushSuccess("Antwort bearbeiten", "Antwort erfolgreich gespeichert");
                return RedirectToAction("Index", new { id = model.IntentId });
            }
            catch (Exception ex)
            {
                PushError("Antwort bearbeiten", "Fehler beim Speichern der Antwort");
                _logger.Error(ex, $"Error while updating answer {id}");
                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Delete(Guid id)
        {
            var answer = await _answerService.GetById(id);
            await _answerService.Delete(id);
            PushSuccess("Antwort löschen", "Antwort erfolgreich gelöscht");
            return RedirectToAction("Index", new { id = answer.IntentId });
        }
    }
}