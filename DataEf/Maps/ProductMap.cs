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
    public class ProductMap : EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            ToTable("Product");

            HasKey(x => x.Id);
            Property(x => x.Name).HasMaxLength(300).IsRequired();
            Property(x => x.Code).HasMaxLength(200).IsRequired()
                 .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Code") { IsUnique = true }));
            HasRequired(x => x.Filter).WithOptional().Map(x=> x.MapKey("FiterId"));
            HasMany(x => x.ProductViews).WithRequired(x => x.Product).Map(x => x.MapKey("ProductId"));
        }
    }
}
