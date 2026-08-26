using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pustok.Application.Dtos.ProductDtos;
using Pustok.Application.Exceptions;
using Pustok.Application.Services.Abstractions;
using Pustok.Domain.Entities;
using Pustok.Infrastructure.Paging;
using Pustok.Infrastructure.Repositories.Abstractions;

namespace Pustok.Application.Services.Implementations;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IProductImageRepository _productImageRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IProductTagRepository _productTagRepository;
    private readonly IFileService _fileService;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBasketItemRepository _basketItemRepository;

    public ProductService(IProductRepository productRepository,
        IMapper mapper, IProductImageRepository productImageRepository,
        ITagRepository tagRepository,
        IProductTagRepository productTagRepository,
        ICategoryRepository categoryRepository,
        IFileService fileService, IBasketItemRepository basketItemRepository)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _productImageRepository = productImageRepository;
        _tagRepository = tagRepository;
        _productTagRepository = productTagRepository;
        _categoryRepository = categoryRepository;
        _fileService = fileService;
        _basketItemRepository = basketItemRepository;
    }

    public async Task<int> CreateAsync(ProductCreateDto dto)
    {
        if (!await _categoryRepository.DoesExistAsync(x => x.Id == dto.CategoryId))
            throw new NotFoundException("Category not found");
        if (dto.TagIds != null)
        {
            foreach (var tag in dto.TagIds)
            {
                if (!await _tagRepository.DoesExistAsync(x => x.Id == tag))
                    throw new NotFoundException("Tag not found");
            }
        }
        var product = _mapper.Map<Product>(dto);
        var mainImgUrl = await _fileService.CreateFileAsync(dto.MainImage);
        var additionalImgUrls = new List<string>();
        foreach (var img in dto.AdditionalImages)
        {
            additionalImgUrls.Add(await _fileService.CreateFileAsync(img));
        }
        var createdProduct = await _productRepository.CreateAsync(product);
        await _productRepository.SaveChangesAsync();
        await _productImageRepository.CreateAsync(new ProductImage { ImageUrl = mainImgUrl, IsMain = true, ProductId = createdProduct.Id });
        foreach (var imgUrl in additionalImgUrls)
        {
            await _productImageRepository.CreateAsync(new ProductImage { ImageUrl = imgUrl, IsMain = false, ProductId = createdProduct.Id });
        }
        if (dto.TagIds != null)
        {
            foreach (var tagId in dto.TagIds)
            {
                await _productTagRepository.CreateAsync(new ProductTag { TagId = tagId, ProductId = createdProduct.Id });
            }
        }
        await _tagRepository.SaveChangesAsync();
        return createdProduct.Id;
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _productRepository.GetAsync(id);
        if (product == null)
            throw new NotFoundException();
        if (await _basketItemRepository.DoesExistAsync(x => x.ProductId == product.Id))
            throw new InvalidDeletionException("Cannot delete a product with exsting basket items");
        _productRepository.Delete(product);
        var productImgs = _productImageRepository.GetAll(predicate: x => x.ProductId == product.Id).ToList();
        foreach (var img in productImgs)
        {
            var deletion = await _fileService.RemoveFileAsync(img.ImageUrl);
            if (!deletion)
                throw new ImageDeletionException();

            _productImageRepository.Delete(img);
        }
        foreach (var productTag in _productTagRepository.GetAll(predicate: x => x.ProductId == product.Id).ToList())
        {
            _productTagRepository.Delete(productTag);
        }
        await _productRepository.SaveChangesAsync();
    }

    public async Task<List<ProductGetDto>> GetAllAsync()
    {
        var products = _productRepository.GetAll(include: x => x.Include(y => y.ProductImages).
        Include(x => x.Category).
        Include(x => x.ProductTags).
        ThenInclude(y => y.Tag)).AsSplitQuery();
        return _mapper.Map<List<ProductGetDto>>(await products.ToListAsync());
    }

    public async Task<ProductGetDto> GetAsync(int id)
    {
        var product = await _productRepository.GetAsync(id, include: x => x.Include(y => y.ProductImages).
        Include(x => x.Category).
        Include(x => x.ProductTags).
        ThenInclude(y => y.Tag));
        if (product == null)
            throw new NotFoundException();
        return _mapper.Map<ProductGetDto>(product);
    }

    public async Task<List<ProductGetDto>> GetDiscountedProducts()
    {
        var products = _productRepository.GetAll(predicate: x => x.Discount > 0, orderBy: x => x.OrderByDescending(y => y.Discount));
        return _mapper.Map<List<ProductGetDto>>( await products.ToListAsync());
    }

    public async Task<List<ProductGetDto>> GetNewestProducts()
    {
        var products =  _productRepository.GetAll(predicate: x => x.CreatedAt >= DateTimeOffset.UtcNow.AddDays(-3),
            orderBy: x => x.OrderByDescending(y => y.CreatedAt));
        return _mapper.Map<List<ProductGetDto>>(await products.ToListAsync());
    }

    public async Task<Paginate<ProductGetDto>> GetPaginateAsync(int index = 0, int size = 10)
    {
        var products = await _productRepository.GetPaginateAsync(include: x => x.Include(y => y.ProductImages).
        Include(x => x.Category).
        Include(x => x.ProductTags).
        ThenInclude(y => y.Tag), index: index, size: size);
        return _mapper.Map<Paginate<ProductGetDto>>(products);
    }

    public async Task<List<ProductGetDto>> GetProductsByCategory(int categoryId)
    {
        if (!await _categoryRepository.DoesExistAsync(x => x.Id == categoryId))
            throw new NotFoundException("Category not found");
        var products = _productRepository.GetAll(predicate: x => x.CategoryId == categoryId, include: x => x.Include(y => y.ProductImages));
        var dtos = _mapper.Map<List<ProductGetDto>>(await products.ToListAsync());
        return dtos;
    }

    public async Task UpdateAsync(ProductUpdateDto dto, int id)
    {
        var product = await _productRepository.GetAsync(id, include: x => x.Include(y => y.ProductImages).
                Include(x => x.Category).
                Include(x => x.ProductTags).
                ThenInclude(y => y.Tag));
        if (product == null)
            throw new NotFoundException();
        if (dto.CategoryId != null)
        {
            if (!await _categoryRepository.DoesExistAsync(x => x.Id == dto.CategoryId))
                throw new NotFoundException("Category not found");
        }
        if (dto.TagIds != null)
        {
            foreach (var tag in dto.TagIds)
            {
                if (!await _tagRepository.DoesExistAsync(x => x.Id == tag))
                    throw new NotFoundException("Tag not found");
            }
        }
        product = _mapper.Map(dto, product);
        if (dto.MainImage != null)
        {
            var oldMainImg = await _productImageRepository.GetAsync(x => x.ProductId == product.Id && x.IsMain == true);
            var mainImgDeletion = await _fileService.RemoveFileAsync(oldMainImg!.ImageUrl);
            if (!mainImgDeletion)
                throw new ImageDeletionException();

            _productImageRepository.Delete(oldMainImg);
            var newMainImgUrl = await _fileService.CreateFileAsync(dto.MainImage);
            await _productImageRepository.CreateAsync(new ProductImage { ImageUrl = newMainImgUrl, IsMain = true, ProductId = product.Id });
        }
        if (dto.AdditionalImages != null)
        {
            var oldAdditionalImgs = _productImageRepository.GetAll(x => x.ProductId == product.Id && x.IsMain == false);
            foreach (var oldImg in await oldAdditionalImgs.ToListAsync())
            {
                var deletion = await _fileService.RemoveFileAsync(oldImg.ImageUrl);
                if (!deletion)
                    throw new ImageDeletionException();

                _productImageRepository.Delete(oldImg);
            }
            foreach (var newImg in dto.AdditionalImages)
            {
                var newImgUrl = await _fileService.CreateFileAsync(newImg);
                await _productImageRepository.CreateAsync(new ProductImage { ImageUrl = newImgUrl, ProductId = product.Id, IsMain = false });
            }
        }
        if (dto.TagIds != null)
        {
            var oldPTags = await _productTagRepository.GetAll(predicate: x => x.ProductId == product.Id).ToListAsync();
            foreach (var oldTag in oldPTags)
            {
                _productTagRepository.Delete(oldTag);
            }
            foreach (var newTag in dto.TagIds)
            {
                await _productTagRepository.CreateAsync(new ProductTag { ProductId = product.Id, TagId = newTag });
            }
        }
        await _productRepository.SaveChangesAsync();

    }
}
