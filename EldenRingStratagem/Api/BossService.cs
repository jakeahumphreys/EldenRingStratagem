using EldenRingStratagem.Api.Types;
using EldenRingStratagem.Client;

namespace EldenRingStratagem.Api;

public interface IBossService
{
    public BestTipsResponse BestTips(BestTipsRequest request);
}
public class BossService : IBossService
{
    private readonly WikiClient _wikiClient;

    public BossService()
    {
        _wikiClient = new WikiClient();
    }
    
    public BestTipsResponse BestTips(BestTipsRequest request)
    {
        return _wikiClient.GetStrategyFor(request.BossName);
    }
}