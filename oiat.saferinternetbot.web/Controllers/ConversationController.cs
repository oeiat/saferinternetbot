using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using mbit.common.logging;
using oiat.saferinternetbot.Business.Interfaces;
using oiat.saferinternetbot.web.Controllers;
using oiat.saferinternetbot.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace oiat.saferinternetbot.Web.Controllers
{
    [Authorize]
    public class ConversationController : BaseController
    {
        private static readonly IMcLogger _logger = McLogFactory.GetCurrentLogger();

        private readonly IConversationService _conversationService;
        private readonly IMapper _mapper;

        public ConversationController(IConversationService conversationService, IMapper mapper)
        {
            _conversationService = conversationService;
            _mapper = mapper;
        }

        // GET: Conversation
        public async Task<ActionResult> List()
        {
            var data = await _conversationService.GetAllConversationIds();
            return View(data.Select(x => new ConversationOverviewViewModel() { ConversationId = x.conversationId, MessageCount = x.msgCount, LatestTimestamp = x.latestTimestamp }));
        }

        public async Task<ActionResult> Export(string id)
        {
            var decodedID = Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(id));
            var data = await _conversationService.GetConversation(decodedID);

            var records = data.Select(x => new ConversationMessageExportModel
            {
                Content = x.Content,
                Timestamp = x.Timestamp.ToString("dd.MM.yyy HH:mm:ss"),
                Type = x.Channel.ToString()
            }).ToList();

            using (var ms = new MemoryStream())
            {
                using (var writer = new StreamWriter(ms, Encoding.UTF8))
                {                    
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.Configuration.Delimiter = ";";
                        csv.Configuration.NewLine = NewLine.Environment;
                        csv.Configuration.ShouldQuote = (field, context) => true;

                        await csv.WriteRecordsAsync(records);
                        await csv.FlushAsync();
                    }
                }
                return File(ms.ToArray(), "text/csv", $"Konversation_{id}.csv");
            }
        }

        public async Task<ActionResult> Messages(string id)
        {
            var decodedID = Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(id));

            var data = await _conversationService.GetConversation(decodedID);
            var model = new ConversationDetailViewModel()
            {
                ConversationId = decodedID,
                MessageCount = data.Count,
                Messages = data.Select(x => _mapper.Map<ConversationMessageViewModel>(x))
            };
            return View(model);
        }

        public async Task<ActionResult> Delete(string id)
        {
            var decodedID = id ?? "NULL"; 
            try
            {
                decodedID = Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(id));
                await _conversationService.DeleteConversation(decodedID);
                PushSuccess("Konversation löschen", "Konversation wurde erfolgreich gelöscht");
            }
            catch (Exception ex)
            {
                PushError("Konversation löschen", "Es ist ein Fehler beim löschen der Konversation aufgetreten!");
                _logger.Error(ex, $"Error while deleting conversation \"{decodedID}\"");
            }
            return RedirectToAction(nameof(List));
        }
    }
}