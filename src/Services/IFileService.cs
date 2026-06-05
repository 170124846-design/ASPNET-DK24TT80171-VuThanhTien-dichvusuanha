using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DatDichVuSuaChuaNhaCua.Services
{
    public interface IFileService
    {
        Task<string> SaveImageAsync(IFormFile file, string folderName);
    }
}


