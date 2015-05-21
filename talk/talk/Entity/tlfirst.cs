using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace talk.Entity
{
    public class tlfirst
    {
        [JsonProperty("code")]
        public string Code { get;set;}

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

         [JsonProperty("list")]
        public List<tlsecond> List { get; set; }
    }
}
