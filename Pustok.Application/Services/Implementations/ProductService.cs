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
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IEmailService _emailService;

    public ProductService(IProductRepository productRepository,
        IMapper mapper, IProductImageRepository productImageRepository,
        ITagRepository tagRepository,
        IProductTagRepository productTagRepository,
        ICategoryRepository categoryRepository,
        IFileService fileService, IBasketItemRepository basketItemRepository, ISubscriptionRepository subscriptionRepository, IEmailService emailService)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _productImageRepository = productImageRepository;
        _tagRepository = tagRepository;
        _productTagRepository = productTagRepository;
        _categoryRepository = categoryRepository;
        _fileService = fileService;
        _basketItemRepository = basketItemRepository;
        _subscriptionRepository = subscriptionRepository;
        _emailService = emailService;
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
        if (await _productRepository.DoesExistAsync(x => x.ProductCode.ToUpper() == dto.ProductCode.ToUpper()))
            throw new AlreadyExistsException("Product with this code already exists");
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
        var subscriptions = await _subscriptionRepository.GetAll().ToListAsync();
        var subject = $"🚀 {createdProduct.Name} just dropped!";

        var body = $"<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <style>\r\n        body {{\r\n            font-family: Arial, sans-serif;\r\n            line-height: 1.6;\r\n            color: #333333;\r\n            background-color: #f9f9f9;\r\n            padding: 20px;\r\n        }}\r\n        .email-container {{\r\n            max-width: 600px;\r\n            margin: 0 auto;\r\n            background: #ffffff;\r\n            border: 1px solid #dddddd;\r\n            border-radius: 8px;\r\n            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);\r\n            padding: 20px;\r\n        }}\r\n        .header {{\r\n            text-align: center;\r\n            color: #ffffff;\r\n            background-color: #ff6b6b;\r\n            padding: 10px 0;\r\n            border-radius: 8px 8px 0 0;\r\n        }}\r\n        .header h1 {{\r\n            margin: 0;\r\n            font-size: 24px;\r\n        }}\r\n        .content {{\r\n            padding: 20px;\r\n        }}\r\n        .content h2 {{\r\n            color: #ff6b6b;\r\n            font-size: 22px;\r\n        }}\r\n        .content p {{\r\n            font-size: 16px;\r\n            margin: 10px 0;\r\n        }}\r\n        .cta {{\r\n            display: inline-block;\r\n            margin-top: 20px;\r\n            padding: 10px 20px;\r\n            background-color: #ff6b6b;\r\n            color: #ffffff;\r\n            text-decoration: none;\r\n            border-radius: 4px;\r\n            font-size: 16px;\r\n        }}\r\n        .cta:hover {{\r\n            background-color: #e55555;\r\n        }}\r\n        .footer {{\r\n            text-align: center;\r\n            font-size: 12px;\r\n            color: #888888;\r\n            margin-top: 20px;\r\n        }}\r\n    </style>\r\n</head>\r\n<body>\r\n    <div class=\"email-container\">\r\n        <div class=\"header\">\r\n            <h1>🚀 New Drop Alert!</h1>\r\n        </div>\r\n        <div class=\"content\">\r\n            <h2>YOOO {createdProduct.Name} is HERE! 🎉</h2>\r\n            <p>We made something cool. You want it. We got it.</p>\r\n            <p>Go check it out before it's gone. No pressure lol.</p>\r\n            <img style=\"height:300px; width:200px;\" src=\"{mainImgUrl}\" />\r\n            <br />\r\n            <a href=\"#\" class=\"cta\">Grab It Now</a>\r\n        </div>\r\n        <div class=\"footer\">\r\n            <p>Peace ✌️</p>\r\n        </div>\r\n    </div>\r\n</body>\r\n</html>";
        foreach (var subscription in subscriptions)
        {
            _emailService.SendEmail(subscription.Email, subject, body);
        }
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

    public async Task<ProductDetailDto> GetDetailedProductAsync(int id)
    {
        var product = await _productRepository.GetAsync(id, include: x => x.Include(y => y.ProductImages).
         Include(x => x.Category).
         Include(x => x.ProductTags).
         ThenInclude(y => y.Tag));
        if (product == null)
            throw new NotFoundException();
        product.ViewCount++;
        _productRepository.Update(product);
        await _productRepository.SaveChangesAsync();
        var tagIds = product.ProductTags.Select(x => x.TagId).ToList();
        var categoryRelatedProducts = await _productRepository.GetAll(include: x => x.Include(y => y.ProductImages).
        Include(x => x.Category).
        Include(x => x.ProductTags).
        ThenInclude(y => y.Tag),
        predicate:x=>x.Id!=id &&(x.CategoryId==product.CategoryId)).Take(5).ToListAsync();
        if(categoryRelatedProducts.Count<5)
        {
            var tagRelatedProducts = await _productRepository.GetAll(include: x => x.Include(y => y.ProductImages)
                .Include(x => x.Category)
                .Include(x => x.ProductTags)
                .ThenInclude(y => y.Tag),
                predicate:x=>x.Id!=id &&
                x.CategoryId!=product.CategoryId&&
                x.ProductTags.Any(pt=>tagIds.Contains(pt.TagId))).Take(5-categoryRelatedProducts.Count).ToListAsync();
            categoryRelatedProducts.AddRange(tagRelatedProducts);
        }
        var pDto = _mapper.Map<ProductGetDto>(product);
        var relatedDtos = _mapper.Map<List<ProductGetDto>>( categoryRelatedProducts);
        var detailDto = new ProductDetailDto
        {
            Product = pDto,
            RelatedProducts = relatedDtos
        };
        return detailDto;
    }

    public async Task<List<ProductGetDto>> GetDiscountedProductsAsync()
    {
        var products = _productRepository.GetAll(predicate: x => x.Discount > 0,
            orderBy: x => x.OrderByDescending(y => y.Discount),
            include: x => x.Include(y => y.ProductImages).
        Include(x => x.Category).
        Include(x => x.ProductTags).
        ThenInclude(y => y.Tag)).Take(7).AsSplitQuery();
        return _mapper.Map<List<ProductGetDto>>(await products.ToListAsync());
    }

    public async Task<List<ProductGetDto>> GetMostSoldProductsAsync()
    {
        var products = _productRepository.GetAll(predicate: x => x.BasketItems.Any(),
            orderBy: x => x.OrderByDescending(y => y.BasketItems.Sum(z => z.Count)),
            include: x => x.Include(y => y.ProductImages).
        Include(x => x.Category).
        Include(x => x.ProductTags).
        ThenInclude(y => y.Tag)).Take(12).AsSplitQuery();
        return _mapper.Map<List<ProductGetDto>>(await products.ToListAsync());
    }

    public async Task<List<ProductGetDto>> GetNewestProductsAsync()
    {
        var products = _productRepository.GetAll(predicate: x => x.CreatedAt >= DateTimeOffset.UtcNow.AddDays(-3),
            orderBy: x => x.OrderByDescending(y => y.CreatedAt), include: x => x.Include(y => y.ProductImages).
        Include(x => x.Category).
        Include(x => x.ProductTags).
        ThenInclude(y => y.Tag)).Take(12).AsSplitQuery();
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

    public async Task<List<ProductGetDto>> GetProductsByCategoryAsync(int categoryId)
    {
        if (!await _categoryRepository.DoesExistAsync(x => x.Id == categoryId))
            throw new NotFoundException("Category not found");
        var products = _productRepository.GetAll(predicate: x => x.CategoryId == categoryId, include: x => x.Include(y => y.ProductImages).
        Include(x => x.Category).
        Include(x => x.ProductTags).
        ThenInclude(y => y.Tag)).AsSplitQuery();
        var dtos = _mapper.Map<List<ProductGetDto>>(await products.ToListAsync());
        return dtos;
    }

    public async Task<List<ProductGetDto>> GetProductsByNameAsync(string name)
    {
        var products = _productRepository.GetAll(include: x => x.Include(y => y.ProductImages).
        Include(x => x.Category).
        Include(x => x.ProductTags).
        ThenInclude(y => y.Tag),
        predicate: x => EF.Functions.Like(x.Name,$"%{name}%")|| EF.Functions.Like(x.Description, $"%{name}%"));
        var dtos = _mapper.Map<List<ProductGetDto>>(await products.ToListAsync());
        return dtos;
    }

    public async Task UpdateAsync(ProductUpdateDto dto, int id)
    {
        var product = await _productRepository.GetAsync(id,enableTracking:true);
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
        if (dto.ProductCode != null)
        {
            if (await _productRepository.DoesExistAsync(x => x.ProductCode.ToUpper() == dto.ProductCode.ToUpper()&&x.Id!=id))
                throw new AlreadyExistsException("Product with this code already exists");
        }
        Console.WriteLine($"Before: Name={product.Name}, Description={product.Description}");
        product = _mapper.Map(dto, product);
        Console.WriteLine($"After: Name={product.Name}, Description={product.Description}");
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
        //_productRepository.Update(product);
        var result = await _productRepository.SaveChangesAsync();
        Console.WriteLine($"Changes saved: {result}");
    }
}
