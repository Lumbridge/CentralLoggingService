using CLS.Core.Data;
using CLS.Infrastructure.Interfaces;
using System;
using System.Linq;
using System.Web.Mvc;
using CLS.Core.StaticData;

namespace CLS.Web.Controllers
{
    [Authorize]
    public class PublishingSystemsController : BaseController
    {
        public PublishingSystemsController(IUnitOfWork uow) : base(uow)
        {
        }

        public ActionResult Index()
        {
            var model = _uow.Repository<PublishingSystem>().Where(x => x.UserOwnsSystem(CurrentUser(User).Id) && !x.IsDeleted).ToList();
            return View(model);
        }

        public ActionResult PublishingSystemOwners(int publishingSystemId)
        {
            var model = _uow.Repository<PublishingSystemOwner>().Where(x => x.PublishingSystemId == publishingSystemId).ToList();
            return View(model);
        }

        public PartialViewResult GetCreateUpdatePublishingSystemOwnerForm(string publishingSystemName, int id = 0)
        {
            if (id > 0)
            {
                var model = _uow.Repository<PublishingSystemOwner>()
                    .FirstOrDefault(x =>
                        x.PublishingSystemOwnersId == id && x.PublishingSystem.UserOwnsSystem(CurrentUser(User).Id));
                return PartialView("_CreateUpdatePublishingSystemOwner", model);
            }

            var publishingSystem =
                _uow.Repository<PublishingSystem>().FirstOrDefault(x => x.Name == publishingSystemName);

            if (publishingSystem == null) {
                return null;
            }

            return PartialView("_CreateUpdatePublishingSystemOwner", new PublishingSystemOwner {PublishingSystem = new PublishingSystem {Id = publishingSystem.Id, Name = publishingSystem.Name}});
        }

        public ActionResult AddPublishingSystemOwner(string publishingSystemName, string username)
        {
            var user = _uow.Repository<AspNetUser>().FirstOrDefault(x => x.UserName == username);

            if (user == null)
            {
                return Json(new {success = false, message = "A user with that email was not found."},
                    JsonRequestBehavior.AllowGet);
            }

            var publishingSystemOwnerRepo = _uow.Repository<PublishingSystemOwner>();

            if (publishingSystemOwnerRepo.Any(x => x.PublishingSystem.Name == publishingSystemName && x.UserId == user.Id))
            {
                return Json(new { success = false, message = "That user is already an owner of the selected publishing system." },
                    JsonRequestBehavior.AllowGet);
            }

            var publishingSystem = _uow.Repository<PublishingSystem>().First(x => x.Name == publishingSystemName);

            publishingSystemOwnerRepo.Put(new PublishingSystemOwner
            {
                PublishingSystemId = publishingSystem.Id,
                UserId = user.Id
            });

            try
            {
                _uow.Commit();
            }
            catch (Exception ex)
            {
                return Json(new {success = false, message = "Unable to add new publishing system owner."},
                    JsonRequestBehavior.AllowGet);
            }

            return Json(
                new
                {
                    success = true,
                    view = RenderPartialViewToString("_PublishingSystemOwnersTable",
                        _uow.Repository<PublishingSystemOwner>().Where(x => x.PublishingSystemId == publishingSystem.Id).ToList())
                }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemovePublishingSystemOwner(string publishingSystemName, string username)
        {
            var user = _uow.Repository<AspNetUser>().FirstOrDefault(x => x.UserName == username);

            if (user == null)
            {
                return Json(new { success = false, message = "A user with that email was not found." }, JsonRequestBehavior.AllowGet);
            }

            var publishingSystemOwnerRepo = _uow.Repository<PublishingSystemOwner>();

            if (publishingSystemOwnerRepo.Any(x => x.PublishingSystem.Name == publishingSystemName && x.UserId == user.Id))
            {
                var publishingSystem = _uow.Repository<PublishingSystem>().First(x => x.Name == publishingSystemName);

                if (publishingSystem.PublishingSystemOwners.Count == 1)
                {
                    return Json(new { success = false, message = "Error, you cannot have less than 1 publishing system owner." },
                        JsonRequestBehavior.AllowGet);
                }

                publishingSystemOwnerRepo.Delete(publishingSystemOwnerRepo.First(x => x.AspNetUser.UserName == username && x.PublishingSystem.Name == publishingSystemName));

                try
                {
                    _uow.Commit();
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Unable to add new publishing system owner." },
                        JsonRequestBehavior.AllowGet);
                }

                return Json(
                    new
                    {
                        success = true,
                        view = RenderPartialViewToString("_PublishingSystemOwnersTable",
                            _uow.Repository<PublishingSystemOwner>().Where(x => x.PublishingSystemId == publishingSystem.Id))
                    }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false, message = "The user was already removed from this publishing system owner list." }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeletePublishingSystem(int publishingSystemId)
        {
            var publishingSystem = _uow.Repository<PublishingSystem>()
                .FirstOrDefault(x => x.Id == publishingSystemId && x.UserOwnsSystem(CurrentUser(User).Id) && !x.IsDeleted);
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
                        _uow.Repository<PublishingSystem>().Where(x => x.UserOwnsSystem(CurrentUser(User).Id) && !x.IsDeleted).ToList())
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                success = false,
                view = RenderPartialViewToString("_PublishingSystemsTable",
                    _uow.Repository<PublishingSystem>().Where(x => x.UserOwnsSystem(CurrentUser(User).Id) && !x.IsDeleted).ToList())
            }, JsonRequestBehavior.AllowGet);
        }
    }
}