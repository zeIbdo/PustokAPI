using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pustok.Application.Dtos.SettingDtos;
using Pustok.Application.Exceptions;
using Pustok.Application.Services.Abstractions;
using Pustok.Domain.Entities;
using Pustok.Infrastructure.Paging;
using Pustok.Infrastructure.Repositories.Abstractions;
using Pustok.Infrastructure.Repositories.Implementations;

namespace Pustok.Application.Services.Implementations;

public class SettingService : ISettingService
{
    private readonly ISettingRepository _settingRepository;
    private readonly IMapper _mapper;

    public SettingService(ISettingRepository settingRepository, IMapper mapper)
    {
        _settingRepository = settingRepository;
        _mapper = mapper;
    }

    public async Task<int> CreateAsync(SettingCreateDto dto)
    {
        var setting = _mapper.Map<Setting>(dto);
        var createdSetting = await _settingRepository.CreateAsync(setting);
        await _settingRepository.SaveChangesAsync();
        return createdSetting.Id;
    }

    public async Task DeleteAsync(int id)
    {
        var setting = await _settingRepository.GetAsync(id);
        if (setting == null)
            throw new NotFoundException("Setting not found");
        _settingRepository.Delete(setting);
        await _settingRepository.SaveChangesAsync();
    }

    public async Task<List<SettingGetDto>> GetAllAsync()
    {
        var settings = _settingRepository.GetAll();
        return _mapper.Map<List<SettingGetDto>>(await settings.ToListAsync());
    }

    public async Task<SettingGetDto> GetAsync(int id)
    {
        var setting = await _settingRepository.GetAsync(id);
        if (setting == null)
            throw new NotFoundException("Setting not found");
        return _mapper.Map<SettingGetDto>(setting);
    }

    public async Task<Paginate<SettingGetDto>> GetPaginateAsync(int index = 0, int size = 10)
    {
        var paginatedSettings = await _settingRepository.GetPaginateAsync(index: index, size: size);
        return _mapper.Map<Paginate<SettingGetDto>>(paginatedSettings);

    }

    public async Task UpdateAsync(SettingUpdateDto dto, int id)
    {
        var setting = await _settingRepository.GetAsync(id);
        if (setting == null)
            throw new NotFoundException("Setting not found");
        setting  = _mapper.Map(dto,setting);
        _settingRepository.Update(setting);
        await _settingRepository.SaveChangesAsync();
    }
}
