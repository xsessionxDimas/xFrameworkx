using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Framework.Core.Enum;

namespace Framework.Repository.AuditTrail
{
    public class AuditTrailFactory
    {
        private readonly DbContext context;
        private readonly IList<Core.Model.AuditTrail> auditData;
        private readonly string[] TypicalCreatedByNaming = {"CreatedBy"};
        private readonly string[] TypicalUpdatedByNaming = {"UpdatedBy"};


        public AuditTrailFactory(DbContext context)
        {
            this.context = context;
            auditData    = new List<Core.Model.AuditTrail>();
        }

        public IEnumerable<Core.Model.AuditTrail> GetAudit(DbEntityEntry entry)
        {
            IEnumerable<Core.Model.AuditTrail> data = null;
            switch (entry.State)
            {
                case EntityState.Added:
                    data = SetAddedProperties(entry);
                    break;
                case EntityState.Deleted:
                    data = SetDeletedProperties(entry);
                    break;
                case EntityState.Modified:
                    data = SetModifiedProperties(entry);
                    break;
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return data;
        }

        private IEnumerable<Core.Model.AuditTrail> SetAddedProperties(DbEntityEntry entry)
        {
            var audit        = new Core.Model.AuditTrail(AuditAction.Create, GetTableName(entry), GetKeyValue(entry));
            var keyValuePair = new List<string>();
            foreach (var propertyName in entry.CurrentValues.PropertyNames)
            {
                var newVal = entry.CurrentValues[propertyName];
                if (newVal == null) continue;
                if (TypicalCreatedByNaming.Contains(propertyName, StringComparer.CurrentCultureIgnoreCase))
                {
                    audit.SetUserName(newVal.ToString());
                }
                var key = propertyName;
                var value = newVal.GetType().Name.Equals("Byte[]", StringComparison.CurrentCultureIgnoreCase) ? "BinaryAttachment" : newVal.ToString();
                keyValuePair.Add($"{key}:{value}");
            }
            audit.SetNewValue(string.Join(";", keyValuePair));
            auditData.Add(audit);
            return auditData;
        }

        private IEnumerable<Core.Model.AuditTrail> SetDeletedProperties(DbEntityEntry entry)
        {
            var audit        = new Core.Model.AuditTrail(AuditAction.Delete, GetTableName(entry), GetKeyValue(entry));
            var dbValues     = entry.GetDatabaseValues();
            var keyValuePair = (from propertyName in dbValues.PropertyNames let oldVal = dbValues[propertyName] where oldVal != null let key = propertyName let value = oldVal.GetType().Name.Equals("Byte[]", StringComparison.CurrentCultureIgnoreCase) ? "BinaryAttachment" : oldVal.ToString() select $"{key}:{value}").ToList();
            foreach (var newVal in from propertyName in entry.OriginalValues.PropertyNames let newVal = entry.OriginalValues[propertyName] where newVal != null where TypicalUpdatedByNaming.Contains(propertyName, StringComparer.CurrentCultureIgnoreCase) select newVal)
            {
                audit.SetUserName(newVal.ToString());
                break;
            }
            audit.SetOldValue(string.Join(";", keyValuePair));
            auditData.Add(audit);
            return auditData;
        }

        private IEnumerable<Core.Model.AuditTrail> SetModifiedProperties(DbEntityEntry entry)
        {
            var audit        = new Core.Model.AuditTrail(AuditAction.Update, GetTableName(entry), GetKeyValue(entry));
            var dbValues     = entry.GetDatabaseValues();
            var keyValuePair = (from propertyName in dbValues.PropertyNames let oldVal = dbValues[propertyName] where oldVal != null let key = propertyName let value = oldVal.GetType().Name.Equals("Byte[]", StringComparison.CurrentCultureIgnoreCase) ? "BinaryAttachment" : oldVal.ToString() select $"{key}:{value}").ToList();
            audit.SetOldValue(string.Join(";", keyValuePair));
            keyValuePair     = new List<string>();
            foreach (var propertyName in entry.CurrentValues.PropertyNames)
            {
                var newVal = entry.CurrentValues[propertyName];
                if (newVal == null) continue;
                if (TypicalCreatedByNaming.Contains(propertyName, StringComparer.CurrentCultureIgnoreCase))
                {
                    audit.SetUserName(newVal.ToString());
                }
                var key = propertyName;
                var value = newVal.GetType().Name.Equals("Byte[]", StringComparison.CurrentCultureIgnoreCase) ? "BinaryAttachment" : newVal.ToString();
                keyValuePair.Add($"{key}:{value}");
            }
            audit.SetNewValue(string.Join(";", keyValuePair));
            auditData.Add(audit);
            return auditData;
        }

        public string GetKeyValue(DbEntityEntry entry)
        {
            var objectStateEntry = ((IObjectContextAdapter) context).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
            return objectStateEntry.EntityKey.EntityKeyValues?[0].Value.ToString() ?? string.Empty;
        }

        private static string GetTableName(DbEntityEntry dbEntry)
        {
            return System.Data.Entity.Core.Objects.ObjectContext.GetObjectType(dbEntry.Entity.GetType()).Name;
        }
    }
}
