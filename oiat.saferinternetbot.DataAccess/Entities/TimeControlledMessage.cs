using mbit.common.dal.Entities;
using oiat.saferinternetbot.DataAccess.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oiat.saferinternetbot.DataAccess.Entities
{
    public class TimeControlledMessage : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public bool Enabled { get; set; }

        public virtual ICollection<DefaultAnswer> DefaultAnswers { get; set; }
    }

    public class TimeControlledMessageEntityTypeConfiguration : McEntityTypeConfiguration<TimeControlledMessage>
    {
        public override void Map(EntityTypeConfiguration<TimeControlledMessage> b)
        {
            b.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            b.Property(x => x.Name).IsRequired();
            b.Property(x => x.Enabled).IsRequired();
            b.Property(x => x.StartTime).IsRequired();
            b.Property(x => x.EndTime).IsRequired();

            b.HasMany(x => x.DefaultAnswers)
                .WithOptional(x => x.TimeControlledMessage)
                .HasForeignKey(x => x.TimeControlledMessageId);
        }
    }
}
