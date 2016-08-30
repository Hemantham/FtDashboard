using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.API.Domain;

namespace DataEf.Maps
{
    public class QuestionGroupMap : EntityTypeConfiguration<QuestionGroup>
    {
        public QuestionGroupMap()
        {
            ToTable("QuestionGroup");

            HasKey(x => x.Id);
            Property(x => x.Code).HasMaxLength(100);
            Property(x => x.Text).HasMaxLength(300);
        }
    }
}
