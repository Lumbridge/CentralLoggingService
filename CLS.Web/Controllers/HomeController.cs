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
            int minRefreshSeconds = 15;
            var metadata = new List<DashboardMetadata>();
            var logRepo = _uow.Repository<Log>();
            // ==============
            // message counts
            // ==============
            var DebugMessageCount = GetMetadata("DebugMessageCount");
            metadata.Add(SecondsAgo(DebugMessageCount?.TimeAdded) > minRefreshSeconds
                ? StoreMetadata("DebugMessageCount", logRepo.Count(x => x.Severity.Name == "Debug"))
                : DebugMessageCount);
            var InfoMessageCount = GetMetadata("InfoMessageCount");
            metadata.Add(SecondsAgo(InfoMessageCount?.TimeAdded) > minRefreshSeconds
                ? StoreMetadata("InfoMessageCount", logRepo.Count(x => x.Severity.Name == "Info"))
                : InfoMessageCount);
            var WarnMessageCount = GetMetadata("WarnMessageCount");
            metadata.Add(SecondsAgo(WarnMessageCount?.TimeAdded) > minRefreshSeconds
                ? StoreMetadata("WarnMessageCount", logRepo.Count(x => x.Severity.Name == "Warn"))
                : WarnMessageCount);
            var ErrorMessageCount = GetMetadata("ErrorMessageCount");
            metadata.Add(SecondsAgo(ErrorMessageCount?.TimeAdded) > minRefreshSeconds
                ? StoreMetadata("ErrorMessageCount", logRepo.Count(x => x.Severity.Name == "Error"))
                : ErrorMessageCount);
            var FatalMessageCount = GetMetadata("FatalMessageCount");
            metadata.Add(SecondsAgo(FatalMessageCount?.TimeAdded) > minRefreshSeconds
                ? StoreMetadata("FatalMessageCount", logRepo.Count(x => x.Severity.Name == "Fatal"))
                : FatalMessageCount);
            // ====================
            // most recent messages
            // ====================
            var MostRecentDebugMessage = GetMetadata("MostRecentDebugMessage");
            metadata.Add(SecondsAgo(MostRecentDebugMessage?.TimeAdded) > minRefreshSeconds
                ? StoreMetadata("MostRecentDebugMessage",
                    logRepo.Where(x => x.Severity.Name == "Debug").OrderByDescending(x => x.Timestamp)
                        .FirstOrDefault()?.Timestamp.ToString(CultureInfo.InvariantCulture) ?? "Never")
                : MostRecentDebugMessage);
            var MostRecentInfoMessage = GetMetadata("MostRecentInfoMessage");
            metadata.Add(SecondsAgo(MostRecentInfoMessage?.TimeAdded) > minRefreshSeconds
                ? StoreMetadata("MostRecentInfoMessage",
                    logRepo.Where(x => x.Severity.Name == "Info").OrderByDescending(x => x.Timestamp)
                        .FirstOrDefault()?.Timestamp.ToString(CultureInfo.InvariantCulture) ?? "Never")
                : MostRecentInfoMessage);
            var MostRecentWarnMessage = GetMetadata("MostRecentWarnMessage");
            metadata.Add(SecondsAgo(MostRecentWarnMessage?.TimeAdded) > minRefreshSeconds
                ? StoreMetadata("MostRecentWarnMessage",
                    logRepo.Where(x => x.Severity.Name == "Warn").OrderByDescending(x => x.Timestamp)
                        .FirstOrDefault()?.Timestamp.ToString(CultureInfo.InvariantCulture) ?? "Never")
                : MostRecentWarnMessage);
            var MostRecentErrorMessage = GetMetadata("MostRecentErrorMessage");
            metadata.Add(SecondsAgo(MostRecentErrorMessage?.TimeAdded) > minRefreshSeconds
                ? StoreMetadata("MostRecentErrorMessage",
                    logRepo.Where(x => x.Severity.Name == "Error").OrderByDescending(x => x.Timestamp)
                        .FirstOrDefault()?.Timestamp.ToString(CultureInfo.InvariantCulture) ?? "Never")
                : MostRecentErrorMessage);
            var MostRecentFatalMessage = GetMetadata("MostRecentFatalMessage");
            metadata.Add(SecondsAgo(MostRecentFatalMessage?.TimeAdded) > minRefreshSeconds
                ? StoreMetadata("MostRecentFatalMessage",
                    logRepo.Where(x => x.Severity.Name == "Fatal").OrderByDescending(x => x.Timestamp)
                        .FirstOrDefault()?.Timestamp.ToString(CultureInfo.InvariantCulture) ?? "Never")
                : MostRecentFatalMessage);
            return metadata;
        }

        public static int SecondsAgo(DateTime? time)
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