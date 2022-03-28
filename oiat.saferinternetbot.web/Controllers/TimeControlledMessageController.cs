using AutoMapper;
using mbit.common.logging;
using oiat.saferinternetbot.Business.Dtos;
using oiat.saferinternetbot.Business.Interfaces;
using oiat.saferinternetbot.DataAccess.Enums;
using oiat.saferinternetbot.web.Controllers;
using oiat.saferinternetbot.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace oiat.saferinternetbot.Web.Controllers
{
    [Authorize]
    public class TimeControlledMessageController : BaseController
    {
        private static readonly IMcLogger _logger = McLogFactory.GetCurrentLogger();

        private readonly IMapper _mapper;
        private readonly ITimeControlledMessageService _timeControlledMessageService;

        public TimeControlledMessageController(ITimeControlledMessageService timeControlledMessageService, IMapper mapper)
        {
            _timeControlledMessageService = timeControlledMessageService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TimeControlledMessageViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    PushWarning("Zeitgesteuerte Nachricht erstellen", "Bitte Eingaben überprüfen");
                    return View(model);
                }

                var dto = _mapper.Map<TimeControlledMessageDto>(model);
                await _timeControlledMessageService.AddMessage(dto);
                
                PushSuccess("Zeitgesteuerte Nachricht erstellen", "Zeitgesteuerte Nachricht erfolgreich erstellt");
                return RedirectToAction("Index", "DefaultAnswer", new { type = (int)DefaultAnswerType.TimeRestrictedMessage });
            }
            catch (Exception ex)
            {
                PushError("Zeitgesteuerte Nachricht erstellen", "Fehler beim Erstellen der Zeitgesteuerte Nachricht");
                _logger.Error(ex, "Error while creating answer");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id)
        {
            var msg = await _timeControlledMessageService.GetMessageByID(id);
            return View(_mapper.Map<TimeControlledMessageViewModel>(msg));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, TimeControlledMessageViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    PushWarning("Zeitgesteuerte Nachricht bearbeiten", "Bitte Eingaben überprüfen");
                    return View(model);
                }

                var msg = await _timeControlledMessageService.GetMessageByID(id);
                _mapper.Map(model, msg);
                await _timeControlledMessageService.UpdateMessage(msg);

                PushSuccess("Zeitgesteuerte Nachricht bearbeiten", "Zeitgesteuerte Nachricht erfolgreich gespeichert");
                return RedirectToAction("Index", "DefaultAnswer", new { type = (int)DefaultAnswerType.TimeRestrictedMessage });
            }
            catch (Exception ex)
            {
                PushError("Zeitgesteuerte Nachricht bearbeiten", "Fehler beim Speichern der Zeitgesteuerte Nachricht");
                _logger.Error(ex, $"Error while updating answer {id}");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Delete(Guid id)
        {
            var msg = await _timeControlledMessageService.GetMessageByID(id);
            await _timeControlledMessageService.DeleteMessage(id);
            PushSuccess("Zeitgesteuerte Nachricht löschen", "Zeitgesteuerte Nachricht erfolgreich gelöscht");
            return RedirectToAction("Index", "DefaultAnswer", new { type = (int)DefaultAnswerType.TimeRestrictedMessage });
        }
    }
}