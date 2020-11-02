using CLS.Core.Data;
using CLS.Infrastructure.Helpers;
using CLS.Infrastructure.Interfaces;
using CLS.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace CLS.Web.Controllers
{
    public class RegisterController : BaseController
    {
        public RegisterController(IUnitOfWork uow) : base(uow)
        {
        }

        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SaveUser(RegisterModel model)
        {
            if(_uow.Repository<User>().Any(x=> string.Equals(x.Email, model.Email, StringComparison.InvariantCultureIgnoreCase))) {
                return Json(new { success = false, message = "Error, an account with that email address is already registered." }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password)) {
                return Json(new { success = false, message = "Error, email or password cannot be blank." }, JsonRequestBehavior.AllowGet);
            }

            if (!RegexHelper.IsValidEmail(model.Email)) {
                return Json(new { success = false, message = "Error, email is not in valid format." }, JsonRequestBehavior.AllowGet);
            }

            var userModel = new User
            {
                Email = model.Email,
                HashedPassword = EncryptionHelper.ComputeHash(model.Password),
                RegistrationDate = DateTime.Now,
                LastLogin = DateTime.Now,
                EmailVerificationKey = Guid.NewGuid()
            };

            _uow.Repository<User>().Put(userModel);

            try
            {
                _uow.Commit();
            }
            catch (Exception ex)
            {
                _ls.LogErrorToDb(ex);
                return Json(new { success = false, message = ex.GetExceptionMessages() }, JsonRequestBehavior.AllowGet);
            }

            FormsAuthentication.SetAuthCookie(model.Email, false);

            // TODO: send email to user prompting account verification

            return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") }, JsonRequestBehavior.AllowGet);
        }
    }
}