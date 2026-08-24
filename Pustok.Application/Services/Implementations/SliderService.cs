using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pustok.Application.Dtos.SliderDtos;
using Pustok.Application.Exceptions;
using Pustok.Application.Services.Abstractions;
using Pustok.Domain.Entities;
using Pustok.Infrastructure.Paging;
using Pustok.Infrastructure.Repositories.Abstractions;

namespace Pustok.Application.Services.Implementations;

public class SliderService : ISliderService
{
    private readonly ISliderRepository _sliderRepository;
    private readonly IMapper _mapper;

    public SliderService(ISliderRepository sliderRepository, IMapper mapper)
    {
        _sliderRepository = sliderRepository;
        _mapper = mapper;
    }

    public async Task<int> CreateAsync(SliderCreateDto dto)
    {
        var slider = _mapper.Map<Slider>(dto);
        var createdSlider = await _sliderRepository.CreateAsync(slider);
        await _sliderRepository.SaveChangesAsync();
        return createdSlider.Id;
    }

    public async Task DeleteAsync(int id)
    {
        var slider = await _sliderRepository.GetAsync(id);
        if (slider == null)
            throw new NotFoundException("Slider Not Found");
         _sliderRepository.Delete(slider);
        await _sliderRepository.SaveChangesAsync();
    }

    public async Task<List<SliderGetDto>> GetAllAsync()
    {
        var sliders = _sliderRepository.GetAll();
        return _mapper.Map<List<SliderGetDto>>(await sliders.ToListAsync());
    }

    public async Task<SliderGetDto> GetAsync(int id)
    {
        var slider = await _sliderRepository.GetAsync(id);
        if (slider == null)
            throw new NotFoundException("Slider Not Found");
        return _mapper.Map<SliderGetDto>(slider);   
    }

    public async Task<Paginate<SliderGetDto>> GetPaginateAsync(int index=0,int size=10)
    {
        var paginatedSliders = await _sliderRepository.GetPaginateAsync(index:index, size:size);
        return _mapper.Map<Paginate<SliderGetDto>>(paginatedSliders);
    }

    public async Task UpdateAsync(SliderUpdateDto dto, int id)
    {
        var slider =await _sliderRepository.GetAsync(id);
        if (slider == null)
            throw new NotFoundException("Slider not found");
        slider = _mapper.Map(dto, slider);
        _sliderRepository.Update(slider);
        await _sliderRepository.SaveChangesAsync();
    }
}
