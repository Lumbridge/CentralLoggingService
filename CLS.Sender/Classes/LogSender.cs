using CLS.Core.Data;
using CLS.Core.StaticData;
using CLS.Infrastructure.Classes;
using CLS.Infrastructure.Helpers;
using CLS.Infrastructure.Interfaces;
using CLS.Sender.Data;
using System;
using System.Linq;
using System.Reflection;

namespace CLS.Sender.Classes
{
    public class LogSender
    {
        public LogSender(PublishingSystem publishingSystem)
        {
            PublishingSystem = publishingSystem;
        }

        public LogSender(string publishingSystemName, StaticData.EnvironmentType environmentType, StaticData.SystemType systemType)
        {
            PublishingSystem = GetPublishingSystem(publishingSystemName, environmentType, systemType);
        }

        public LogSender(StaticData.EnvironmentType environmentType, StaticData.SystemType systemType)
        {
            PublishingSystem = GetPublishingSystem(Assembly.GetCallingAssembly().FullName, environmentType, systemType);
        }

        public PublishingSystem PublishingSystem { get; set; }
        
        public bool LogDebugToDb(Exception exception = null, string info = null)
        {
            var uow = new UnitOfWork(new DBEntities());

            PublishingSystem = uow.Repository<PublishingSystem>()
                          .FirstOrDefault(x => x.Name == PublishingSystem.Name
                                               && x.EnvironmentType.Code == PublishingSystem.EnvironmentType.Code) ?? PublishingSystem;

            uow.Repository<Log>().Put(new Log
            {
                Exception = exception?.GetExceptionMessages(),
                StackTrace = exception?.StackTrace,
                Message = info,
                PublishingSystem = PublishingSystem,
                Timestamp = DateTime.Now,
                Severity = uow.Repository<Severity>().First(x=> x.Code == "D")
            });

            return TryCommit(uow);
        }
        
        public bool LogInfoToDb(Exception exception = null, string info = null)
        {
            var uow = new UnitOfWork(new DBEntities());

            PublishingSystem = uow.Repository<PublishingSystem>()
                          .FirstOrDefault(x => x.Name == PublishingSystem.Name
                                               && x.EnvironmentType.Code == PublishingSystem.EnvironmentType.Code) ?? PublishingSystem;

            uow.Repository<Log>().Put(new Log
            {
                Exception = exception?.GetExceptionMessages(),
                StackTrace = exception?.StackTrace,
                Message = info,
                PublishingSystem = PublishingSystem,
                Timestamp = DateTime.Now,
                Severity = uow.Repository<Severity>().First(x => x.Code == "I")
            });

            return TryCommit(uow);
        }

        public bool LogWarningToDb(Exception exception = null, string info = null)
        {
            var uow = new UnitOfWork(new DBEntities());

            PublishingSystem = uow.Repository<PublishingSystem>()
                          .FirstOrDefault(x => x.Name == PublishingSystem.Name
                                               && x.EnvironmentType.Code == PublishingSystem.EnvironmentType.Code) ?? PublishingSystem;

            uow.Repository<Log>().Put(new Log
            {
                Exception = exception?.GetExceptionMessages(),
                StackTrace = exception?.StackTrace,
                Message = info,
                PublishingSystem = PublishingSystem,
                Timestamp = DateTime.Now,
                Severity = uow.Repository<Severity>().First(x => x.Code == "W")
            });

            return TryCommit(uow);
        }

        public bool LogErrorToDb(Exception exception = null, string info = null)
        {
            var uow = new UnitOfWork(new DBEntities());

            PublishingSystem = uow.Repository<PublishingSystem>()
                          .FirstOrDefault(x => x.Name == PublishingSystem.Name
                                               && x.EnvironmentType.Code == PublishingSystem.EnvironmentType.Code) ?? PublishingSystem;

            uow.Repository<Log>().Put(new Log
            {
                Exception = exception?.GetExceptionMessages(),
                StackTrace = exception?.StackTrace,
                Message = info,
                PublishingSystem = PublishingSystem,
                Timestamp = DateTime.Now,
                Severity = uow.Repository<Severity>().First(x => x.Code == "E")
            });

            return TryCommit(uow);
        }

        public bool LogFatalToDb(Exception exception = null, string info = null)
        {
            var uow = new UnitOfWork(new DBEntities());

            PublishingSystem = uow.Repository<PublishingSystem>()
                          .FirstOrDefault(x => x.Name == PublishingSystem.Name
                                               && x.EnvironmentType.Code == PublishingSystem.EnvironmentType.Code) ?? PublishingSystem;

            uow.Repository<Log>().Put(new Log
            {
                Exception = exception?.GetExceptionMessages(),
                StackTrace = exception?.StackTrace,
                Message = info,
                PublishingSystem = PublishingSystem,
                Timestamp = DateTime.Now,
                Severity = uow.Repository<Severity>().First(x => x.Code == "F")
            });

            return TryCommit(uow);
        }

        public PublishingSystem GetPublishingSystem(string systemName, StaticData.EnvironmentType environmentType, StaticData.SystemType systemType)
        {
            var uow = new UnitOfWork(new DBEntities());

            // get static data objects from database
            var environmentTypeObj = uow.Repository<EnvironmentType>().First(x => x.Name == environmentType.ToString());
            var systemTypeObj = uow.Repository<PublishingSystemType>().First(x => x.Name == systemType.ToString());

            // check if publishing system exists in database
            var pSystem = uow.Repository<PublishingSystem>().FirstOrDefault(x =>
                x.Name == systemName && x.EnvironmentTypeId == environmentTypeObj.Id &&
                x.PublishingSystemTypeId == systemTypeObj.Id);
            
            // if the publishing system doesn't exist in the database then create it
            if (pSystem == null)
            {
                pSystem = uow.Repository<PublishingSystem>().Put(new PublishingSystem { EnvironmentType = environmentTypeObj, PublishingSystemType = systemTypeObj, Name = systemName });
                TryCommit(uow);
            }

            // return the publishing system
            return pSystem;
        }

        private bool TryCommit(IUnitOfWork uow)
        {
            try
            {
                uow.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
