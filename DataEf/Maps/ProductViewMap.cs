
using System.Data.Entity.ModelConfiguration;
using Dashboard.API.Domain;

namespace DataEf.Maps
{
    public class ProductViewMap : EntityTypeConfiguration<ProductView>
    {
        public ProductViewMap()
        {
            ToTable("ProductView");

            HasKey(x => x.Id);
            HasRequired(x => x.DashboardView).WithMany().Map(x => x.MapKey("DashboardId"));
            HasRequired(x => x.Product).WithMany().Map(x => x.MapKey("ProductId"));
            HasMany(x => x.ViewSplits)
                .WithOptional()
                .Map(x=>x.MapKey("ProductViewId"));
          
        }
    }
}
