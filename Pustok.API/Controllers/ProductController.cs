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
    public async Task<IActionResult> GetProducts()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }
    //[HttpGet]
    //public IActionResult GetPaginatedProducts(int index,int size)
    //{
    //    var products = _productService.GetPaginateAsync(index, size);
    //    return Ok(products);
    //}
    [HttpPost]
    [Consumes("multipart/form-data")]
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
    [HttpPatch("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductUpdateDto dto)
    {
        await _productService.UpdateAsync(dto, id);
        return NoContent();
    }
}
