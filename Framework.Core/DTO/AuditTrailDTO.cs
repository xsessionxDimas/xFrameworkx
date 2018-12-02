using System;
using Framework.Core.Enum;

namespace Framework.Core.DTO
{
    public class AuditTrailDTO
    {
        public int Id                { get; set; }
        public AuditAction AuditType { get; set; }
        public string EntityName     { get; set; }
        public string EntityKey      { get; set; }
        public string OldValue       { get; set; }
        public string NewValue       { get; set; }
        public DateTime Date         { get; set; }
        public string UserName       { get; set; }
    }
}
