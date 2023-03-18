using EldenRingStratagem.Api.Types;
using Microsoft.AspNetCore.Mvc;

namespace EldenRingStratagem.Api;

[ApiController]
[Route("api/bosses")]
public class BossController
{
    private readonly IBossService _bossService;

    public BossController(IBossService bossService)
    {
        _bossService = bossService;
    }
    
    [Route("best-tips")]
    public BestTipsResponse BestTips([FromBody] BestTipsRequest request)
    {
        return _bossService.BestTips(request);
    }
}