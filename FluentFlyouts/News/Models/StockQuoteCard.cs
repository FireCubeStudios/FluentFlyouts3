using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFlyouts.News.Models
{
    public class StockQuoteCard : Card
    {
        [JsonProperty("data")]
        public string Data
        {
            set
            {
                StockQuote = ProcessData(value);
            }
        }

        [JsonIgnore]
        public StockQuoteData StockQuote { get; set; }

        public StockQuoteData ProcessData(string data)
        {
            return JsonConvert.DeserializeObject<StockQuoteData>(data);
        }
    }

    public class StockQuoteData
    {
        [JsonProperty("userSignedIn")]
        public bool IsUserSignedIn { get; set; }

        [JsonProperty("quoteItems")]
        public List<QuoteItem> QuoteItems { get; set; }

        [JsonProperty("dataFrom")]
        public string DataFrom { get; set; }

    }

    public class QuoteItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("priceNumber")]
        public double PriceNumber { get; set; }

        [JsonProperty("changePcntNumber")]
        public double ChangePercentNumber { get; set; }

        [JsonProperty("changeValueNumber")]
        public double ChangeValueNumber { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("securityType")]
        public string SecurityType { get; set; }

        [JsonProperty("timeLastUpdated")]
        public DateTimeOffset TimeLastUpdated { get; set; }

        [JsonProperty("exchangeName")]
        public string ExchangeName { get; set; }

        [JsonProperty("exchangeId")]
        public string ExchangeId { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("changeValue")]
        public string ChangeValue { get; set; }

        [JsonProperty("changePcnt")]
        public string ChangePercent { get; set; }

        [JsonProperty("quoteHref")]
        public string QuoteHref { get; set; }

        [JsonProperty("gain")]
        public bool Gain { get; set; }

        [JsonProperty("unchanged")]
        public bool Unchanged { get; set; }

        [JsonProperty("formatedUpdateTime")]
        public string FormattedUpdateTime { get; set; }
    }
}
