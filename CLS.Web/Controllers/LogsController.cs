using CLS.Core.Data;
using CLS.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CLS.Web.Controllers
{
    public class LogsController : BaseController
    {
        public LogsController(IUnitOfWork uow) : base(uow)
        {
        }

        // GET: Logs
        [Route("Logs/{logLevel?}")]
        public ActionResult Index(string logLevel = "all")
        {
            var validLevels = _uow.Repository<Severity>().Select(x => x.Name.ToLowerInvariant()).ToList();
            validLevels.Add("all");
            if (!validLevels.Contains(logLevel.ToLowerInvariant())) {
                logLevel = "all";
            }
            ViewData["logLevel"] = logLevel;
            var logs = new List<Log>();
            logs = logLevel == "all"
                ? _uow.Repository<Log>().OrderByDescending(x => x.Timestamp).ToList()
                : _uow.Repository<Log>().Where(x => string.Equals(x.Severity.Name, logLevel, StringComparison.InvariantCultureIgnoreCase)).OrderByDescending(x => x.Timestamp).ToList();
            return View(logs);
        }
    }
}