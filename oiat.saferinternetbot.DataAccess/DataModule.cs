using System.Data.Entity;
using Autofac;
using mbit.common.dal;
using mbit.common.dal.Repositories;
using mbit.common.dal.UnitOfWork;
using oiat.saferinternetbot.DataAccess.Repositories;

namespace oiat.saferinternetbot.DataAccess
{
    public class DataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DataContext>().As<DbContext>().InstancePerLifetimeScope();
            builder.RegisterType<EfUnitOfWork>().As<IUnitOfWork>().As<IContextProvider<DbContext>>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
        }
    }
}
