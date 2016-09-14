using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.API.Domain;

namespace DataEf.Maps
{
    public class ViewSplitMap : EntityTypeConfiguration<ViewSplit>
    {
        public ViewSplitMap()
        {
            ToTable("ViewSplit");

            HasKey(x => x.Id);
            HasRequired(x => x.Question).WithMany().Map(x=> x.MapKey("QuestionId")) ;
            Property(x => x.SplitName).HasMaxLength(100).IsRequired();
            HasOptional(x => x.Filter).WithMany().Map(x=> x.MapKey("FilterId"));
        }
    }
}
