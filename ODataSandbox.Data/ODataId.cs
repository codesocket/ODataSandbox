using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODataSandbox.Data
{
    public class ODataId
    {
        [JsonProperty("@odata.id")]
        public string Url { get; set; }
    }
}