using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CLS.Core.Data;
using CLS.Core.StaticData;
using CLS.Infrastructure.Interfaces;
using CLS.Sender.Classes;

namespace CLS.Web.Controllers
{
    public class SubscribersController : BaseController
    {
        public SubscribersController(IUnitOfWork uow) : base(uow)
        {
        }

        // GET: Subscribers
        public ActionResult Index()
        {
            var model = _uow.Repository<Subscriber>().ToList();
            return View(model);
        }

        public JsonResult SaveSubscriber(Subscriber model)
        {
            _uow.Repository<Subscriber>().Put(model);

            try
            {
                _uow.Commit();
            }
            catch (Exception ex)
            {
                _ls.LogErrorToDb(ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

            return Json(
                new
                {
                    success = true,
                    view = RenderPartialViewToString("_SubscriberTable", _uow.Repository<Subscriber>().ToList())
                }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteSubscriber(int subscriberId)
        {
            var subscriber = _uow.Repository<Subscriber>().Get(subscriberId);
            var subscriptions = _uow.Repository<Subscription>().Where(x => x.SubscriberId == subscriberId);
            foreach (var subscription in subscriptions)
            {
                var alertTriggerGroup = _uow.Repository<AlertTriggerGroup>().Get(subscription.AlertTriggerGroupId);
                _uow.Repository<Subscription>().CascadingDelete(subscription);
                _uow.Repository<AlertTriggerGroup>().CascadingDelete(alertTriggerGroup);
            }
            _uow.Repository<Subscriber>().CascadingDelete(subscriber);
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
                view = RenderPartialViewToString("_SubscriberTable", _uow.Repository<Subscriber>().ToList())
            }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetCreateUpdateSubscriberForm(int id = 0)
        {
            if (id > 0)
            {
                var model = _uow.Repository<Subscriber>().Get(id);
                return PartialView("_CreateUpdateSubscriber", model);
            }

            return PartialView("_CreateUpdateSubscriber", new Subscriber());
        }
    }
}