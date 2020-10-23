using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CLS.Core.Data;
using CLS.Infrastructure.Interfaces;
using Microsoft.Ajax.Utilities;

namespace CLS.Web.Controllers
{
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
                ViewData["AlertHistories"] = _uow.Repository<AlertHistory>().DistinctBy(x => x.AlertHistoryGroupId)
                    .OrderByDescending(x => x.Timestamp).ToList();
                return View(model: null);
            }

            var alertHistories = _uow.Repository<AlertHistory>().Where(x => x.AlertHistoryGroupId == alertHistoryGroupId).ToList();

            if (alertHistories.Any()) {
                model.AddRange(alertHistories.Select(alertHistory => alertHistory.Log));
            }

            ViewData["AlertHistoryRecord"] = alertHistories.FirstOrDefault(x => x.AlertHistoryGroupId == alertHistoryGroupId);

            return View(model);
        }
    }
}