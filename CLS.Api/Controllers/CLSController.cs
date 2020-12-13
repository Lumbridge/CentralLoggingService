using CLS.Core.Data;
using CLS.Infrastructure.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CLS.Core.Models;
using CLS.Infrastructure.Helpers;

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
            return $"You are authenticated as {userName}.";
        }

        [HttpPost]
        public string Post([FromBody]WebServiceLogModel logMessage)
        {
            var user = _uow.Repository<AspNetUser>().FirstOrDefault(x => x.Email == RequestContext.Principal.Identity.Name);

            if (user == null) {
                return $"Unable to find a user with the credentials in the current context.";
            }

            try
            {
                var exceptionHash = HashHelper.Hash(logMessage.Exception);
                var stackTraceHash = HashHelper.Hash(logMessage.StackTrace);
                var messageHash = HashHelper.Hash(logMessage.Message);

                var exceptionIndex =
                    _uow.Repository<LogIndexException>().FirstOrDefault(x => x.ExceptionHash == exceptionHash) ??
                    _uow.Repository<LogIndexException>().Put(new LogIndexException
                    {
                        Exception = logMessage.Exception,
                        ExceptionHash = exceptionHash
                    });

                var stackTraceIndex =
                    _uow.Repository<LogIndexStackTrace>().FirstOrDefault(x => x.StackTraceHash == stackTraceHash) ??
                    _uow.Repository<LogIndexStackTrace>().Put(new LogIndexStackTrace
                    {
                        StackTrace = logMessage.StackTrace,
                        StackTraceHash = stackTraceHash
                    });

                var messageIndex =
                    _uow.Repository<LogIndexMessage>().FirstOrDefault(x => x.MessageHash == messageHash) ?? 
                    _uow.Repository<LogIndexMessage>().Put(new LogIndexMessage
                        {
                            Message = logMessage.Message,
                            MessageHash = messageHash
                        });
                
                _uow.Commit();

                var model = new Log
                {
                    ExceptionId = exceptionIndex.Id,
                    StackTraceId = stackTraceIndex.Id,
                    MessageId = messageIndex.Id,
                    PublishingSystemId = logMessage.PublishingSystemId,
                    Timestamp = DateTime.Now,
                    SeverityId = logMessage.SeverityId,
                    UserId = user.Id
                };

                _uow.Repository<Log>().Put(model);

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
                var publishingSystemModel = new PublishingSystem
                {
                    EnvironmentTypeId = environmentTypeObj.Id,
                    PublishingSystemTypeId = systemTypeObj.Id,
                    Name = publishingSystemName
                };

                var publishingSystemOwnerModel = new PublishingSystemOwner
                {
                    UserId = user.Id,
                    PublishingSystem = publishingSystemModel
                };

                _uow.Repository<PublishingSystemOwner>().Put(publishingSystemOwnerModel);

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
