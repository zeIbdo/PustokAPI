using Microsoft.AspNetCore.Http;

namespace Pustok.Application.Services.Abstractions;

public interface IFileService
{
    Task<string> CreateFileAsync(IFormFile file);
    Task<bool> RemoveFileAsync(string path);
}
