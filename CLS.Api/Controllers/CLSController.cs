using CLS.Core.Data;
using CLS.Infrastructure.Interfaces;
using System;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CLS.Api.Controllers
{
    [RoutePrefix("CLS")]
    [Authorize]
    public class CLSController : BaseController
    {
        public CLSController(IUnitOfWork uow) : base(uow)
        {
        }

        public string Get()
        {
            var userName = RequestContext.Principal.Identity.Name;
            return $"Hello, {userName}.";
        }

        public string Post(string logMessageJson)
        {
            var clsUser = _uow.Repository<CLSUser>().FirstOrDefault(x => x.ApplicationUser.Email == RequestContext.Principal.Identity.Name);

            if (clsUser == null) {
                return $"Unable to find a user with the credentials in the current context.";
            }

            try
            {
                var logModel = JsonSerializer.Deserialize<Log>(logMessageJson);
                logModel.CLSUser = clsUser;
                logModel.CLSUserId = clsUser.Id;
                _uow.Repository<Log>().Put(logModel);
            }
            catch (Exception ex)
            {
                return $"{ex.Message}";
            }

            return "Successfully logged message";
        }

        [HttpGet]
        [Route("PublishingSystem")]
        public string GetPublishingSystem(string publishingSystemName, string environmentType, string systemType)
        {
            var clsUser = _uow.Repository<CLSUser>().FirstOrDefault(x => x.ApplicationUser.Email == RequestContext.Principal.Identity.Name);
            if (clsUser == null)
                return $"Unable to find a user with the credentials in the current context.";

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
                _uow.Repository<PublishingSystem>().Put(new PublishingSystem
                {
                    EnvironmentType = environmentTypeObj, 
                    PublishingSystemType = systemTypeObj,
                    Name = publishingSystemName,
                    CLSUserId = clsUser.Id,
                    CLSUser = clsUser
                });
                _uow.Commit();
            }

            // return the publishing system
            return JsonConvert.SerializeObject(pSystem);
        }
    }
}
