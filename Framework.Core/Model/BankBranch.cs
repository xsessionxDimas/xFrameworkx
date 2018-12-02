using System;
using Framework.Core.Model.Base;

namespace Framework.Core.Model
{
    public class BankBranch : BaseEntity<int>
    {
        public string BranchName { get; protected set; }
        public int BankId        { get; protected set; }
        public Bank Bank         { get; protected set; }

        public BankBranch(string branchName, int bankId, Bank bank,
                    bool isDelete, string createdBy, DateTime createdDate, string updatedBy, DateTime? updatedDate)
            : base(isDelete, createdBy, createdDate, updatedBy, updatedDate)
        {
            BranchName = branchName;
            BankId     = bankId;
            Bank       = bank;
        }
    }
}
