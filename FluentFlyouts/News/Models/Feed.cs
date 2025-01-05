using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFlyouts.News.Models
{
    public class Feed
    {
        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("nextPageUrl")]
        public string NextPageUrl { get; set; }

        [JsonProperty("nextRequestUrl")]
        public string NextRequestUrl { get; set; }

        [JsonProperty("landingPageUrl")]
        public string LandingPageUrl { get; set; }

        [JsonProperty("flyoutUrl")]
        public string FlyoutUrl { get; set; }

        [JsonProperty("expirationDateTime")]
        public DateTimeOffset ExpirationDateTime { get; set; }

        [JsonProperty("showBadge")]
        public bool ShowBadge { get; set; }

        [JsonProperty("settings")]
        public Dictionary<string, object> Settings { get; set; }

        [JsonProperty("dimensions")]
        public Dictionary<string, object> Dimensions { get; set; }

        [JsonProperty("template")]
        public string Template { get; set; }

        [JsonProperty("cards")]
        public List<Newtonsoft.Json.Linq.JObject> Cards { get; set; }
    }
}
