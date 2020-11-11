using CLS.Core.Data;
using CLS.Infrastructure.Interfaces;
using System;
using System.Linq;
using System.Web.Mvc;
using CLS.Core.StaticData;

namespace CLS.UserWeb.Controllers
{
    [Authorize]
    public class PublishingSystemsController : BaseController
    {
        public PublishingSystemsController(IUnitOfWork uow) : base(uow)
        {
        }

        public ActionResult Index()
        {
            var model = _uow.Repository<PublishingSystem>().Where(x => CurrentUser(User).Id == x.UserId && !x.IsDeleted).ToList();
            return View(model);
        }

        public JsonResult DeletePublishingSystem(int publishingSystemId)
        {
            var publishingSystem = _uow.Repository<PublishingSystem>()
                .FirstOrDefault(x => x.Id == publishingSystemId && CurrentUser(User).Id == x.UserId && !x.IsDeleted);
            if (publishingSystem != null)
            {
                publishingSystem.IsDeleted = true;
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
                    view = RenderPartialViewToString("_PublishingSystemsTable",
                        _uow.Repository<PublishingSystem>().Where(x => CurrentUser(User).Id == x.UserId && !x.IsDeleted).ToList())
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                success = false,
                view = RenderPartialViewToString("_PublishingSystemsTable",
                    _uow.Repository<PublishingSystem>().Where(x => CurrentUser(User).Id == x.UserId && !x.IsDeleted).ToList())
            }, JsonRequestBehavior.AllowGet);
        }
    }
}