using System;
using System.Linq;
using System.Reflection;
using Autofac;
using AutoMapper;
using Module = Autofac.Module;

namespace oiat.saferinternetbot.Core
{
    public class MappingModule : Module
    {
        private readonly Type[] _types;

        public MappingModule(params Type[] types)
        {
            _types = types;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => Register(_types)).As<IMapper>().SingleInstance();
        }

        public static IMapper Register(params Type[] types)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMissingTypeMaps = false;
                cfg.AddProfiles(types);
            });

            return config.CreateMapper();
        }
    }
}
