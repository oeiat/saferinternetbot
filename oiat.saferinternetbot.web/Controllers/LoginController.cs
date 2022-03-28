using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using oiat.saferinternetbot.Business.Identity;
using oiat.saferinternetbot.web.Models;

namespace oiat.saferinternetbot.web.Controllers
{
    public class LoginController : BaseController
    {
        private readonly ApplicationSignInManager _applicationSignInManager;
        private readonly IAuthenticationManager _authenticationManager;

        public LoginController(ApplicationSignInManager applicationSignInManager, IAuthenticationManager authenticationManager)
        {
            _applicationSignInManager = applicationSignInManager;
            _authenticationManager = authenticationManager;
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Intent");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _applicationSignInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

            if (result != SignInStatus.Success)
            {
                PushError("Login", "Die Logindaten sind nicht korrekt");
                return View();
            }

            PushSuccess("Login", "Login erfolgreich");
            return RedirectToAction("Index", "Intent");
        }

        public ActionResult Logout()
        {
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index");
        }
    }
}