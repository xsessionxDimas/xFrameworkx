using AutoMapper;
using Framework.Core.DTO;
using Framework.Core.Model;

namespace Framework.IoC.AutoMapper
{
    public class MasterProfile : Profile
    {
        public MasterProfile()
        {
            CreateMap<Bank, BankDTO>().ReverseMap();
            CreateMap<BankBranch, BankBranchDTO>().ReverseMap();
        }
    }
}
