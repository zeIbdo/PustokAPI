using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pustok.Application.Dtos.ProductDtos;
using Pustok.Application.Services.Abstractions;
using System.Threading.Tasks;

namespace Pustok.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(string? search = null)
    {
        if(search is { })
            return Ok(await _productService.GetProductsByNameAsync(search));
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("paginated")]
    public async Task<IActionResult> GetPaginatedProducts(int index=0, int size=10)
    {
        var products = await _productService.GetPaginateAsync(index, size);
        return Ok(products);
    }

    [HttpPost]
    [Authorize(Roles ="Admin,Moderator")]
    [Consumes("multipart/form-data")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateProduct([FromForm] ProductCreateDto dto)
    {
        var prodId = await _productService.CreateAsync(dto);
        return Ok(prodId);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await _productService.DeleteAsync(id);
        return NoContent();
    }

    //[Authorize(Roles = "Admin,Moderator")]
    [HttpPatch("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductUpdateDto dto)
    {
        await _productService.UpdateAsync(dto, id);
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await _productService.GetAsync(id);
        return Ok(product);
    }

    [HttpGet("newest")]
    public async Task<IActionResult> GetNewest()
    {
        var products = await _productService.GetNewestProductsAsync();
        return Ok(products);
    }

    [HttpGet("discounted")]
    public async Task<IActionResult> GetDiscounted()
    {
        var products = await _productService.GetDiscountedProductsAsync();
        return Ok(products);
    }

    [HttpGet("most-sold")]
    public async Task<IActionResult> GetMostSold()
    {
        var products = await _productService.GetMostSoldProductsAsync();
        return Ok(products);
    }

    [HttpGet("{id}/detail")]
    public async Task<IActionResult> GetDetailedProduct(int id)
    {
        var detailDto = await _productService.GetDetailedProductAsync(id);
        return Ok(detailDto);
    }
}
