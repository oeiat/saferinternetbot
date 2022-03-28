using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using mbit.common.dal.Entities;
using oiat.saferinternetbot.DataAccess.Common;

namespace oiat.saferinternetbot.DataAccess.Entities
{
    public class Answer : BaseEntity<Guid>
    {
        public Guid IntentId { get; set; }
        public string Text { get; set; }
    }

    public class AnswerEntityTypeConfiguration : McEntityTypeConfiguration<Answer>
    {
        public override void Map(EntityTypeConfiguration<Answer> b)
        {
            b.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            b.Property(x => x.IntentId).IsRequired();
            b.Property(x => x.Text).IsRequired();
        }
    }
}
