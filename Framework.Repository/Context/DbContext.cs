using System.Data.Entity;
using Framework.Repository.MappingConfiguration;

namespace Framework.Repository.Context
{
    public class DbContext : System.Data.Entity.DbContext
    {
        public DbContext() : base("name=FrameworkConnectionString")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BankMappingConfiguration());
            modelBuilder.Configurations.Add(new BankBranchMappingConfiguration());
        }
    }
}
