using System.Net;
using EldenRingStratagem.Api.Types;
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

    public BestTipsResponse GetStrategyFor(string bossName)
    {
        var cachedStratagemForBoss = CachedStratagems.SingleOrDefault(x =>
            x.BossName == bossName && IsNewerThanTenMinutes(x.DateTime));

        if (cachedStratagemForBoss != null)
        {
            return new BestTipsResponse
            {
                BestTips = cachedStratagemForBoss.BestTips,
                Source = "Fextralife (Cached)"
            };
        }
        
        var adjustedBossName = ReplaceWhiteSpaceWithPlus(bossName);
        var urlWithBossName = $"{_baseUrl}/{adjustedBossName}";

        var html = GetPageHtml(urlWithBossName);
        var decodedHtml = WebUtility.HtmlDecode(html);

        var doc = new HtmlDocument();
        doc.OptionFixNestedTags = false;
        doc.LoadHtml(decodedHtml);

        if (doc == null)
            throw new Exception("Not found"); //todo improve message

        var div = doc.DocumentNode.Descendants("div")
            .Where(d => !d.Descendants("div").Any(d2 => d2.Descendants("h4").Any(h => h.GetAttributeValue("class", "") == "special" && h.InnerText.Trim() == $"{bossName} Fight Strategy")))
            .FirstOrDefault(d => d.Descendants("h4").Any(h => h.GetAttributeValue("class", "") == "special" && h.InnerText.Trim() == $"{bossName} Fight Strategy"));
        
        if (div == null)
            throw new Exception("Another bit not found"); //todo improve message

        var list = div.Descendants("ul").FirstOrDefault();

        if (list == null)
            throw new Exception("yet another bit not found"); //todo improve message
        
        var tipsList = list.Descendants("li").Select(x => x.InnerText.Trim()).ToList();
        
        CachedStratagems.Add(new CachedStratagem
        {
            BossName = bossName,
            BestTips = tipsList,
            DateTime = DateTime.Now
        });

        return new BestTipsResponse
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