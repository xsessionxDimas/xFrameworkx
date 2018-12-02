using System.Collections.Generic;

namespace Framework.Core.DTO
{
    public class BankDTO : BaseEntityDTO
    {
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public IEnumerable<BankBranchDTO> Branches { get; set; }
    }
}
