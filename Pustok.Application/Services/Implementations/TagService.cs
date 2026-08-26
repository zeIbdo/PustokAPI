using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pustok.Application.Dtos.SubscriptionDtos;
using Pustok.Application.Dtos.TagDtos;
using Pustok.Application.Exceptions;
using Pustok.Application.Services.Abstractions;
using Pustok.Domain.Entities;
using Pustok.Infrastructure.Paging;
using Pustok.Infrastructure.Repositories.Abstractions;
using Pustok.Infrastructure.Repositories.Implementations;

namespace Pustok.Application.Services.Implementations;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;
    private readonly IProductTagRepository _productTagRepository;
    private readonly IMapper _mapper;

    public TagService(ITagRepository tagRepository, IMapper mapper, IProductTagRepository productTagRepository)
    {
        _tagRepository = tagRepository;
        _mapper = mapper;
        _productTagRepository = productTagRepository;
    }

    public async Task<int> CreateAsync(TagCreateDto dto)
    {
        if (await _tagRepository.DoesExistAsync(x => x.Name.ToLower() == dto.Name.ToLower()))
            throw new AlreadyExistsException("Tag with this name already exists");
        var tag = _mapper.Map<Tag>(dto);
        var createdTag = await _tagRepository.CreateAsync(tag);
        await _tagRepository.SaveChangesAsync();
        return createdTag.Id;
    }

    public async Task DeleteAsync(int id)
    {
        var tag = await _tagRepository.GetAsync(id);
        if (tag == null)
            throw new NotFoundException("Tag not found");
        if(await _productTagRepository.DoesExistAsync(x=>x.TagId==id))
            throw new InvalidDeletionException();
        _tagRepository.Delete(tag);
        await _tagRepository.SaveChangesAsync();
    }

    public async Task<List<TagGetDto>> GetAllAsync()
    {
        var tags = _tagRepository.GetAll();
        return _mapper.Map<List<TagGetDto>>(await tags.ToListAsync());
    }

    public async Task<TagGetDto> GetAsync(int id)
    {
        var tag = await _tagRepository.GetAsync(id);
        if (tag == null)
            throw new NotFoundException("Tag not found");
        return _mapper.Map<TagGetDto>(tag);
    }

    public async Task<Paginate<TagGetDto>> GetPaginateAsync(int index = 0, int size = 10)
    {
        var paginatedTags = await _tagRepository.GetPaginateAsync(index: index, size: size);
        return _mapper.Map<Paginate<TagGetDto>>(paginatedTags);
    }

    public async Task UpdateAsync(TagUpdateDto dto, int id)
    {
        var tag = await _tagRepository.GetAsync(id);
        if (tag == null)
            throw new NotFoundException("Tag not found");
        if (dto.Name != null)
        {
            if (await _tagRepository.DoesExistAsync(x => x.Name.ToLower() == dto.Name.ToLower()&&x.Id!=id))
                throw new AlreadyExistsException("Tag with this name already exists");
        }
        tag = _mapper.Map(dto, tag);
        _tagRepository.Update(tag);
        await _tagRepository.SaveChangesAsync();
    }
}
