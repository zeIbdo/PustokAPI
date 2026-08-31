using Pustok.Application.Dtos.BasketItemDtos;

namespace Pustok.Application.Services.Abstractions;

public interface IBasketService
{
    Task AddToBasketAsync(int productId,int count=1);
    Task DecreaseToBasketAsync(int productId);
    Task RemoveFromBasketAsync(int productId);
    Task UpdateBasketAsync(int productId, int count);                                                         
    Task<BasketDto> GetBasketAsync();
}
