using System.Data.Entity;


namespace DashboardMaster.Domain.DataContext
{
    public class DashboardContext : DbContext
    {
       
        public DashboardContext( string client)  : base("name=" + client )
        {
           
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //if (dashboardType == Dashtype.brand)
            //{
            //    modelBuilder.Entity<BrandChartValue>().Map(m =>
            //    {
            //        m.ToTable("vChartValue");
            //    });
            //    modelBuilder.Entity<BrandChartValue>().un
            //}
            //else 
            //{

            //modelBuilder.Entity<FChartValue>().Map(m =>
            //{
            //    m.ToTable("vChartValue");
            //});
            //}
        }
    }
}
