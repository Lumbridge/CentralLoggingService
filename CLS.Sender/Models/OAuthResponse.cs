using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CLS.Sender.Models
{
    public class OAuthResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string userName { get; set; }
        [JsonProperty(".issued")]
        public DateTime issued { get; set; }
        [JsonProperty(".expires")]
        public DateTime expires { get; set; }
        public string error { get; set; }
        public string error_description { get; set; }
    }
}
