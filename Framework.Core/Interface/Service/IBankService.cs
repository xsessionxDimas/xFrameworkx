﻿using System.Threading.Tasks;
using Framework.Core.DTO;
using Framework.Utility.Paging;

namespace Framework.Core.Interface.Service
{
    public interface IBankService
    {
        void CreateNewBank(string bankCode, string bankName, string createdBy);
        void UpdateBank(BankDTO bank);
        void DeleteBank(int id, string updatedBy);
        BankDTO SearchBankById(int id);
        Task<PagedList<BankDTO>> SearchBanks(string code, string name, int page, int pageSize);
    }
}
