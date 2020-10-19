using System;
using System.Collections.Generic;
using System.Globalization;
using CLS.Core.Data;
using CLS.Infrastructure.Interfaces;
using CLS.Web.Models;
using System.Linq;
using System.Web.Mvc;

namespace CLS.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IUnitOfWork uow) : base(uow)
        {
        }

        public ActionResult Index()
        {
            var model = new DashboardModel
            {
                PublishingSystemCount = _uow.Repository<PublishingSystem>().Count(),
                SubscriberCount = _uow.Repository<Subscriber>().Count(),
                AlertCount = _uow.Repository<Subscription>().Count(),
                MetaData = GetDashboardMetadata()
            };
            return View(model);
        }

        public List<DashboardMetadata> GetDashboardMetadata()
        {
            var metadata = new List<DashboardMetadata>();
            var logRepo = _uow.Repository<Log>();
            // ==============
            // message counts
            // ==============
            var DebugMessageCount = GetMetadata("DebugMessageCount");
            metadata.Add(MinutesAgo(DebugMessageCount?.TimeAdded) > 2
                ? StoreMetadata("DebugMessageCount", logRepo.Count(x => x.Severity.Name == "Debug"))
                : DebugMessageCount);
            var InfoMessageCount = GetMetadata("InfoMessageCount");
            metadata.Add(MinutesAgo(InfoMessageCount?.TimeAdded) > 2
                ? StoreMetadata("InfoMessageCount", logRepo.Count(x => x.Severity.Name == "Info"))
                : InfoMessageCount);
            var WarnMessageCount = GetMetadata("WarnMessageCount");
            metadata.Add(MinutesAgo(WarnMessageCount?.TimeAdded) > 2
                ? StoreMetadata("WarnMessageCount", logRepo.Count(x => x.Severity.Name == "Warn"))
                : WarnMessageCount);
            var ErrorMessageCount = GetMetadata("ErrorMessageCount");
            metadata.Add(MinutesAgo(ErrorMessageCount?.TimeAdded) > 2
                ? StoreMetadata("ErrorMessageCount", logRepo.Count(x => x.Severity.Name == "Error"))
                : ErrorMessageCount);
            var FatalMessageCount = GetMetadata("FatalMessageCount");
            metadata.Add(MinutesAgo(FatalMessageCount?.TimeAdded) > 2
                ? StoreMetadata("FatalMessageCount", logRepo.Count(x => x.Severity.Name == "Fatal"))
                : FatalMessageCount);
            // ====================
            // most recent messages
            // ====================
            var MostRecentDebugMessage = GetMetadata("MostRecentDebugMessage");
            metadata.Add(MinutesAgo(MostRecentDebugMessage?.TimeAdded) > 2
                ? StoreMetadata("MostRecentDebugMessage",
                    logRepo.Where(x => x.Severity.Name == "Debug").OrderByDescending(x => x.Timestamp)
                        .FirstOrDefault()?.Timestamp.ToString(CultureInfo.InvariantCulture) ?? "Never")
                : MostRecentDebugMessage);
            var MostRecentInfoMessage = GetMetadata("MostRecentInfoMessage");
            metadata.Add(MinutesAgo(MostRecentInfoMessage?.TimeAdded) > 2
                ? StoreMetadata("MostRecentInfoMessage",
                    logRepo.Where(x => x.Severity.Name == "Info").OrderByDescending(x => x.Timestamp)
                        .FirstOrDefault()?.Timestamp.ToString(CultureInfo.InvariantCulture) ?? "Never")
                : MostRecentInfoMessage);
            var MostRecentWarnMessage = GetMetadata("MostRecentWarnMessage");
            metadata.Add(MinutesAgo(MostRecentWarnMessage?.TimeAdded) > 2
                ? StoreMetadata("MostRecentWarnMessage",
                    logRepo.Where(x => x.Severity.Name == "Warn").OrderByDescending(x => x.Timestamp)
                        .FirstOrDefault()?.Timestamp.ToString(CultureInfo.InvariantCulture) ?? "Never")
                : MostRecentWarnMessage);
            var MostRecentErrorMessage = GetMetadata("MostRecentErrorMessage");
            metadata.Add(MinutesAgo(MostRecentErrorMessage?.TimeAdded) > 2
                ? StoreMetadata("MostRecentErrorMessage",
                    logRepo.Where(x => x.Severity.Name == "Error").OrderByDescending(x => x.Timestamp)
                        .FirstOrDefault()?.Timestamp.ToString(CultureInfo.InvariantCulture) ?? "Never")
                : MostRecentErrorMessage);
            var MostRecentFatalMessage = GetMetadata("MostRecentFatalMessage");
            metadata.Add(MinutesAgo(MostRecentFatalMessage?.TimeAdded) > 2
                ? StoreMetadata("MostRecentFatalMessage",
                    logRepo.Where(x => x.Severity.Name == "Fatal").OrderByDescending(x => x.Timestamp)
                        .FirstOrDefault()?.Timestamp.ToString(CultureInfo.InvariantCulture) ?? "Never")
                : MostRecentFatalMessage);
            return metadata;
        }

        public static int MinutesAgo(DateTime? time)
        {
            if (time == null) return int.MaxValue;
            return (int)Math.Round(DateTime.Now.Subtract(time.Value).TotalMinutes);
        }

        public DashboardMetadata StoreMetadata(string name, object value)
        {
            var metaRepo = _uow.Repository<DashboardMetadata>();

            // check if this metadata exists in the database
            var metadata = metaRepo.FirstOrDefault(x => x.MetadataItemName == name);

            // if the metadata exists then update it's value
            if (metadata != null)
            {
                metadata.MetadataItemValue = value.ToString();
                metaRepo.Put(metadata);
                _uow.Commit();
                return metadata;
            }

            // if the metadata doesn't exist then add it
            metadata = metaRepo.Put(new DashboardMetadata
            {
                MetadataItemName = name, 
                MetadataItemValue = value.ToString(),
                MetadataItemDotNetType = value.GetType().ToString(),
                TimeAdded = DateTime.Now
            });

            _uow.Commit();

            return metadata;
        }

        public DashboardMetadata GetMetadata(string name)
        {
            return _uow.Repository<DashboardMetadata>().FirstOrDefault(x => x.MetadataItemName == name);
        }
    }
}