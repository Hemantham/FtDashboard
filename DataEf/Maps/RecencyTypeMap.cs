using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.API.Domain;

namespace DataEf.Maps
{
    public class RecencyTypeMap : EntityTypeConfiguration<RecencyType>
    {
        public RecencyTypeMap()
        {
            ToTable("RecencyType");

            HasKey(x => x.Id);
            Property(x => x.Name).HasMaxLength(100);
           
        }
    }
}
