using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace oiat.saferinternetbot.DataAccess.Common
{
    public static class ModelBuilderExtensions
    {
        public static void AddEntityConfigurationsFromAssembly(this DbModelBuilder modelBuilder, Assembly assembly)
        {
            var mappingTypes = assembly.GetMappingTypes(typeof(IMcEntityTypeConfiguration<>));
            foreach (var config in mappingTypes.Select(Activator.CreateInstance).Cast<IMcEntityTypeConfiguration>())
            {
                config.Map(modelBuilder);
            }
        }

        private static IEnumerable<Type> GetMappingTypes(this Assembly assembly, Type mappingInterface)
        {
            return assembly.GetTypes().Where(x => !x.IsAbstract && x.GetInterfaces().Any(y => y.GetTypeInfo().IsGenericType && y.GetGenericTypeDefinition() == mappingInterface));
        }

    }
}
