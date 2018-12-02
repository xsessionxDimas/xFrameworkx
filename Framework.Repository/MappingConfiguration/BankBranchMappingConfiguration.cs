using System.Data.Entity.ModelConfiguration;
using Framework.Core.Model;

namespace Framework.Repository.MappingConfiguration
{
    public class BankBranchMappingConfiguration : EntityTypeConfiguration<BankBranch>
    {
        public BankBranchMappingConfiguration()
        {
            ToTable("BankBranchTable");
            HasKey(k => k.Id);
            HasRequired(k => k.Bank).WithMany(p => p.Branches).HasForeignKey(k => k.BankId);
        }
    }
}
