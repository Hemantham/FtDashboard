using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.API.Domain;

namespace DataEf.Maps
{
    public class FilterMap : EntityTypeConfiguration<Filter>
    {
        public FilterMap()
        {
            ToTable("Filter");

            HasKey(x => x.Id);
            Property(x => x.Name).HasMaxLength(100);
            Property(x => x.FilterString).HasMaxLength(1000);
        }
    }
}
