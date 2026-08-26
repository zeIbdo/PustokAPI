using Microsoft.AspNetCore.Mvc;
using Pustok.Application.Dtos.CategoryDtos;
using Pustok.Application.Services.Abstractions;
using Pustok.Application.Services.Implementations;
using System.Threading.Tasks;

namespace Pustok.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly IProductService _productService;

    public CategoryController(ICategoryService categoryService, IProductService productService)
    {
        _categoryService = categoryService;
        _productService = productService;
    }
    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _categoryService.GetAllAsync();
        return Ok(categories);
    }
    [HttpGet("{id}/products")]
    public async Task<IActionResult> GetProductsByCategory(int id)
    {
        var products = await _productService.GetProductsByCategory(id);
        return Ok(products);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(int id)
    {
        var category = await _categoryService.GetAsync(id);
        return Ok(category);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        await _categoryService.DeleteAsync(id);
        return NoContent();
    }
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody]CategoryCreateDto dto)
    {
        var catId = await _categoryService.CreateAsync(dto);
        return Ok(catId);
    }
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDto dto)
    {
        await _categoryService.UpdateAsync(dto, id);
        return NoContent();
    }
}
