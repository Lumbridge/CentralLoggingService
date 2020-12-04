using CLS.Core.StaticData;
using CLS.Infrastructure.Interfaces;
using CLS.Sender.Classes;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using CLS.Core.Data;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace CLS.Web.Controllers
{
    public class BaseController : Controller
    {
        protected IUnitOfWork _uow;
        protected LogSender _ls;
        protected ApplicationSignInManager _signInManager;
        protected ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager => _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();

        public ApplicationUserManager UserManager => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        public IUnitOfWork UnitOfWork { get => _uow; set => _uow = value; }
        
        public BaseController(IUnitOfWork uow)
        {
            _uow = uow;
            _ls = new LogSender(StaticData.EnvironmentType.DEV, StaticData.SystemType.Website);
        }

        public AspNetUser CurrentUser(IPrincipal user) => _uow.Repository<AspNetUser>().FirstOrDefault(x => x.UserName == user.Identity.Name);

        // renders a partial view to a html string
        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }

        #region Account Management Helpers
        // Used for XSRF protection when adding external logins
        protected const string XsrfKey = "XsrfId";

        protected IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        protected void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        protected bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            return user?.PasswordHash != null;
        }

        protected bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            return user?.PhoneNumber != null;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}