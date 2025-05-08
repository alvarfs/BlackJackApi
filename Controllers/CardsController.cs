using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

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

            if (!response.IsSuccessStatusCode) throw new Exception();

            var deckData = await response.Content.ReadAsStringAsync();
            return Content(deckData, "application/json");
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, $"Error al intentar mezclar la baraja: {ex.Message}");
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

            if (!response.IsSuccessStatusCode) throw new Exception();

            var cardsData = await response.Content.ReadAsStringAsync();
            return Content(cardsData, "application/json");
        }
        catch (HttpRequestException ex)
        {
            if (count > 1)
                return StatusCode(500, $"Error al intentar recoger {count} cartas: {ex.Message}");
            return StatusCode(500, $"Error al intentar recoger una carta: {ex.Message}");
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

            if (!response.IsSuccessStatusCode) throw new Exception();

            var deckData = await response.Content.ReadAsStringAsync();
            return Content(deckData, "application/json");
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, $"Error al intentar mezclar la baraja: {ex.Message}");
        }
    }
}
