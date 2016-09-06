using System;
using System.Collections.Generic;
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
            Property(x => x.Name).HasMaxLength(300);
            Property(x => x.Code).HasMaxLength(50);
            HasRequired(x => x.Filter).WithOptional().Map(x=> x.MapKey("FiterId"));
            HasMany(x => x.DashboardViews)
                .WithMany(x=>x.Products)
                .Map(x =>
                    {
                        x.MapLeftKey("ProductId");
                        x.MapRightKey("DashboardViewId");
                    }
                );
        }
    }
}
