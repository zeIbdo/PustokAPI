using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pustok.Application.Dtos.SettingDtos;
using Pustok.Application.Dtos.SubscriptionDtos;
using Pustok.Application.Exceptions;
using Pustok.Application.Services.Abstractions;
using Pustok.Domain.Entities;
using Pustok.Infrastructure.Paging;
using Pustok.Infrastructure.Repositories.Abstractions;
using Pustok.Infrastructure.Repositories.Implementations;

namespace Pustok.Application.Services.Implementations;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IMapper _mapper;

    public SubscriptionService(ISubscriptionRepository subscriptionRepository, IMapper mapper)
    {
        _subscriptionRepository = subscriptionRepository;
        _mapper = mapper;
    }

    public async Task<int> CreateAsync(SubscriptionCreateDto dto)
    {
        var subscription = _mapper.Map<Subscription>(dto);
        var createdSubscription = await _subscriptionRepository.CreateAsync(subscription);
        await _subscriptionRepository.SaveChangesAsync();
        return createdSubscription.Id;
    }

    public async Task DeleteAsync(int id)
    {
        var subscription = await _subscriptionRepository.GetAsync(id);
        if (subscription == null)
            throw new NotFoundException("Subscription not found");
        _subscriptionRepository.Delete(subscription);
        await _subscriptionRepository.SaveChangesAsync();
    }

    public async Task<List<SubscriptionGetDto>> GetAllAsync()
    {
        var subscriptions = _subscriptionRepository.GetAll();
        return _mapper.Map<List<SubscriptionGetDto>>(await subscriptions.ToListAsync());
    }

    public async Task<SubscriptionGetDto> GetAsync(int id)
    {
        var subscription = await _subscriptionRepository.GetAsync(id);
        if (subscription == null)
            throw new NotFoundException("Subscription not found");
        return _mapper.Map<SubscriptionGetDto>(subscription);
    }

    public async Task<Paginate<SubscriptionGetDto>> GetPaginateAsync(int index = 0, int size = 10)
    {
        var paginatedSubsriptions = await _subscriptionRepository.GetPaginateAsync(index: index, size: size);
        return _mapper.Map<Paginate<SubscriptionGetDto>>(paginatedSubsriptions);
    }

    public async Task UpdateAsync(SubscriptionUpdateDto dto, int id)
    {
        var subscription = await _subscriptionRepository.GetAsync(id);
        if (subscription == null)
            throw new NotFoundException("Subscription not found");
        subscription = _mapper.Map(dto, subscription);
        _subscriptionRepository.Update(subscription);
        await _subscriptionRepository.SaveChangesAsync();
    }
}
