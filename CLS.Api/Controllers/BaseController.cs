using System.Web.Http;
using System.Web.Mvc;
using CLS.Infrastructure.Interfaces;

namespace CLS.Api.Controllers
{
    public class BaseController : ApiController
    {
        protected IUnitOfWork _uow;

        public IUnitOfWork UnitOfWork { get => _uow; set => _uow = value; }

        public BaseController(IUnitOfWork uow)
        {
            _uow = uow;
        }
    }
}