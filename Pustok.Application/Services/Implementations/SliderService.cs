using AutoMapper;
using Pustok.Application.Dtos.SliderDtos;
using Pustok.Application.Services.Abstractions;
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

    public Task<int> CreateAsync(SliderCreateDto dto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<SliderGetDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<SliderGetDto> GetAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Paginate<SliderGetDto>> GetPaginateAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(SliderUpdateDto dto)
    {
        throw new NotImplementedException();
    }
}
