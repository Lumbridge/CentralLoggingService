using CLS.Core.Data;
using CLS.Infrastructure.Interfaces;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace CLS.Web.Controllers
{
    public class BaseController : Controller
    {
        protected IUnitOfWork _uow;

        public IUnitOfWork UnitOfWork { get => _uow; set => _uow = value; }

        public BaseController(IUnitOfWork uow)
        {
            _uow = uow;
        }
    }
}