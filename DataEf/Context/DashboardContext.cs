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

            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DashboardContext>());
        }
    }
}
