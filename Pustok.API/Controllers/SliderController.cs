using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pustok.Application.Dtos.SliderDtos;
using Pustok.Application.Services.Abstractions;
using System.Threading.Tasks;

namespace Pustok.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SliderController : ControllerBase
    {
        private readonly ISliderService _sliderService;

        public SliderController(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSliders()
        {
            var sliders = await _sliderService.GetAllAsync();
            return Ok(sliders);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> CreateSlider([FromForm]SliderCreateDto dto)
        {
            var sliderId = await _sliderService.CreateAsync(dto);
            return Ok(sliderId);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSlider(int id)
        {
            await _sliderService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin,Moderator")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateSlider(int id, [FromForm] SliderUpdateDto dto)
        {
            await _sliderService.UpdateAsync(dto,id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSlider(int id)
        {
            var slider = await _sliderService.GetAsync(id);
            return Ok(slider);
        }
    }
}
