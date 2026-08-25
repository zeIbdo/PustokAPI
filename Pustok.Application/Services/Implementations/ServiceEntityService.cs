using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pustok.Application.Dtos.ServiceDtos;
using Pustok.Application.Exceptions;
using Pustok.Application.Services.Abstractions;
using Pustok.Domain.Entities;
using Pustok.Infrastructure.Paging;
using Pustok.Infrastructure.Repositories.Abstractions;

namespace Pustok.Application.Services.Implementations;

public class ServiceEntityService : IServiceEntityService
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;

    public ServiceEntityService(IServiceRepository serviceRepository, IMapper mapper, IFileService fileService)
    {
        _serviceRepository = serviceRepository;
        _mapper = mapper;
        _fileService = fileService;
    }

    public async Task<int> CreateAsync(ServiceCreateDto dto)
    {
        var service = _mapper.Map<Service>(dto);
        service.IconUrl = await _fileService.CreateFileAsync(dto.Image);
        var createdService = await _serviceRepository.CreateAsync(service);
        await _serviceRepository.SaveChangesAsync();
        return createdService.Id;
    }

    public async Task DeleteAsync(int id)
    {
        var service = await _serviceRepository.GetAsync(id);
        if (service == null)
            throw new NotFoundException("Service Not Found");
        var imageDeletion = await _fileService.RemoveFileAsync(service.IconUrl!);
        if (!imageDeletion)
            throw new ImageDeletionException();
        _serviceRepository.Delete(service);
        await _serviceRepository.SaveChangesAsync();
    }

    public async Task<List<ServiceGetDto>> GetAllAsync()
    {
        var services = _serviceRepository.GetAll();
        return _mapper.Map<List<ServiceGetDto>>(await services.ToListAsync());
    }

    public async Task<ServiceGetDto> GetAsync(int id)
    {
        var service = await _serviceRepository.GetAsync(id);
        if (service == null)
            throw new NotFoundException("Service Not Found");
        return _mapper.Map<ServiceGetDto>(service);
    }

    public async Task<Paginate<ServiceGetDto>> GetPaginateAsync(int index = 0, int size = 10)
    {
        var paginatedServices = await _serviceRepository.GetPaginateAsync(index: index, size: size);
        return _mapper.Map<Paginate<ServiceGetDto>>(paginatedServices);
    }

    public async Task UpdateAsync(ServiceUpdateDto dto, int id)
    {
        var service = await _serviceRepository.GetAsync(id);
        if (service == null)
            throw new NotFoundException("Service not found");
        service = _mapper.Map(dto, service);
        if (dto.Image != null)
        {
            var imageDeletion = await _fileService.RemoveFileAsync(service.IconUrl!);
            if (!imageDeletion)
                throw new ImageDeletionException();
            service.IconUrl = await _fileService.CreateFileAsync(dto.Image);
        }
        _serviceRepository.Update(service);
        await _serviceRepository.SaveChangesAsync();
    }
}
