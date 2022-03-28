using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace oiat.saferinternetbot.DataAccess.Common
{
    public abstract class McEntityTypeConfiguration<T> : IMcEntityTypeConfiguration<T> where T : class
    {
        public abstract void Map(EntityTypeConfiguration<T> b);

        public void Map(DbModelBuilder b)
        {
            Map(b.Entity<T>());
        }
    }
}
