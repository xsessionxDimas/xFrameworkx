using System;

namespace Framework.Core.Model.Base
{
    public abstract class BaseEntity
    {
        /* no implementation needed */    
    }

    public abstract class BaseEntity<T> : BaseEntity
    {
        protected BaseEntity()
        {
            /* empty constructor */
        } 
        
        protected BaseEntity(bool isDelete, string createdBy, DateTime createdDate, string updatedBy, DateTime? updatedDate)
        {
            IsDelete    = isDelete;
            CreatedBy   = createdBy;
            CreatedDate = createdDate;
            UpdatedBy   = updatedBy;
            UpdatedDate = updatedDate;
        }

        public T Id                  { get; protected set; }
        public bool IsDelete         { get; protected set; }
        public string CreatedBy      { get; protected set; }
        public DateTime CreatedDate  { get; protected set; }
        public string UpdatedBy      { get; protected set; }
        public DateTime? UpdatedDate { get; protected set; }

        public override bool Equals(object entity)
        {
            if (!(entity is BaseEntity<T>))
                return false;
            return (this == (BaseEntity<T>) entity);
        }

        public static bool operator ==(BaseEntity<T> entityOne, BaseEntity<T> entityTwo)
        {
            if ((object) entityOne == null && (object) entityTwo == null)
            {
                return true;
            }
            if ((object)entityOne == null || (object)entityTwo == null)
            {
                return false;
            }
            return (object)entityOne.Id == (object)entityTwo.Id;
        }

        public static bool operator !=(BaseEntity<T> entityOne, BaseEntity<T> entityTwo)
        {
            return !(entityOne == entityTwo);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public void MarkAsDeleted(string user)
        {
            IsDelete    = true;
            UpdatedBy   = user;
            UpdatedDate = DateTime.Now;
        }
    }
}
