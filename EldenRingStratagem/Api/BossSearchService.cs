using EldenRingStratagem.Api.Types.Search;
using EldenRingStratagem.Client;

namespace EldenRingStratagem.Api;

public interface IBossSearchService
{
    public SearchResponse SearchWiki(SearchRequest request);
}
public class BossSearchSearchService : IBossSearchService
{
    private readonly WikiClient _wikiClient;

    public BossSearchSearchService()
    {
        _wikiClient = new WikiClient();
    }
    
    public SearchResponse SearchWiki(SearchRequest request)
    {
        return _wikiClient.Search(request.BossName);
    }
}