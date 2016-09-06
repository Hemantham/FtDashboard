using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.API.Domain;

namespace DataEf.Maps
{
    public class DashboardViewMap : EntityTypeConfiguration<DashboardView>
    {
        public DashboardViewMap()
        {
            ToTable("DashboardView");

            HasKey(x => x.Id);
            Property(x => x.FieldOfInterest).HasMaxLength(100);
            Property(x => x.Name).HasMaxLength(300);
            Property(x => x.Code).HasMaxLength(50);
            HasOptional(x => x.Parent).WithOptionalDependent()
                .Map(x=> x.MapKey("ParentId"));
            HasMany(x => x.ViewSplits)
                .WithOptional()
                .Map(x=>x.MapKey("DashBoardViewId"));
        }
    }
}
