using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pustok.Application.Dtos.BasketItemDtos;
using Pustok.Application.Services.Abstractions;
using System.Threading.Tasks;

namespace Pustok.API.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class BasketController : ControllerBase
{
    private readonly IBasketService _basketService;

    public BasketController(IBasketService basketService)
    {
        _basketService = basketService;
    }

    [HttpGet]
    public async Task<IActionResult> GetBasket()
    {
        var basket = await _basketService.GetBasketAsync();
        return Ok(basket);
    }

    [HttpPost("{productId}/{count}")]
    public async Task<IActionResult> AddtoBasket(int productId, int count = 1)
    {
        await _basketService.AddToBasketAsync(productId, count);
        return NoContent();
    }

    [HttpPatch("{productId}/decrease")]
    public async Task<IActionResult> DecreaseBasketItem(int productId)
    {
        await _basketService.DecreaseToBasketAsync(productId);
        return NoContent();
    }

    [HttpPatch("{productId}/{count}")]
    public async Task<IActionResult> UpdateBasket(int productId, int count)
    {
        await _basketService.UpdateBasketAsync(productId, count);
        return NoContent();
    }

    [HttpDelete("{productId}")]
    public async Task<IActionResult> RemoveFromBasket(int productId)
    {
        await _basketService.RemoveFromBasketAsync(productId);
        return NoContent();
    }


}
