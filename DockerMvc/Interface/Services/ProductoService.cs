using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DockerMvc.Data;
using DockerMvc.Models;
using Microsoft.AspNetCore.Http;

namespace DockerMvc.Interface.Services
{
    using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DockerMvc.Data;
using DockerMvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

public class ProductoService : IProductoService
{
    private readonly BaseDbContext _context;

    public ProductoService(BaseDbContext context)
    {
        _context = context;
    }

    public async Task<string> SaveImageAsync(IFormFile image)
    {
        if (image != null && image.Length > 0)
        {
            var fileExtension = Path.GetExtension(image.FileName).ToLower();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new Exception("Only image files (jpg, jpeg, png, gif) are allowed.");
            }

            var fileName = Path.GetFileName(image.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return $"/images/{fileName}";
        }

        return "/images/default-image.jpg";
    }

    public async Task<string> UpdateImageAsync(IFormFile newImage, string currentImagePath)
    {
        if (newImage != null && newImage.Length > 0)
        {
            if (!string.IsNullOrEmpty(currentImagePath) && currentImagePath != "/images/default-image.jpg")
            {
                var currentFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", currentImagePath.TrimStart('/'));
                if (File.Exists(currentFilePath))
                {
                    File.Delete(currentFilePath);
                }
            }

            return await SaveImageAsync(newImage);
        }

        return currentImagePath;
    }

    public async Task<Productos> GetProductoByIdAsync(int id)
    {
        return await _context.Productos.FindAsync(id);
    }

    public async Task<IEnumerable<Productos>> GetProductosAsync()
    {
        return await _context.Productos.ToListAsync();
    }

    public async Task AddProductoAsync(Productos producto)
    {
        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateProductoAsync(Productos producto)
    {
        _context.Productos.Update(producto);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProductoAsync(Productos producto)
    {
        _context.Productos.Remove(producto);
        await _context.SaveChangesAsync();
    }
}


}