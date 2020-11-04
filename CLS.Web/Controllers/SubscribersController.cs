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
            var model = _uow.Repository<CLSUser>().ToList();
            return View(model);
        }
    }
}