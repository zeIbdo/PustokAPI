using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Pustok.Application.AppSettingModels;
using Pustok.Application.Services.Abstractions;
using System.Net;

namespace Pustok.Application.Services.Implementations;

public class CloudinaryService : IFileService
{
    private readonly Cloudinary _cloudinary;
    private readonly IConfiguration _configuration;
    private readonly CloudinaryOptions _options;

    public CloudinaryService(IConfiguration configuration)
    {
        _configuration = configuration;
        _options = _configuration.GetSection("CloudinarySettings").Get<CloudinaryOptions>() ?? new();
        var myAccount = new Account { ApiKey = _options.ApiKey, ApiSecret = _options.ApiSecret, Cloud = _options.CloudName };
        _cloudinary = new Cloudinary(myAccount);
        _cloudinary.Api.Secure = true;
    }

    public async Task<string> CreateFileAsync(IFormFile file)
    {
        var fileName = string.Concat(Guid.NewGuid(), file.FileName.Substring(file.FileName.LastIndexOf('.')));
        var uploadResult = new ImageUploadResult();
        if (file.Length > 0)
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, stream),
                Folder = "PustokApiJs"
            };
            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }
        var url = uploadResult.SecureUrl.ToString();
        return url;
    }

    //static string GetPublicIdFromUrl(string url)
    //{
    //    var imageUrl = new Uri(url);
    //    var segments = imageUrl.AbsolutePath.Split('/');
    //    var fileName = segments[^1];
    //    return fileName.Substring(0, fileName.LastIndexOf('.'));
    //}

    public async Task<bool> RemoveFileAsync(string path)
    {
        string publicIdWithExtension = path.Substring(path.LastIndexOf("PustokApiJs"));
        var fileName = publicIdWithExtension.Substring(0, publicIdWithExtension.LastIndexOf('.'));
        var deletionParams = new DeletionParams(fileName)
        {
            ResourceType = ResourceType.Image
        };
        var result = await _cloudinary.DestroyAsync(deletionParams);
        if (result.StatusCode == HttpStatusCode.OK)
            return true;
        else
            return false;
    }
}
