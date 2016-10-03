using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
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
            Property(x => x.Name).HasMaxLength(300).IsRequired();
            Property(x => x.FilterString).HasMaxLength(500);
            Property(x => x.Group).HasMaxLength(50);
            Property(x => x.Code).HasMaxLength(200).IsRequired()
                 .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Code")));
            HasMany(x => x.FilteredDashboardViews).WithRequired(x => x.Filter).Map(x => x.MapKey("ProductId"));
        }
    }
}
