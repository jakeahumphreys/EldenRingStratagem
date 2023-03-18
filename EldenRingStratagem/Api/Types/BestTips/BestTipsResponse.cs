namespace EldenRingStratagem.Api.Types.BestTips;

public sealed class BestTipsResponse : ResponseBase
{
    public List<string> BestTips { get; set; }
    public string Source { get; set; }
}