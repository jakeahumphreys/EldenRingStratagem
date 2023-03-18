using EldenRingStratagem;

var wikiClient = new WikiClient();
var exit = false;

while (!exit)
{
    Console.WriteLine("Enter the name of the boss as it's displayed in-game");
    var bossName = Console.ReadLine();

    if (bossName != null)
    {
        if (bossName == "exit")
            exit = true;
        else
        {
            var items = wikiClient.GetStrategyFor(bossName);
            if(items.Count == 0)
                Console.WriteLine($"Looks like the a stratagem can't be found for {bossName}, did you enter their name correctly?");
    
            Console.WriteLine($"Best Tips for {bossName}:");
            Console.WriteLine(string.Join("\n", items));
        }
    }
}




