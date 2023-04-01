namespace EldenRingStratagem.Api.Types.Search;

public sealed class SearchResponse : ResponseBase
{
    public List<string> BestTips { get; set; }
    public string Source { get; set; }
}