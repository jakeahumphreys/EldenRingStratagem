namespace EldenRingStratagem.Client.Types;

public class CachedStratagem
{
    public string BossName { get; set; }
    public DateTime DateTime { get; set; }
    public List<string> BestTips { get; set; }

    public CachedStratagem()
    {
        BestTips = new List<string>();
    }
}