using System.Net;
using HtmlAgilityPack;

namespace EldenRingStratagem;

public sealed class WikiClient
{
    private readonly HtmlWeb web;
    private readonly string _baseUrl;
    
    public WikiClient()
    {
        _baseUrl = "https://eldenring.wiki.fextralife.com/";
        web = new HtmlWeb();
    }

    public List<string> GetStrategyFor(string bossName)
    {
        var adjustedBossName = ReplaceWhiteSpaceWithPlus(bossName);
        var urlWithBossName = $"{_baseUrl}/{adjustedBossName}";

        var html = GetPageHtml(urlWithBossName);
        var decodedHtml = WebUtility.HtmlDecode(html);

        var doc = new HtmlDocument();
        doc.OptionFixNestedTags = false;
        doc.LoadHtml(decodedHtml);

        if (doc == null)
            throw new Exception("Not found"); //todo improve message

        // var header = doc.DocumentNode
        //     .Descendants("h4")
        //     .FirstOrDefault(x => String.Equals(x.InnerText.Trim(), $"{bossName} Fight Strategy", StringComparison.CurrentCultureIgnoreCase));
        // if (header == null)
        //     throw new Exception("Another bit not found"); //todo improve message

        var div = doc.DocumentNode.Descendants("div")
            .Where(d => !d.Descendants("div").Any(d2 => d2.Descendants("h4").Any(h => h.GetAttributeValue("class", "") == "special" && h.InnerText.Trim() == $"{bossName} Fight Strategy")))
            .FirstOrDefault(d => d.Descendants("h4").Any(h => h.GetAttributeValue("class", "") == "special" && h.InnerText.Trim() == $"{bossName} Fight Strategy"));



        if (div == null)
            throw new Exception("Another bit not found"); //todo improve message

        var list = div.Descendants("ul").FirstOrDefault();

        if (list == null)
            throw new Exception("yet another bit not found"); //todo improve message

        return list.Descendants("li").Select(x => x.InnerText.Trim()).ToList();
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
}