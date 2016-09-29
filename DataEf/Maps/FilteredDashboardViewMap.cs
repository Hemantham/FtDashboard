
using System.Data.Entity.ModelConfiguration;
using Dashboard.API.Domain;

namespace DataEf.Maps
{
    public class FilteredDashboardViewMap : EntityTypeConfiguration<FilteredDashboardView>
    {
        public FilteredDashboardViewMap()
        {
            ToTable("FilteredDashboardView");

            HasKey(x => x.Id);
            HasRequired(x => x.DashboardView).WithMany(x=>x.ProductViews).Map(x => x.MapKey("DashboardId"));
            HasRequired(x => x.Filter).WithMany().Map(x => x.MapKey("ProductId"));
            HasMany(x => x.ViewSplits)
                .WithOptional()
                .Map(x=>x.MapKey("ProductViewId"));
          
        }
    }
}
