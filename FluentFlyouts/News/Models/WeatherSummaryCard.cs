using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FluentFlyouts.News.Models
{
    public class WeatherSummaryCard : Card
    {
        [JsonProperty("data")]
        public string Data
        {
            set
            {
                WeatherSummary = ProcessData(value);
            }
        }

        [JsonIgnore]
        public WeatherSummaryData WeatherSummary { get; set; }

        public WeatherSummaryData ProcessData(string data)
        {
            return JsonConvert.DeserializeObject<WeatherSummaryData>(data);
        }

        public WeatherReportCurrent GetFirstCurrentReport()
        {
            var current = WeatherSummary.Responses[0].Weather[0].Current;
            current.TemperatureUnit = WeatherSummary.Units["temperature"];
            return current;
        }

        public List<WeatherReportForecast> GetFirstForecastReports()
        {
            return WeatherSummary.Responses[0].Weather[0].Forecasts;
        }
    }

    public class WeatherSummaryData
    {
        [JsonProperty("userProfile")]
        public WeatherUserProfile UserProfile { get; set; }

        [JsonProperty("responses")]
        public List<WeatherResponse> Responses { get; set; }

        [JsonProperty("units")]
        public Dictionary<string, string> Units { get; set; }
    }

    public class WeatherUserProfile
    {
        [JsonProperty("location")]
        public WeatherLocation Location { get; set; }
    }

    public class WeatherLocation
    {
        public string Country { get; set; }

        public string CountryCode { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string UtcOffset { get; set; }

        public int DmaCode { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }

    public class WeatherResponse
    {
        [JsonProperty("weather")]
        public List<WeatherResult> Weather { get; set; }

        [JsonProperty("source")]
        public WeatherLocationSource Source { get; set; }

        public static readonly Dictionary<int, string> WeatherIcons = new Dictionary<int, string>()
        {
            { 1, "SunnyDayV2" },
            { 3, "PartlyCloudyDayV2" },
            { 5, "CloudyV2" },
            { 6, "BlowingHailV2" },
            { 7, "BlowingSnowV2" },
            { 8, "LightRainV2" },
            { 9, "FogV2" },
            { 10, "FreezingRainV2" },
            { 12, "HazySmokeV2" },
            { 14, "ModerateRainV2" },
            { 15, "HeavySnowV2" },
            { 16, "HailDayV2" },
            { 20, "LightSnowV2" },
            { 23, "RainShowersDayV2" },
            { 24, "RainSnowV2" },
            { 26, "SnowShowersDayV2" },
            { 27, "ThunderstormsV2" },
            { 28, "ClearNightV2" },
            { 30, "PartlyCloudyNightV2" },
            { 39, "HazeSmokeNightV2_106" },
            { 43, "HailNightV2" },
            { 50, "RainShowersNightV2" },
            { 77, "RainSnowV2" },
            { 78, "RainSnowShowersNightV2" },
            { 82, "SnowShowersNightV2" },
            { 91, "WindyV2" },
        };
        public static readonly Dictionary<int, int> WeatherIconMapping = new Dictionary<int, int>()
        {
            { 17, 8 }, { 31, 30 }, { 49, 14 }, { 41, 14 }, { 35, 8 }, { 4, 3 }, { 19, 8 }, { 22, 14 },
            { 23, 23 }, { 40, 8 }, { 25, 15 }, { 34, 7 }, { 47, 20 }, { 52, 15 }, { 53, 26 }, { 57, 7 },
            { 58, 7 }, { 59, 7 }, { 60, 7 }, { 81, 26 }, { 11, 10 }, { 37, 10 }, { 38, 10 }, { 51, 24 },
            { 65, 16 }, { 66, 43 }, { 69, 10 }, { 70, 10 }, { 71, 10 }, { 72, 10 }, { 73, 16 }, { 74, 43 },
            { 89, 12 }, { 92, 91 }, { 32, 5 }, { 2, 1 }, { 101, 1 }, { 42, 15 }, { 33, 6 }, { 61, 6 },
            { 62, 6 }, { 87, 6 }, { 88, 6 }, { 93, 6 }, { 94, 6 }, { 95, 6 }, { 96, 6 }, { 54, 27 },
            { 67, 27 }, { 68, 27 }, { 90, 12 }, { 18, 9 }, { 21, 9 }, { 36, 9 }, { 45, 9 }, { 48, 9 },
            { 63, 9 }, { 64, 9 }, { 29, 28 }, { 102, 28 }, { 44, 8 }, { 46, 8 }, { 79, 23 }, { 80, 50 },
            { 76, 24 }, { 75, 24 }, { 83, 77 }, { 84, 78 }, { 85, 24 }, { 86, 24 }, { 39, 39 }, { 13, 8 },
        };
        public static Uri ResolveWeatherIconUrl(int iconId)
        {
            string svgFileName = "SunnyDayV2.png";
            int actualIcon = iconId;
            if (WeatherIconMapping.ContainsKey(actualIcon))
                actualIcon = WeatherIconMapping[actualIcon];

            if (WeatherIcons.ContainsKey(actualIcon))
            {
                svgFileName = WeatherIcons[actualIcon] + ".png";
            }
            //return new Uri(Api.MSN_ASSETS_WINSHELL_LATEST + "/" + svgFileName);
            return new Uri("ms-appx:///Assets/WeatherIcons/" + svgFileName);
        }
    }

    public class WeatherResult
    {
        [JsonProperty("alerts")]
        public List<WeatherAlert> Alerts { get; set; }

        [JsonProperty("current")]
        public WeatherReportCurrent Current { get; set; }

        [JsonProperty("forecast")]
        public JObject ForecastObject
        {
            set
            {
                Forecasts = new List<WeatherReportForecast>();
                foreach (var day in value["days"])
                {
                    Forecasts.Add(day["daily"].ToObject<WeatherReportForecast>());
                }
            }
        }

        [JsonIgnore]
        public List<WeatherReportForecast> Forecasts { get; internal set; }

        [JsonProperty("provider")]
        public WeatherProvider Provider { get; set; }
    }

    public class WeatherLocationSource
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("coordinates")]
        public Coordinates Coordinates { get; set; }
    }

    public class Coordinates
    {
        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lon")]
        public double Longitude { get; set; }
    }

    public class WeatherReportForecast
    {
        [JsonIgnore]
        public List<string> DaySummaries { get; internal set; }

        [JsonIgnore]
        public List<string> NightSummaries { get; internal set; }

        [JsonProperty("day")]
        public JObject Day
        {
            set
            {
                DaySummaries = value["summaries"].ToObject<List<string>>();
            }
        }

        [JsonProperty("night")]
        public JObject Night
        {
            set
            {
                NightSummaries = value["summaries"].ToObject<List<string>>();
            }
        }

        [JsonProperty("pvdrCap")]
        public string ProviderCaption { get; set; }

        [JsonProperty("valid")]
        public DateTimeOffset ValidUntil { get; set; }

        [JsonProperty("icon")]
        public int Icon { get; set; }

        [JsonIgnore]
        public Uri IconSource => WeatherResponse.ResolveWeatherIconUrl(Icon);

        [JsonProperty("pvdrIcon")]
        public string ProviderIcon { get; set; }

        [JsonProperty("precip")]
        public double Precipitation { get; set; }

        [JsonProperty("rhHi")]
        public double RelativeHumidityHigh { get; set; }

        [JsonProperty("rhLo")]
        public double RelativeHumidityLow { get; set; }

        [JsonProperty("tempHi")]
        public double TemperatureHigh { get; set; }

        [JsonProperty("tempLo")]
        public double TemperatureLow { get; set; }

        [JsonProperty("uv")]
        public double UV { get; set; }

        [JsonProperty("uvDesc")]
        public string UVDescription { get; set; }

        [JsonProperty("created")]
        public DateTimeOffset CreatedAt { get; set; }
    }

    public class WeatherReportCurrent
    {
        [JsonProperty("baro")]
        public double Barometer { get; set; }

        [JsonProperty("cap")]
        public string Caption { get; set; }

        [JsonProperty("daytime")]
        public string Daytime { get; set; }

        [JsonProperty("dewPt")]
        public double DewPoint { get; set; }

        [JsonProperty("feels")]
        public double FeelsLike { get; set; }

        [JsonProperty("rh")]
        public double RelativeHumidity { get; set; }

        [JsonProperty("icon")]
        public int Icon { get; set; }

        [JsonIgnore]
        public Uri IconSource => WeatherResponse.ResolveWeatherIconUrl(Icon);

        [JsonProperty("pvdrIcon")]
        public string ProviderIcon { get; set; }

        [JsonProperty("wx")]
        public string Wx { get; set; }

        [JsonProperty("sky")]
        public string Sky { get; set; }

        [JsonProperty("temp")]
        public double Temperature { get; set; }

        [JsonIgnore]
        public string TemperatureUnit { get; set; }

        [JsonProperty("uv")]
        public double UV { get; set; }

        [JsonProperty("uvDesc")]
        public string UVDescription { get; set; }

        [JsonProperty("vis")]
        public double Visibility { get; set; }

        [JsonProperty("waterTemp")]
        public double WaterTemperature { get; set; }

        [JsonProperty("windDir")]
        public double WindDirection { get; set; }

        [JsonProperty("windSpd")]
        public double WindSpeed { get; set; }

        [JsonProperty("windGust")]
        public double WindGust { get; set; }

        [JsonProperty("created")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("pvdrCap")]
        public string ProviderCaption { get; set; }
    }

    public class WeatherProvider
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class WeatherAlert
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("abbreviation")]
        public string[] Abbreviation { get; set; }

        [JsonProperty("severity")]
        public string Severity { get; set; }

        [JsonProperty("significance")]
        public string Significance { get; set; }

        [JsonProperty("credit")]
        public string Credit { get; set; }

        [JsonProperty("created")]
        public DateTimeOffset Created { get; set; }

        [JsonProperty("start")]
        public DateTimeOffset Start { get; set; }

        [JsonProperty("end")]
        public DateTimeOffset End { get; set; }

        [JsonProperty("class")]
        public string Class { get; set; }
    }
}
