using System.Configuration;
using Autofac;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using Microsoft.WindowsAzure.Storage;

namespace oiat.saferinternetbot.Web
{
    public class BotModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var connection = ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString;
            var store = new TableBotDataStore(connection);

            Conversation.UpdateContainer(
                builder1 =>
                {
                    builder1.Register(c => store)
                        .Keyed<IBotDataStore<BotData>>(AzureModule.Key_DataStore)
                        .AsSelf()
                        .SingleInstance();

                    builder1.Register(c => new CachingBotDataStore(store, CachingBotDataStoreConsistencyPolicy.ETagBasedConsistency))
                        .As<IBotDataStore<BotData>>()
                        .AsSelf()
                        .InstancePerLifetimeScope();
                });
        }
    }
}