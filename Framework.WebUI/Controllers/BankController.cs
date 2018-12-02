using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Framework.Core.DTO;
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

        public ViewResult AddNewBank()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewBank(BankDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);
            try
            {
                model.SetCreateNewLog("System");
                bankService.CreateNewBank(model.BankCode, model.BankName, "Dimas");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                AddErrors(new[] { ex.Message });
                return View(model);
            }
            
        }

        private void AddErrors(IEnumerable<string> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public async Task<JsonResult> SetBankDeleted(int id)
        {
            try
            {
                await Task.Run(() => bankService.DeleteBank(id, "SYSTEM"));
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}