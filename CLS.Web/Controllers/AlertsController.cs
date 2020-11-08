using CLS.Core.Data;
using CLS.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CLS.Core.StaticData;

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

        public JsonResult SaveAlert(string expression, string subscriberId, int alertTypeId)
        {
            var nodes = expression.Split(' ');
            var nodeList = new List<AlertTriggerNode>();
            var operatorList = _uow.Repository<AlertTriggerNodeOperator>().ToList();
            for (var i = 0; i < nodes.Length; i++)
            {
                var node = nodes[i];
                var staticOperator = operatorList.FirstOrDefault(x => x.DotNetProperty == node || x.Value == node);

                nodeList.Add(staticOperator == null
                    ? new AlertTriggerNode {DynamicNodeValue = node, PositionInGroup = i}
                    : new AlertTriggerNode
                    {
                        AlertTriggerNodeOperator = staticOperator, AlertTriggerNodeOperatorId = staticOperator.Id,
                        PositionInGroup = i
                    });
            }

            if (nodeList.Any(x => x.AlertTriggerNodeOperator?.Value == "PublishingSystemName"))
            {
                var publishingSystemNodes = nodeList.Where(x => x.AlertTriggerNodeOperator?.Value == "PublishingSystemName").ToList();
                foreach (var publishingSystemNode in publishingSystemNodes)
                {
                    var nameValueNode = nodeList[nodeList.IndexOf(publishingSystemNode) + 2];
                    _uow.Repository<Subscription>().Put(new Subscription
                    {
                        UserId = subscriberId,
                        AlertTriggerGroup = new AlertTriggerGroup
                        {
                            UserId = subscriberId,
                            AlertTriggerNodes = nodeList
                        },
                        AlertTypeId = alertTypeId,
                        IsActive = true,
                        PublishingSystemId = _uow.Repository<PublishingSystem>()
                            .First(x => x.Name == nameValueNode.DynamicNodeValue).Id
                    });
                }
            }
            else
            {
                _uow.Repository<Subscription>().Put(new Subscription
                {
                    UserId = subscriberId,
                    AlertTriggerGroup = new AlertTriggerGroup
                    {
                        UserId = subscriberId,
                        AlertTriggerNodes = nodeList
                    },
                    AlertTypeId = alertTypeId,
                    IsActive = true
                });
            }

            try
            {
                _uow.Commit();
            }
            catch (Exception ex)
            {
                _ls.Log(StaticData.SeverityType.Error, ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

            return Json(
                new
                {
                    success = true,
                    view = RenderPartialViewToString("_SubscriptionTable", _uow.Repository<Subscription>().ToList())
                }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteAlert(int subscriptionId)
        {
            var subscription = _uow.Repository<Subscription>().Get(subscriptionId);
            var alertTriggerGroup = _uow.Repository<AlertTriggerGroup>().Get(subscription.AlertTriggerGroupId);
            _uow.Repository<Subscription>().CascadingDelete(subscription);
            _uow.Repository<AlertTriggerGroup>().CascadingDelete(alertTriggerGroup);
            try
            {
                _uow.Commit();
            }
            catch (Exception ex)
            {
                _ls.Log(StaticData.SeverityType.Error, ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                success = true,
                view = RenderPartialViewToString("_SubscriptionTable", _uow.Repository<Subscription>().ToList())
            }, JsonRequestBehavior.AllowGet);
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
                case "DayOfWeek":
                {
                    options = new List<string> {"Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"};
                    break;
                }
            }

            return Json(new {success = true, options}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSubscriptionRow()
        {
            PopulateSelectListViewData();
            return Json(new {success = true, view = RenderPartialViewToString("_SubscriptionRow", new Subscription())},
                JsonRequestBehavior.AllowGet);
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
            ViewData["AlertTypes"] = _uow.Repository<AlertType>()
                .Select(o => new SelectListItem { Text = o.Name, Value = o.Id.ToString() }).ToList();

            //ViewData["Subscribers"] = _uow.Repository<CLSUser>()
            //    .Select(o => new SelectListItem { Text = o.Email, Value = o.Id.ToString()}).ToList();
            
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
    }
}