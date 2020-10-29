using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CLS.Core.Data;
using CLS.Infrastructure.Interfaces;

namespace CLS.Web.Controllers
{
    public class PublishingSystemsController : BaseController
    {
        public PublishingSystemsController(IUnitOfWork uow) : base(uow)
        {
        }

        // GET: PublishingSystems
        public ActionResult Index()
        {
            var model = _uow.Repository<PublishingSystem>().ToList();
            return View(model);
        }

        public JsonResult DeletePublishingSystem(int publishingSystemId)
        {
            var publishingSystem = _uow.Repository<PublishingSystem>().Get(publishingSystemId);
            var publishingSystems = _uow.Repository<PublishingSystem>().Where(x => x.Id == publishingSystemId);
            foreach (var system in publishingSystems)
            {
                //var alertTriggerGroup = _uow.Repository<AlertTriggerGroup>().Get(system.AlertTriggerGroupId);
                //_uow.Repository<PublishingSystem>().CascadingDelete(system);
                //_uow.Repository<AlertTriggerGroup>().CascadingDelete(alertTriggerGroup);
            }
            _uow.Repository<PublishingSystem>().CascadingDelete(publishingSystem);
            try
            {
                _uow.Commit();
            }
            catch (Exception ex)
            {
                _ls.LogErrorToDb(ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                success = true,
                view = RenderPartialViewToString("_PublishingSystemsTable", _uow.Repository<PublishingSystem>().ToList())
            }, JsonRequestBehavior.AllowGet);
        }

        
    }
}