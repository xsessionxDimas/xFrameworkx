using System.Web.Mvc;

namespace Framework.WebUI.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult PostLogin(string code)
        {
            var googleAuthenticator = new Auth.GoogleAuth();
            var googleProfile       = googleAuthenticator.AuthWithGoogle(code, @"http://localhost:1025/Login/PostLogin");
            return RedirectToAction("Index", "Bank");
        }
    }
}