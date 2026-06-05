using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DatDichVuSuaChuaNhaCua.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveImageAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0) return null;

            string folder = Path.Combine(_env.WebRootPath, "images", folderName);
            Directory.CreateDirectory(folder);
            
            string tenFile = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(folder, tenFile);
            
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
            
            return $"/images/{folderName}/{tenFile}";
        }
    }
}


