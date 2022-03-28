using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using oiat.saferinternetbot.DataAccess.Common;
using oiat.saferinternetbot.DataAccess.Entities;

namespace oiat.saferinternetbot.DataAccess
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public virtual IDbSet<Answer> Answers { get; set; }
        public virtual IDbSet<DefaultAnswer> DefaultAnswers { get; set; }
        public virtual IDbSet<TimeControlledMessage> TimeControlledMessages { get; set; }

        public DataContext()
        { 
        }

        public DataContext(string connectionString)
            : base(connectionString)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Types().Configure(entity => entity.ToTable($"saferbot{entity.ClrType.Name}"));
            modelBuilder.AddEntityConfigurationsFromAssembly(GetType().Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
