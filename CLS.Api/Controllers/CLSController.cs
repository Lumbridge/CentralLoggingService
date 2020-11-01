using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CLS.Infrastructure.Interfaces;
using CLS.Core.Data;

namespace CLS.Api.Controllers
{
    public class CLSController : BaseController
    {
        public CLSController(IUnitOfWork uow) : base(uow)
        {
        }

        // GET api/values
        public IEnumerable<string> Get()
        {
            var test = _uow.Repository<Log>().Where(x => x.Timestamp.Date == DateTime.Today).Select(x => x.Id.ToString()).ToArray();
            return test;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
