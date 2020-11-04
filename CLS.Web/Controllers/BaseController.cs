using CLS.Infrastructure.Interfaces;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using CLS.Core.Data;
using CLS.Core.StaticData;
using CLS.Infrastructure.Helpers;
using CLS.Sender.Classes;
using CLS.Web.Models;

namespace CLS.Web.Controllers
{
    public class BaseController : Controller
    {
        protected IUnitOfWork _uow;

        protected LogSender _ls;

        public IUnitOfWork UnitOfWork { get => _uow; set => _uow = value; }

        public BaseController(IUnitOfWork uow)
        {
            _uow = uow;
            _ls = new LogSender("CLS.Web", StaticData.EnvironmentType.DEV, StaticData.SystemType.Website);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return Json(new {success = true, redirectUrl = Request.UrlReferrer.PathAndQuery}, JsonRequestBehavior.AllowGet);
        }

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
    }
}