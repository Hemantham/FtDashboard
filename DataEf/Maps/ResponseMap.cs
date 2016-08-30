using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.API.Domain;

namespace DataEf.Maps
{
    public class ResponseMap : EntityTypeConfiguration<Response>
    {
        public ResponseMap()
        {
            ToTable("Response");

            HasKey(x => x.Id);
            HasRequired(x => x.Question);
            Property(x => x.Answer).HasMaxLength(500);
            Property(x => x.Email).HasMaxLength(100);
            Property(x => x.ResponseId).HasMaxLength(50);
        }
    }
}
