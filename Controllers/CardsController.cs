using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BlackJackApi.Models;

namespace BlackJackApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CardsController : ControllerBase
{
    private readonly ILogger<CardsController> _logger;
    private readonly HttpClient _httpClient;

    public CardsController(ILogger<CardsController> logger)
    {
        _logger = logger;
        _httpClient = new HttpClient();
    }

    // Obtenemos un mazo nuevo de 6 barajas mezcladas
    [HttpGet("GetDeck")]
    public async Task<IActionResult> GetDeck()
    {
        var getDeck = "https://deckofcardsapi.com/api/deck/new/shuffle/?deck_count=6";

        try
        {
            var response = await _httpClient.GetAsync(getDeck);

            if (!response.IsSuccessStatusCode) throw new HttpRequestException();

            var deckDataJson = await response.Content.ReadAsStringAsync();
            var deckResponse = JsonConvert.DeserializeObject<DeckResponse>(deckDataJson);
            
            DeckReturn result = new DeckReturn
            {
                Deck = deckResponse.Deck_id,
                Remaining = deckResponse.Remaining
            };

            return Ok(result);
        }
        catch (HttpRequestException)
        {
            return StatusCode(500, $"Error al intentar mezclar la baraja");
        }
    }

    // Robamos una cantidad especifica de cartas de un mazo existente
    [HttpGet("GetCards")]
    public async Task<IActionResult> GetCards(string deck_id, int count)
    {
        var getCards = $"https://deckofcardsapi.com/api/deck/{deck_id}/draw/?count={count}";

        try
        {
            var response = await _httpClient.GetAsync(getCards);

            if (!response.IsSuccessStatusCode) throw new HttpRequestException();

            var cardsDataJson = await response.Content.ReadAsStringAsync();
            var cardResponse = JsonConvert.DeserializeObject<CardResponse>(cardsDataJson);
            
            List<string> cards = new List<string>();
            foreach (var card in cardResponse.Cards)
                cards.Add(card.Code);

            CardReturn result = new CardReturn
            {
                Cards = cards,
                Remaining = cardResponse.Remaining
            };

            return Ok(result);
        }
        catch (HttpRequestException)
        {
            if (count > 1)
                return StatusCode(500, $"Error al intentar recoger {count} cartas");
            return StatusCode(500, $"Error al intentar recoger una carta");
        }
    }

    // Mezclamos nuevamente todas las cartas del mazo (incluidos los descartes)
    [HttpGet("ShuffleDeck")]
    public async Task<IActionResult> ShuffleDeck(string deck_id)
    {
        var getShuffleDeck = $"https://deckofcardsapi.com/api/deck/{deck_id}/shuffle/";

        try
        {
            var response = await _httpClient.GetAsync(getShuffleDeck);

            if (!response.IsSuccessStatusCode) throw new HttpRequestException();

            var deckDataJson = await response.Content.ReadAsStringAsync();
            var deckResponse = JsonConvert.DeserializeObject<DeckResponse>(deckDataJson);
            
            DeckReturn result = new DeckReturn
            {
                Deck = deckResponse.Deck_id,
                Remaining = deckResponse.Remaining
            };

            return Ok(result);
        }
        catch (HttpRequestException)
        {
            return StatusCode(500, $"Error al intentar mezclar la baraja");
        }
    }
}
