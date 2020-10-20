using CLS.Core.Data;
using CLS.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CLS.Web.Controllers
{
    public class AlertsController : BaseController
    {
        public AlertsController(IUnitOfWork uow) : base(uow)
        {
        }

        // GET: Alerts
        public ActionResult Index()
        {
            var model = _uow.Repository<Subscription>().ToList();
            return View(model);
        }

        public JsonResult GetDynamicSelectList(string variableName)
        {
            var options = new List<string>();

            switch (variableName)
            {
                case "MessageSeverity":
                {
                    options = _uow.Repository<Severity>().Select(x => x.Name).ToList();
                    break;
                }
                case "PublishingSystemName":
                {
                    options = _uow.Repository<PublishingSystem>().Select(x => x.Name).ToList();
                    break;
                }
                case "EnvironmentType":
                {
                    options = _uow.Repository<EnvironmentType>().Select(x => x.Name).ToList();
                    break;
                }
            }

            return Json(new {success = true, options}, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetCreateUpdateAlertForm(int id = 0)
        {
            PopulateSelectListViewData();

            if (id > 0)
            {
                var model = _uow.Repository<Subscription>().Get(id);
                return PartialView("_CreateUpdateSubscription", model);
            }

            return PartialView("_CreateUpdateSubscription", new Subscription());
        }

        public void PopulateSelectListViewData()
        {
            ViewData["LogicalOperators"] = _uow.Repository<AlertTriggerNodeOperator>()
                .Where(x => x.AlertTriggerNodeType.Name == "LogicalOperator")
                .Select(o => new SelectListItem { Text = o.Value, Value = o.Value }).ToList();

            ViewData["ComparisonOperators"] = _uow.Repository<AlertTriggerNodeOperator>()
                .Where(x => x.AlertTriggerNodeType.Name == "ComparisonOperator")
                .Select(o => new SelectListItem { Text = o.Value, Value = o.Value }).ToList();

            ViewData["VariableNames"] = _uow.Repository<AlertTriggerNodeOperator>()
                .Where(x => x.AlertTriggerNodeType.Name == "VariableName" || x.AlertTriggerNodeType.Name == "DynamicVariable")
                .Select(o => new SelectListItem { Text = o.Value, Value = o.DotNetProperty }).ToList();
        }

        public JsonResult GetSubscriptionRow()
        {
            PopulateSelectListViewData();
            return Json(new {success = true, view = RenderPartialViewToString("_SubscriptionRow", new Subscription())},
                JsonRequestBehavior.AllowGet);
        }
    }
}