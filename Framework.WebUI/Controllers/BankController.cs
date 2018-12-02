using System.Threading.Tasks;
using System.Web.Mvc;
using Framework.Core.Interface.Service;

namespace Framework.WebUI.Controllers
{
    public class BankController : Controller
    {
        private readonly IBankService bankService;

        public BankController(IBankService bankService)
        {
            this.bankService = bankService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> GetBanks(string code, string name, int page = 1, int pageSize = 25)
        {
            var result = await Task.Run(() => bankService.SearchBanks(code, name, page, pageSize));
            return PartialView("ListBanks", result);
        }

        public async Task<ActionResult> SetBankDeleted(int id)
        {
            
        }
    }
}