using System;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using oiat.saferinternetbot.Business.Dtos;
using oiat.saferinternetbot.Business.Interfaces;

namespace oiat.saferinternetbot.web.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            using (var scope = ((AutofacWebApiDependencyResolver) GlobalConfiguration.Configuration.DependencyResolver)
                .Container.BeginLifetimeScope())
            {
                try
                {
                    var scoreService = scope.Resolve<IScoreService>();
                    var answerService = scope.Resolve<IAnswerService>();

                    var activity = await result as Activity;

                    AnswerDto answer;

                    if (activity == null || string.IsNullOrWhiteSpace(activity.Text))
                    {
                        answer = await answerService.GetByInvalidMessage();
                    }
                    else
                    {
                        var score = await scoreService.GetScore(activity.Text);
                        answer = await answerService.GetRandomByIntent(score.IntentId);
                    }
                    await context.PostAsync(answer.Text);
                }
                catch (Exception ex)
                {
                    await context.PostAsync(ex.ToString());
                }

                context.Wait(MessageReceivedAsync);
            }
        }
    }
}