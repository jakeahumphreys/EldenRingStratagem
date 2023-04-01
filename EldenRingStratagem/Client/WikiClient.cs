using System.Net;
using EldenRingStratagem.Api.Types;
using EldenRingStratagem.Api.Types.Search;
using EldenRingStratagem.Client.Types;
using HtmlAgilityPack;

namespace EldenRingStratagem.Client;

public sealed class WikiClient
{
    private readonly HtmlWeb _web;
    private readonly string _baseUrl;
    private List<CachedStratagem> _cachedStratagems;

    private List<CachedStratagem> CachedStratagems { get; set; }

    public WikiClient()
    {
        _baseUrl = "https://eldenring.wiki.fextralife.com/";
        _web = new HtmlWeb();
        CachedStratagems = new List<CachedStratagem>();
    }
    
    public SearchResponse Search(string bossName)
    {
        var cachedStratagemForBoss = CachedStratagems.SingleOrDefault(x =>
            x.BossName == bossName && IsNewerThanTenMinutes(x.DateTime));

        if (cachedStratagemForBoss != null)
        {
            return new SearchResponse
            {
                BestTips = cachedStratagemForBoss.BestTips,
                Source = "Fextralife (Cached)"
            };
        }
        
        var adjustedBossName = ReplaceWhiteSpaceWithPlus(bossName);
        var urlWithBossName = $"{_baseUrl}/{adjustedBossName}";

        var html = GetPageHtml(urlWithBossName);

        if (string.IsNullOrWhiteSpace(html))
            return new SearchResponse().WithError<SearchResponse>(new Error
            {
                Message = "Unable to find the requested page on Fextralife"
            });

        var decodedHtml = WebUtility.HtmlDecode(html);

        var doc = new HtmlDocument();
        doc.OptionFixNestedTags = false;
        doc.LoadHtml(decodedHtml);

        var div = doc.DocumentNode.Descendants("div")
            .Where(d => !d.Descendants("div").Any(d2 => d2.Descendants("h4").Any(h => h.GetAttributeValue("class", "") == "special" && h.InnerText.Trim() == $"{bossName} Fight Strategy")))
            .FirstOrDefault(d => d.Descendants("h4").Any(h => h.GetAttributeValue("class", "") == "special" && h.InnerText.Trim() == $"{bossName} Fight Strategy"));

        if (div == null)
            return new SearchResponse().WithError<SearchResponse>(new Error
            {
                Message = $"The fextralife wiki page doesn't appear to be configured for {bossName}"
            });

        var list = div.Descendants("ul").FirstOrDefault();

        if (list == null)
            return new SearchResponse().WithError<SearchResponse>(new Error
            {
                Message = $"The fextralife wiki page doesn't appear to have any tips for {bossName}"
            });
        
        var tipsList = list.Descendants("li").Select(x => x.InnerText.Trim()).ToList();
        
        CachedStratagems.Add(new CachedStratagem
        {
            BossName = bossName,
            BestTips = tipsList,
            DateTime = DateTime.Now
        });

        return new SearchResponse
        {
            BestTips = tipsList,
            Source = "Fextralife"
        };
    }

    private string ReplaceWhiteSpaceWithPlus(string value)
    {
        return value.Replace(" ", "+");
    }

    private string GetPageHtml(string url)
    {
        string result;

        using (HttpClient client = new HttpClient())
        {
            using (HttpResponseMessage response = client.GetAsync(url).Result)
            {
                using (HttpContent content = response.Content)
                {
                    result = content.ReadAsStringAsync().Result;
                }
            }
        }

        return result;
    }

    private bool IsNewerThanTenMinutes(DateTime dateTimeToCheck)
    {
        var difference = DateTime.Now - dateTimeToCheck;

        return difference.TotalMinutes <= 10;
    }
}