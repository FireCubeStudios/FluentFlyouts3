using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFlyouts.News.Models
{
    public class ArticleCard : Card
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("abstract")]
        public string Abstract { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("publishedDateTime")]
        public DateTimeOffset PublishedDateTime { get; set; }

        [JsonProperty("images")]
        public List<Image> Images { get; set; }

        [JsonProperty("provider")]
        public Provider Provider { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        // TODO
        [JsonProperty("reactionSummary")]
        public object ReactionSummary { get; set; }
    }

    public class Image
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("attribution")]
        public string Attribution { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("quality")]
        public int Quality { get; set; }


        [JsonIgnore()]
        public Uri Uri => new Uri(Url ?? string.Empty);
    }

    public class Provider
    {
        [JsonProperty("logoUrl")]
        public string LogoUrl { get; set; }

        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
