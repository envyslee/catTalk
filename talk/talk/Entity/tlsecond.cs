using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace talk.Entity
{
   public  class tlsecond
    {
       [JsonProperty("name")]
       public string Name { get; set; }

       [JsonProperty("detailurl")]
       public string Detailurl { get; set; }
    }
}
