using CLS.Core.Data;
using System.Collections.Generic;
using System.Linq;

namespace CLS.UserWeb.Models
{
    public class DashboardModel
    {
        public List<DashboardMetadata> MetaData { get; set; }

        public int PublishingSystemCount { get; set; }
        public int SubscriberCount { get; set; }
        public int AlertCount { get; set; }
        public int AlertHistoryCount { get; set; }

        public int DebugMessageCount => int.Parse(MetaData.FirstOrDefault(x => x.MetadataItemName == "DebugMessageCount")?.MetadataItemValue ?? "0");
        public int InfoMessageCount => int.Parse(MetaData.FirstOrDefault(x => x.MetadataItemName == "InfoMessageCount")?.MetadataItemValue ?? "0");
        public int WarnMessageCount => int.Parse(MetaData.FirstOrDefault(x => x.MetadataItemName == "WarnMessageCount")?.MetadataItemValue ?? "0");
        public int ErrorMessageCount => int.Parse(MetaData.FirstOrDefault(x => x.MetadataItemName == "ErrorMessageCount")?.MetadataItemValue ?? "0");
        public int FatalMessageCount => int.Parse(MetaData.FirstOrDefault(x => x.MetadataItemName == "FatalMessageCount")?.MetadataItemValue ?? "0");

        public string MostRecentDebugMessage => MetaData.FirstOrDefault(x => x.MetadataItemName == "MostRecentDebugMessage")?.MetadataItemValue ?? "Never";
        public string MostRecentInfoMessage => MetaData.FirstOrDefault(x => x.MetadataItemName == "MostRecentInfoMessage")?.MetadataItemValue ?? "Never";
        public string MostRecentWarnMessage => MetaData.FirstOrDefault(x => x.MetadataItemName == "MostRecentWarnMessage")?.MetadataItemValue ?? "Never";
        public string MostRecentErrorMessage => MetaData.FirstOrDefault(x => x.MetadataItemName == "MostRecentErrorMessage")?.MetadataItemValue ?? "Never";
        public string MostRecentFatalMessage => MetaData.FirstOrDefault(x => x.MetadataItemName == "MostRecentFatalMessage")?.MetadataItemValue ?? "Never";
    }
}