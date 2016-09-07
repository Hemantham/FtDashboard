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
            Property(x => x.SplitField).HasMaxLength(100).IsRequired();
            Property(x => x.SplitName).HasMaxLength(100).IsRequired();
            HasOptional(x => x.Filter).WithOptionalDependent().Map(x=> x.MapKey("FilterId"));
        }
    }
}
