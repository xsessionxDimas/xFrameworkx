namespace Framework.Core.DTO
{
    public class BankBranchDTO : BaseEntityDTO
    {
        public string BranchName { get; set; }
        public int BankId        { get; set; }
        public BankDTO Bank      { get; set; }
    }
}
