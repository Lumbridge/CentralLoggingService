using CLS.Infrastructure.Interfaces;
using System.IO;
using System.Web.Mvc;

namespace CLS.Web.Controllers
{
    public class BaseController : Controller
    {
        protected IUnitOfWork _uow;

        public IUnitOfWork UnitOfWork { get => _uow; set => _uow = value; }

        public BaseController(IUnitOfWork uow)
        {
            _uow = uow;
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