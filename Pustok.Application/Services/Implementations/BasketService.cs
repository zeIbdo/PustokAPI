using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Pustok.Application.Dtos.BasketItemDtos;
using Pustok.Application.Exceptions;
using Pustok.Application.Services.Abstractions;
using Pustok.Domain.Entities;
using Pustok.Infrastructure.Repositories.Abstractions;
using System.Security.Claims;

namespace Pustok.Application.Services.Implementations;

public class BasketService : IBasketService
{
    private readonly IBasketItemRepository _basketItemRepository;
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public BasketService(IBasketItemRepository basketItemRepository, IMapper mapper, IProductRepository productRepository, IHttpContextAccessor httpContextAccessor)
    {
        _basketItemRepository = basketItemRepository;
        _mapper = mapper;
        _productRepository = productRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task AddToBasketAsync(int productId, int count = 1)
    {
        var product = await _getProductAsync(productId);
        if (product.Stock == 0) throw new NotFoundException("Product stock is 0");

        if (count < 1) count = 1;


        string userId = _getUserId();
        var existingBasketItem = await _basketItemRepository.GetAsync(expression: x => x.ProductId == productId && x.AppUserId == userId);
        if (existingBasketItem != null)
        {
            if (existingBasketItem.Count + count > product.Stock)
            {
                existingBasketItem.Count += product.Stock;
                product.Stock = 0;
            }
            else
            {
                existingBasketItem.Count += count;
                product.Stock -= count;

            }
            _basketItemRepository.Update(existingBasketItem);
        }
        else
        {
            if (count > product.Stock)
            {
                count = product.Stock;
                product.Stock = 0;
            }
            else
                product.Stock -= count; 


            await _basketItemRepository.CreateAsync(new BasketItem { AppUserId = userId, ProductId = productId, Count = count });
        }
        _productRepository.Update(product);
        await _basketItemRepository.SaveChangesAsync();
    }
    public async Task UpdateBasketAsync(int productId, int count)
    {
        var product = await _getProductAsync(productId);
        var userId = _getUserId();
        var basketItem = await _getBasketItemAsync(productId, userId);
        if (count < 1) count = 1;
        if (count > product.Stock+basketItem.Count) count = product.Stock+basketItem.Count;
        product.Stock += basketItem.Count;
        basketItem.Count = count;
        product.Stock -= count;
        _basketItemRepository.Update(basketItem);
        _productRepository.Update(product);
        await _basketItemRepository.SaveChangesAsync();
    }

    public async Task DecreaseToBasketAsync(int productId)
    {
        var product = await _getProductAsync(productId);
        var userId = _getUserId();
        var basketItem = await _getBasketItemAsync(productId, userId);
        if (basketItem.Count == 1)
            return;
        product.Stock++;
        basketItem.Count--;
        _basketItemRepository.Update(basketItem);
        _productRepository.Update(product);
        await _basketItemRepository.SaveChangesAsync();
    }

    public async Task<BasketDto> GetBasketAsync()
    {
        var userId = _getUserId();
        var basketItems = await _basketItemRepository.GetAll(predicate: x => x.AppUserId == userId, include: x => x.Include(y => y.Product)).ToListAsync();
        var dtos = _mapper.Map<List<BasketItemGetDto>>(basketItems);
        var basket = new BasketDto { Items = dtos };
        basket.Count = basketItems.Sum(x => x.Count);
        basket.Total = dtos.Sum(x => x.Count * x.Product.Price);
        basket.DiscountedTotal = dtos.Sum(x => x.Count * (x.Product.Price - (x.Product.Price * x.Product.Discount / 100)));
        return basket;
    }

    public async Task RemoveFromBasketAsync(int productId)
    {
        var product = await _getProductAsync(productId);
        var userId = _getUserId();
        var basketItem = await _getBasketItemAsync(productId, userId);
        product.Stock += basketItem.Count;
        _basketItemRepository.Delete(basketItem);
        _productRepository.Update(product);
        await _basketItemRepository.SaveChangesAsync();
    }

    private string _getUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (userId == null)
            throw new UnauthorizedException("Login to add basket with db");
        return userId;
    }
    private async Task<BasketItem> _getBasketItemAsync(int productId, string userId)
    {
        var basketItem = await _basketItemRepository.GetAsync(expression: x => x.ProductId == productId && x.AppUserId == userId);
        if (basketItem == null) throw new NotFoundException("Basket item not found");
        return basketItem;
    }

    private async Task<Product> _getProductAsync(int productId)
    {
        var product = await _productRepository.GetAsync(productId);
        if (product == null) throw new NotFoundException("Product not found");
        return product;
    }


}
