using System;

namespace Framework.Core.DTO
{
    public abstract class BaseEntityDTO
    {
        public int Id                { get; set; }
        public bool IsDelete         { get; set; }
        public string CreatedBy      { get; set; }
        public DateTime CreatedDate  { get; set; }
        public string UpdatedBy      { get; set; }
        public DateTime? UpdatedDate { get; set; }
       
        public virtual void SetCreateNewLog(string createdBy)
        {
            CreatedBy   = createdBy;
            CreatedDate = DateTime.Now;
        }

        public virtual void SetUpdateExistingLog(string updatedBy)
        {
            UpdatedBy   = updatedBy;
            UpdatedDate = DateTime.Now;
        }
    }
}
