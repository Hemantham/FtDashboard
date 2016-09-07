using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using Dashboard.API.Domain;

namespace DataEf.Maps
{
    public class QuestionMap : EntityTypeConfiguration<Question>
    {
        public QuestionMap()
        {
            ToTable("Question");

            HasKey(x => x.Id);
            HasOptional(x => x.QuestionGroup)
                .WithOptionalDependent()
                .Map(x=> x.MapKey("QuestionGroupId"));
            Property(x => x.Code).HasMaxLength(200)
               .IsRequired().HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Code") { IsUnique = true }));
            Property(x => x.Text).HasMaxLength(300);
        }
    }
}
