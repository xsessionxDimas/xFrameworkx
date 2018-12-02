using System.Data.Entity.ModelConfiguration;
using Framework.Core.Model;

namespace Framework.Repository.MappingConfiguration
{
    public class BankMappingConfiguration : EntityTypeConfiguration<Bank>
    {
        public BankMappingConfiguration()
        {
            ToTable("BankTable");
            HasKey(k => k.Id);
        }
    }
}
