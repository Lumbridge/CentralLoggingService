using CLS.Core.Data;
using CLS.Infrastructure.Interfaces;
using Microsoft.Ajax.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CLS.UserWeb.Controllers
{
    [Authorize]
    public class AlertHistoryController : BaseController
    {
        public AlertHistoryController(IUnitOfWork uow) : base(uow)
        {
        }

        // GET: AlertHistory
        [Route("AlertHistory/{alertHistoryGroupId?}")]
        public ActionResult Index(int alertHistoryGroupId = 0)
        {
            var model = new List<Log>();

            if (_uow.Repository<AlertHistory>().All(x => x.AlertHistoryGroupId != alertHistoryGroupId))
            {
                ViewData["AlertHistories"] = _uow.Repository<AlertHistory>()
                    .Where(x => CurrentUser(User).Id == x.UserId).DistinctBy(x => x.AlertHistoryGroupId)
                    .OrderByDescending(x => x.Timestamp).ToList();
                return View(model: null);
            }

            var alertHistories = _uow.Repository<AlertHistory>()
                .Where(x => x.AlertHistoryGroupId == alertHistoryGroupId && x.UserId == CurrentUser(User).Id).ToList();

            if (alertHistories.Any())
            {
                model.AddRange(alertHistories.Select(alertHistory => alertHistory.Log));
            }

            ViewData["AlertHistoryRecord"] =
                alertHistories.FirstOrDefault(x => x.AlertHistoryGroupId == alertHistoryGroupId);

            return View(model);
        }
    }
}