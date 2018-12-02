using System;
using System.Collections.Generic;
using Framework.Core.Model.Base;

namespace Framework.Core.Model
{
    /* no need to be virtual cause we implement dynamic includes */
    public class Bank : BaseEntity<int>
    {
        public string BankCode { get; protected set; }
        public string BankName { get; protected set; }
        public ICollection<BankBranch>  Branches { get; protected set; }

        public Bank()
        {
            Branches = new HashSet<BankBranch>();
        }

        public Bank(string bankCode, string bankName, ICollection<BankBranch> branches,
                    bool isDelete, string createdBy, DateTime createdDate, string updatedBy, DateTime? updatedDate)
            : base(isDelete, createdBy, createdDate, updatedBy, updatedDate)
        {
            BankCode = bankCode;
            BankName = bankName;
            Branches = branches ?? new HashSet<BankBranch>();
        }
    }
}
