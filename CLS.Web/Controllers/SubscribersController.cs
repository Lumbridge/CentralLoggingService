using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CLS.Core.Data;
using CLS.Core.StaticData;
using CLS.Infrastructure.Helpers;
using CLS.Infrastructure.Interfaces;
using CLS.Sender.Classes;
using CLS.Web.Models;

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
            var model = _uow.Repository<User>().ToList();
            return View(model);
        }

        public JsonResult SaveSubscriber(RegisterModel model)
        {
            var userModel = new User
            {
                Email = model.Email,
                HashedPassword = EncryptionHelper.ComputeHash(model.Password)
            };

            _uow.Repository<User>().Put(userModel);

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
                    view = RenderPartialViewToString("_SubscriberTable", _uow.Repository<User>().ToList())
                }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteSubscriber(int subscriberId)
        {
            var user = _uow.Repository<User>().Get(subscriberId);
            var subscriptions = _uow.Repository<Subscription>().Where(x => x.UserId == subscriberId);
            foreach (var subscription in subscriptions)
            {
                var alertTriggerGroup = _uow.Repository<AlertTriggerGroup>().Get(subscription.AlertTriggerGroupId);
                _uow.Repository<Subscription>().CascadingDelete(subscription);
                _uow.Repository<AlertTriggerGroup>().CascadingDelete(alertTriggerGroup);
            }
            _uow.Repository<User>().CascadingDelete(user);
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
                view = RenderPartialViewToString("_SubscriberTable", _uow.Repository<User>().ToList())
            }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetCreateUpdateSubscriberForm(int id = 0)
        {
            if (id > 0)
            {
                var model = _uow.Repository<User>().Get(id);
                return PartialView("_CreateUpdateSubscriber", model);
            }

            return PartialView("_CreateUpdateSubscriber", new User());
        }
    }
}