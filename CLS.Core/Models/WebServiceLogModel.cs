using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLS.Core.Models
{
    public class WebServiceLogModel
    {
        public string Exception { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public int PublishingSystemId { get; set; }
        public DateTime Timestamp { get; set; }
        public int SeverityId { get; set; }
    }
}
