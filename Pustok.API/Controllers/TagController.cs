using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pustok.Application.Dtos.TagDtos;
using Pustok.Application.Services.Abstractions;
using System.Threading.Tasks;

namespace Pustok.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTags()
    {
        var tags = await _tagService.GetAllAsync();
        return Ok(tags);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTag(int id)
    {
        var tag = await _tagService.GetAsync(id);
        return Ok(tag);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> CreateTag([FromBody] TagCreateDto dto)
    {
        var tagId = await _tagService.CreateAsync(dto);
        return Ok(tagId);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteTag(int id)
    {
        await _tagService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> UpdateTag(int id, [FromBody] TagUpdateDto dto)
    {
        await _tagService.UpdateAsync(dto, id);
        return NoContent();
    }
}
