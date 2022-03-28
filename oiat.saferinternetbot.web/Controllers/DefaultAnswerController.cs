using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using mbit.common.Extensions;
using mbit.common.logging;
using oiat.saferinternetbot.Business.Dtos;
using oiat.saferinternetbot.Business.Interfaces;
using oiat.saferinternetbot.DataAccess.Enums;
using oiat.saferinternetbot.web.Models;
using oiat.saferinternetbot.Web.Models;

namespace oiat.saferinternetbot.web.Controllers
{
    [Authorize]
    public class DefaultAnswerController : BaseController
    {
        private static readonly IMcLogger _logger = McLogFactory.GetCurrentLogger();

        private readonly IAnswerService _answerService;
        private readonly ITimeControlledMessageService _timeControlledMessageService;
        private readonly IMapper _mapper;

        public DefaultAnswerController(IAnswerService answerService, IMapper mapper, IIntentService intentService, ITimeControlledMessageService timeControlledMessageService)
        {
            _answerService = answerService;
            _mapper = mapper;
            _timeControlledMessageService = timeControlledMessageService;
        }

        public async Task<ActionResult> List()
        {
            var items = Enum.GetValues(typeof(DefaultAnswerType)).Cast<DefaultAnswerType>().Select(x => new DefaultAnswerTypeViewModel { Name = x.GetDisplayName(), Type = x });
            return View(items);
        }

        [HttpGet]
        public async Task<ActionResult> Index(DefaultAnswerType type, Guid? messageId = null)
        {
            var isEmptyTimeMessageList = type == DefaultAnswerType.TimeRestrictedMessage && messageId == null;

            ICollection<AnswerDto> items = await GetAnswerItems(type, messageId);
            IEnumerable<(Guid Id, string Name, bool Enabled)> messages = (!isEmptyTimeMessageList) ? null : (await _timeControlledMessageService.GetMessageInfos());

            var model = new DefaultAnswerListViewModel
            {
                Name = type.GetDisplayName(),
                Type = type,
                Items = (items != null) ? _mapper.Map<IEnumerable<AnswerListItemViewModel>>(items) : null,
                TimeControlledMessages = messages?.Select(x => new TimeControlledMessageListViewModel
                {
                    Enabled = x.Enabled,
                    Id = x.Id,
                    Name = x.Name
                }),
                TimeControlledMessageId = messageId
            };

            return View(model);
        }

        private async Task<ICollection<AnswerDto>> GetAnswerItems(DefaultAnswerType type, Guid? messageId)
        {
            if (type == DefaultAnswerType.TimeRestrictedMessage)
            {
                if (!messageId.HasValue)
                    return null;
                else
                    return await _answerService.GetByType(type, messageId.Value);
            }

            return await _answerService.GetByType(type);
        }

        [HttpGet]
        public ActionResult Create(int type, Guid? messageId = null)
        {
            return View(new DefaultAnswerEditViewModel { Type = type, TimeControlledMessageId = messageId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int type, DefaultAnswerEditViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    PushWarning("Antwort erstellen", "Bitte Eingaben überprüfen");
                    return View(model);
                }

                var dto = new AnswerDto
                {
                    Text = model.Text
                };
                await _answerService.AddDefault((DefaultAnswerType)type, dto, model.TimeControlledMessageId);

                PushSuccess("Antwort erstellen", "Antwort erfolgreich erstellt");
                return RedirectToAction("Index", new { type = type, messageId = model.TimeControlledMessageId });
            }
            catch (Exception ex)
            {
                PushError("Antwort erstellen", "Fehler beim Erstellen der Antwort");
                _logger.Error(ex, "Error while creating answer");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id, int type, Guid? messageId = null)
        {
            var answer = await _answerService.GetDefaultById(id);
            var model = _mapper.Map<DefaultAnswerEditViewModel>(answer);
            model.Type = type;
            model.TimeControlledMessageId = messageId;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, DefaultAnswerEditViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    PushWarning("Antwort bearbeiten", "Bitte Eingaben überprüfen");
                    return View();
                }

                var answer = await _answerService.GetDefaultById(id);
                answer.Text = model.Text;
                await _answerService.UpdateDefault(id, answer);

                PushSuccess("Antwort bearbeiten", "Antwort erfolgreich gespeichert");
                return RedirectToAction("Index", new { type = model.Type, messageId = model.TimeControlledMessageId });
            }
            catch (Exception ex)
            {
                PushError("Antwort bearbeiten", "Fehler beim Speichern der Antwort");
                _logger.Error(ex, $"Error while updating answer {id}");
                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Delete(Guid id, int type = 0, Guid? messageId = null)
        {
            await _answerService.DeleteDefault(id);
            PushSuccess("Antwort löschen", "Antwort erfolgreich gelöscht");
            return RedirectToAction("Index", "DefaultAnswer", new { type, messageId });
        }
    }
}