using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace oiat.saferinternetbot.DataAccess.Common
{
    public interface IMcEntityTypeConfiguration
    {
        void Map(DbModelBuilder b);
    }
    public interface IMcEntityTypeConfiguration<T> : IMcEntityTypeConfiguration where T : class
    {
        void Map(EntityTypeConfiguration<T> builder);
    }
}
