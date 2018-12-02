using System.Data.Entity.ModelConfiguration;

namespace Framework.Repository.MappingConfiguration
{
    public class AuditTrailMappingConfiguration : EntityTypeConfiguration<Core.Model.AuditTrail>
    {
        public AuditTrailMappingConfiguration()
        {
            ToTable("AuditTrailTable");
            HasKey(k => k.Id);
        }
    }
}
