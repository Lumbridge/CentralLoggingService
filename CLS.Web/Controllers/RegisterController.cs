using CLS.Core.Data;
using CLS.Infrastructure.Helpers;
using CLS.Infrastructure.Interfaces;
using CLS.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace CLS.Web.Controllers
{
    public class RegisterController : BaseController
    {
        public RegisterController(IUnitOfWork uow) : base(uow)
        {
        }

        // GET: Register
        public ActionResult Index()
        {
            return View();
        }
    }
}