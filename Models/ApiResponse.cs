namespace BlackJackApi.Models;

public class DeckResponse
{
    public string? Deck_id { get; set; }
    public int Remaining { get; set; }
}

public class CardResponse
{
    public List<Card>? Cards { get; set; }
    public int Remaining { get; set; }
}

public class Card
{
    public string? Code { get; set; }
}