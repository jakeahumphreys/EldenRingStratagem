using EldenRingStratagem.Api.Types.Search;
using EldenRingStratagem.Client;

namespace EldenRingStratagem.Api;

public interface IBossSearchService
{
    public SearchResponse BestTips(SearchRequest request);
}
public class BossSearchSearchService : IBossSearchService
{
    private readonly WikiClient _wikiClient;

    public BossSearchSearchService()
    {
        _wikiClient = new WikiClient();
    }
    
    public SearchResponse BestTips(SearchRequest request)
    {
        return _wikiClient.GetStrategyFor(request.BossName);
    }
}