using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using oiat.whatsapp.functions.DTOS;
using Microsoft.Azure.Cosmos.Table;
using oiat.saferinternetbot.Business.Dtos;
using oiat.saferinternetbot.Business.Extensions;
using oiat.whatsapp.functions.Infrastructure;

namespace oiat.whatsapp.functions
{
    public static class ReceiveMessageFunction
    {
        public const string VERIFICATION_TOKEN = "WhatsApp@MBIT2020";
        public const string SECRET = "SaferInternetBot#2020";
        public const int SPAMMESSAGETIMEOUT = 15;

        [FunctionName("ReceiveMessage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Queue("received-messages")] ICollector<string> messages,
            ILogger log)
        {
            log.LogInformation($"ReceiveMessageFunction: Start processing new request.");

            if (!req.Headers.ContainsKey("Authorization") || req.Headers["Authorization"][0] != $"Bearer {SECRET}")
            {
                if (!req.Headers.ContainsKey("Authorization"))
                    log.LogWarning("Request without Authorization Header!");
                else
                    log.LogWarning($"Request with Authorization Header {req.Headers["Authorization"][0]}!");

                return new UnauthorizedResult();
            }

            using var sr = new StreamReader(req.Body);
            string requestBody = await sr.ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            if (data.sender != null && data.payload != null)
            {
                log.LogInformation($"ReceiveMessageFunction: Request is Message-Request.");
                messages.Add(requestBody);
                return new OkObjectResult(new { success = true });
            }
            else if (data.challenge != null && data.verification_token != null)
            {
                log.LogInformation($"ReceiveMessageFunction: Request is Challenge-Request.");
                if (data.verification_token == VERIFICATION_TOKEN)
                {
                    log.LogInformation($"ReceiveMessageFunction: Verification for Challenge-Request successful.");
                    return new OkObjectResult(new { data.challenge });
                }

                return new UnauthorizedResult();
            }

            return new BadRequestResult();
        }
    }
}
