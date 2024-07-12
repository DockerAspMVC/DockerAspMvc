using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DockerMvc.Data;
using DockerMvc.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DockerMvc.Interface.Services
{
    public class SubCategoriaService : ISubCategoriaService
    {
        private readonly BaseDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public SubCategoriaService(BaseDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<string> SaveImageAsync(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return "/images/default-image.jpg"; // Ruta por defecto si no hay imagen
            }

            var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
            var uniqueFileName = Path.GetRandomFileName() + "_" + image.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return "/images/" + uniqueFileName;
        }

        public async Task<string> UpdateImageAsync(IFormFile newImage, string currentImagePath)
        {
            if (newImage == null || newImage.Length == 0)
            {
                return currentImagePath; // Mantener la imagen actual si no se proporciona una nueva
            }

            var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
            var uniqueFileName = Path.GetRandomFileName() + "_" + newImage.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Eliminar la imagen actual si no es la imagen por defecto
            if (!string.IsNullOrEmpty(currentImagePath) && currentImagePath != "/images/default-image.jpg")
            {
                var currentFilePath = Path.Combine(_hostEnvironment.WebRootPath, currentImagePath.TrimStart('/'));
                if (File.Exists(currentFilePath))
                {
                    File.Delete(currentFilePath);
                }
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await newImage.CopyToAsync(fileStream);
            }

            return "/images/" + uniqueFileName;
        }

        public async Task<IEnumerable<SubCategoria>> GetSubCategoriasAsync()
        {
            return await _context.SubCategorias
                .Include(sc => sc.Productos)
                .Include(sc => sc.Categoria)
                .ToListAsync();
        }

        public async Task<SubCategoria> GetSubCategoriaByIdAsync(int id)
        {
            return await _context.SubCategorias
                .Include(sc => sc.Productos)
                .Include(sc => sc.Categoria)
                .FirstOrDefaultAsync(sc => sc.SubId == id);
        }

        public async Task CreateSubCategoriaAsync(SubCategoria subCategoria)
        {
            _context.SubCategorias.Add(subCategoria);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSubCategoriaAsync(SubCategoria subCategoria)
        {
            _context.SubCategorias.Update(subCategoria);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSubCategoriaAsync(int id)
        {
            var subCategoria = await _context.SubCategorias.FindAsync(id);
            if (subCategoria != null)
            {
                _context.SubCategorias.Remove(subCategoria);
                await _context.SaveChangesAsync();
            }
        }
    }
}
