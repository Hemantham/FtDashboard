using System.Data.Entity;
using DataEf.Maps;

namespace DataEf.Context
{
    public class DashboardContext : DbContext
    {
        public DashboardContext(string connection)  : base("name=" + connection)
        {
           
        }

        public DashboardContext() : base("DefaultConnection")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new QuestionMap());
            modelBuilder.Configurations.Add(new ResponseMap());
            modelBuilder.Configurations.Add(new QuestionGroupMap());
            modelBuilder.Configurations.Add(new FilterMap());
            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new ViewSplitMap());
            modelBuilder.Configurations.Add(new DashboardViewMap());


            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DashboardContext>());
        }
    }
}
