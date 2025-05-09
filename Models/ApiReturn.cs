namespace BlackJackApi.Models;

public class DeckReturn
{
    public string? Deck { get; set; }
    public int Remaining { get; set; }
}

public class CardReturn
{
    public List<string>? Cards { get; set; }
    public int Remaining { get; set; }
}
