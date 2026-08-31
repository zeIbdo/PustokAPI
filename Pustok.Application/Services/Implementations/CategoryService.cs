using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pustok.Application.Dtos.CategoryDtos;
using Pustok.Application.Exceptions;
using Pustok.Application.Services.Abstractions;
using Pustok.Domain.Entities;
using Pustok.Infrastructure.Paging;
using Pustok.Infrastructure.Repositories.Abstractions;

namespace Pustok.Application.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IProductRepository productRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<int> CreateAsync(CategoryCreateDto dto)
    {
        if (await _categoryRepository.DoesExistAsync(x => x.Name.ToLower() == dto.Name.ToLower()))
            throw new AlreadyExistsException("Category with this name already exists");
        var category = _mapper.Map<Category>(dto);
        if (dto.ParentId != null)
        {
            if (!(await _categoryRepository.DoesExistAsync(x => x.Id == dto.ParentId)))
                throw new NotFoundException("Category not found");
        }
        var createdCategory = await _categoryRepository.CreateAsync(category);
        await _categoryRepository.SaveChangesAsync();
        return createdCategory.Id;
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _categoryRepository.GetAsync(id);
        if (category == null)
            throw new NotFoundException("Category not found");
        if (await _productRepository.DoesExistAsync(x => x.CategoryId == category.Id))
            throw new InvalidDeletionException("Cannot delete category with existing products");
        foreach (var child in await _categoryRepository.GetAllDescendantsAsync(category.Id))
        {
            if (await _productRepository.DoesExistAsync(x => x.CategoryId == child.Id))
                throw new InvalidDeletionException("Cannot delete category with existing products");
        }

        _categoryRepository.Delete(category);
        await _categoryRepository.SaveChangesAsync();
    }

    public async Task<List<CategoryGetDto>> GetAllAsync()
    {
        var categories = _categoryRepository.GetAll(include: x => x.Include(y => y.Children));
        return _mapper.Map<List<CategoryGetDto>>(await categories.ToListAsync());
    }

    public async Task<CategoryGetDto> GetAsync(int id)
    {
        var category = await _categoryRepository.GetAsync(id,include:x=>x.Include(y=>y.Parent)!);
        if (category == null)
            throw new NotFoundException();
        return _mapper.Map<CategoryGetDto>(category);
    }

    public async Task<Paginate<CategoryGetDto>> GetPaginateAsync(int index = 0, int size = 10)
    {
        var categories = await _categoryRepository.GetPaginateAsync(include: x => x.Include(y => y.Children), index: index, size: size);
        return _mapper.Map<Paginate<CategoryGetDto>>(categories);
    }

    public async Task UpdateAsync(CategoryUpdateDto dto, int id)
    {
        var category = await _categoryRepository.GetAsync(id);
        if (category == null)
            throw new NotFoundException();
        if (dto.ParentId != null)
        {
            if (!(await _categoryRepository.DoesExistAsync(x => x.Id == dto.ParentId)))
                throw new NotFoundException("Category not found");
            if (dto.ParentId == id)
                throw new InvalidParentIdException("Parent id cannot be same as its own id");
            foreach (var child in await _categoryRepository.GetAllDescendantsAsync(category.Id))
            {
                if (child.Id == dto.ParentId)
                    throw new InvalidParentIdException("Parent id cannot be its child id");
            }
        }
        if (dto.Name != null)
        {
            if (await _categoryRepository.DoesExistAsync(x => x.Name.ToLower() == dto.Name.ToLower() && x.Id != category.Id))
                throw new AlreadyExistsException("Category with this name already exists");
        }
        category = _mapper.Map(dto, category);
        if (dto.ParentId == null)
            category.ParentId = null;
        _categoryRepository.Update(category);
        await _categoryRepository.SaveChangesAsync();
    }
}
