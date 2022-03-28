using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using mbit.common.dal.Entities;
using oiat.saferinternetbot.DataAccess.Common;
using oiat.saferinternetbot.DataAccess.Enums;

namespace oiat.saferinternetbot.DataAccess.Entities
{
    public class DefaultAnswer : BaseEntity<Guid>
    {
        public DefaultAnswerType Type { get; set; }
        public string Text { get; set; }
        public Guid? TimeControlledMessageId { get; set; }

        public virtual TimeControlledMessage TimeControlledMessage { get; set; }
    }

    public class DefaultAnswerEntityTypeConfiguration : McEntityTypeConfiguration<DefaultAnswer>
    {
        public override void Map(EntityTypeConfiguration<DefaultAnswer> b)
        {
            b.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            b.Property(x => x.Type).IsRequired();
            b.Property(x => x.Text).IsRequired();
            b.Property(x => x.TimeControlledMessageId).IsOptional();
        }
    }
}
