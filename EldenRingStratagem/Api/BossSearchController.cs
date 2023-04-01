using EldenRingStratagem.Api.Types.Search;
using Microsoft.AspNetCore.Mvc;

namespace EldenRingStratagem.Api;

[ApiController]
[Route("api/bosses")]
public class BossSearchController
{
    private readonly IBossSearchService _bossSearchService;

    public BossSearchController(IBossSearchService bossSearchService)
    {
        _bossSearchService = bossSearchService;
    }
    
    [Route("search")]
    public SearchResponse Search([FromBody] SearchRequest request)
    {
        return _bossSearchService.BestTips(request);
    }
}