using CLS.Core.Data;
using CLS.Infrastructure.Interfaces;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web.Http;

namespace CLS.Api.Controllers
{
    [RoutePrefix("CLS")]
    [Authorize]
    public class CLSController : BaseController
    {
        public CLSController(IUnitOfWork uow) : base(uow)
        {
        }

        [HttpGet]
        public string Get()
        {
            var userName = RequestContext.Principal.Identity.Name;
            return $"Hello, {userName}.";
        }

        [HttpPost]
        public string Post([FromBody]Log logMessage)
        {
            var user = _uow.Repository<AspNetUser>().FirstOrDefault(x => x.Email == RequestContext.Principal.Identity.Name);

            if (user == null) {
                return $"Unable to find a user with the credentials in the current context.";
            }

            try
            {
                logMessage.UserId = user.Id;
                _uow.Repository<Log>().Put(logMessage);
                _uow.Commit();
            }
            catch (Exception ex)
            {
                return $"{ex.Message}";
            }

            return "Successfully logged message.";
        }

        [HttpGet]
        [Route("Severity")]
        public string GetSeverity(string severityTypeCode)
        {
            var user = _uow.Repository<AspNetUser>().FirstOrDefault(x => x.Email == RequestContext.Principal.Identity.Name);
            if (user == null) {
                return $"Unable to find a user with the credentials in the current context.";
            }

            // get static data objects from database
            var severity = _uow.Repository<Severity>().First(x => x.Code == severityTypeCode);

            // return the publishing system
            return JsonConvert.SerializeObject(severity,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }

        [HttpGet]
        [Route("PublishingSystem")]
        public string GetPublishingSystem(string publishingSystemName, string environmentType, string systemType)
        {
            var user = _uow.Repository<AspNetUser>().FirstOrDefault(x => x.Email == RequestContext.Principal.Identity.Name);
            if (user == null)
            {
                return $"Unable to find a user with the credentials in the current context.";
            }

            // get static data objects from database
            var environmentTypeObj = _uow.Repository<EnvironmentType>().First(x => x.Name == environmentType);
            var systemTypeObj = _uow.Repository<PublishingSystemType>().First(x => x.Name == systemType);

            // check if publishing system exists in database
            var pSystem = _uow.Repository<PublishingSystem>().FirstOrDefault(x =>
                x.Name == publishingSystemName && x.EnvironmentTypeId == environmentTypeObj.Id &&
                x.PublishingSystemTypeId == systemTypeObj.Id);

            // if the publishing system doesn't exist in the database then create it
            if (pSystem == null)
            {
                var model = new PublishingSystem
                {
                    EnvironmentType = environmentTypeObj,
                    PublishingSystemType = systemTypeObj,
                    Name = publishingSystemName,
                    AspNetUser = user,
                    UserId = user.Id
                };
                _uow.Repository<PublishingSystem>().Put(model);
                model.EnvironmentTypeId = environmentTypeObj.Id;
                model.PublishingSystemTypeId = systemTypeObj.Id;
                _uow.Commit();

                pSystem = _uow.Repository<PublishingSystem>().FirstOrDefault(x =>
                    x.Name == publishingSystemName && x.EnvironmentTypeId == environmentTypeObj.Id &&
                    x.PublishingSystemTypeId == systemTypeObj.Id);
            }

            var publishingSystem = new PublishingSystem
            {
                Id = pSystem.Id,
                EnvironmentTypeId = pSystem.EnvironmentTypeId,
                PublishingSystemTypeId = pSystem.PublishingSystemTypeId,
                Name = pSystem.Name
            };

            var result = JsonConvert.SerializeObject(publishingSystem,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            // return the publishing system
            return result;
        }
    }
}
