using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Core.DTO;
using Framework.Core.Interface.Repository;
using Framework.Core.Interface.Service;
using Framework.Core.Interface.UnitOfWork;
using Framework.Core.Model;
using AutoMapper;
using Framework.Core.Interface.Specification;
using Framework.Core.Specification;
using Framework.Utility.Paging;

namespace Framework.Service
{
    public class BankService : IBankService
    {
        private readonly IUnitOfWorkManager unitOfWorkManager;
        private readonly IRepository<Bank, int> bankRepository;
        private readonly IMapper mapper;

        public BankService(IUnitOfWorkManager unitOfWorkManager, IRepository<Bank, int> bankRepository, IMapper mapper)
        {
            this.unitOfWorkManager = unitOfWorkManager;
            this.bankRepository    = bankRepository;
            this.mapper            = mapper;
        }

        public void CreateNewBank(string bankCode, string bankName)
        {
            try
            {

                using (var unitOfWork = unitOfWorkManager.NewUnitOfWork())
                {
                    try
                    {
                        var bank = new Bank(bankCode, bankName, null, false, "System", DateTime.Now, null, null);
                        bankRepository.Add(bank);
                        unitOfWork.Commit();
                    }
                    catch (Exception)
                    {
                        unitOfWork.Rollback();
                        throw;
                    }
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                var sb = new StringBuilder();
                foreach (var validationError in dbEx.EntityValidationErrors.SelectMany(entityValidationErrors => entityValidationErrors.ValidationErrors))
                {
                    sb.Append("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                }
                throw new Exception(sb.ToString());
            }
        }

        public void UpdateBank(BankDTO bank)
        {
            try
            {

                using (var unitOfWork = unitOfWorkManager.NewUnitOfWork())
                {
                    try
                    {
                        var entity = mapper.Map<Bank>(bank);
                        bankRepository.Add(entity);
                        unitOfWork.Commit();
                    }
                    catch (Exception)
                    {
                        unitOfWork.Rollback();
                        throw;
                    }
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                var sb = new StringBuilder();
                foreach (var validationError in dbEx.EntityValidationErrors.SelectMany(entityValidationErrors => entityValidationErrors.ValidationErrors))
                {
                    sb.Append("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                }
                throw new Exception(sb.ToString());
            }
        }

        public BankDTO SearchBankById(int id)
        {
            var bank = bankRepository.FindOne(new BankByIdSpec(id));
            return mapper.Map<BankDTO>(bank);
        }

        public async Task<PagedList<BankDTO>> SearchBanks(string code, string name, int page, int pageSize)
        {
            ISpecification<Bank> specification = new AllBankSpec();
            if (!string.IsNullOrEmpty(code))
                specification   = specification.And(new BankByCodeSpec(code));
            if (!string.IsNullOrEmpty(name))
                specification   = specification.And(new BankByNameSpec(name));
            var totalRecords    = await Task.Run(() => bankRepository.Count(specification));
            var requests        = bankRepository.Find(specification).ToPagedList(page, pageSize, totalRecords);
            var result          = mapper.Map<PagedList<BankDTO>>(requests);
            result.PageCount    = requests.PageCount;
            result.TotalCount   = requests.TotalCount;
            result.Page         = requests.Page;
            result.PageSize     = requests.PageSize;
            return result;
        }
    }
}
