using System;
using Framework.Core.Enum;

namespace Framework.Core.Model
{
    public class AuditTrail
    {
        public int Id                { get; protected set; }
        public AuditAction AuditType { get; protected set; }
        public string EntityName     { get; protected set; }
        public string EntityKey      { get; protected set; }
        public string OldValue       { get; protected set; }
        public string NewValue       { get; protected set; }
        public DateTime Date         { get; protected set; }
        public string UserName       { get; protected set; }

        public AuditTrail(AuditAction type, string entityName, string key)
        {
            AuditType   = type;
            EntityName  = entityName;
            EntityKey   = key;
            Date        = DateTime.Now;
        }

        public void SetUserName(string username)
        {
            UserName = username;
        }

        public void SetNewValue(string value)
        {
            NewValue = value;
        }

        public void SetOldValue(string value)
        {
            OldValue = value;
        }
    }
}
