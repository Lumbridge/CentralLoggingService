using CLS.Core.Data;
using CLS.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CLS.Web.Controllers
{
    [Authorize]
    public class LogsController : BaseController
    {
        public LogsController(IUnitOfWork uow) : base(uow)
        {
        }

        [Route("Logs/{logLevel?}/{publishingSystemId?}/{environmentTypeId?}")]
        public ActionResult Index(string logLevel = "All", int publishingSystemId = 0, int environmentTypeId = 0)
        {
            var validLevels = _uow.Repository<Severity>().Select(x => x.Name.ToLowerInvariant()).ToList();
            validLevels.Add("All");
            if (!validLevels.Contains(logLevel.ToLowerInvariant()))
            {
                logLevel = "All";
            }
            ViewData["logLevel"] = logLevel;
            var logs = logLevel == "All"
                ? _uow.Repository<Log>().Where(x => x.PublishingSystem.UserOwnsSystem(CurrentUser(User).Id))
                    .OrderByDescending(x => x.Timestamp).ToList()
                : _uow.Repository<Log>()
                    .Where(x => x.PublishingSystem.UserOwnsSystem(CurrentUser(User).Id) && string.Equals(x.Severity.Name, logLevel,
                                    StringComparison.InvariantCultureIgnoreCase)).OrderByDescending(x => x.Timestamp)
                    .ToList();

            if (publishingSystemId != 0)
            {
                var publishingSystem = _uow.Repository<PublishingSystem>().FirstOrDefault(x => x.Id == publishingSystemId && x.UserOwnsSystem(CurrentUser(User).Id));
                if (publishingSystem != null)
                {
                    ViewData["publishingSystemName"] = publishingSystem.Name;
                    logs = logs.Where(x => string.Equals(x.PublishingSystem.Name, publishingSystem.Name,
                        StringComparison.InvariantCultureIgnoreCase)).ToList();
                }
            }

            if (environmentTypeId != 0)
            {
                ViewData["environmentTypeName"] = _uow.Repository<EnvironmentType>().Get(environmentTypeId).Name;
                logs = logs.Where(x => x.PublishingSystem.EnvironmentTypeId == environmentTypeId).ToList();
            }

            return View(logs);
        }
    }
}