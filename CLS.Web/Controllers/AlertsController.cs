using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CLS.Infrastructure.Interfaces;

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
            return View();
        }
    }
}