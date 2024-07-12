using System.Collections.Generic;
using System.Threading.Tasks;
using DockerMvc.Models;
using Microsoft.AspNetCore.Http;

namespace DockerMvc.Interface
{
    public interface IProductoService
    {
        Task<string> SaveImageAsync(IFormFile image);
        Task<string> UpdateImageAsync(IFormFile newImage, string currentImagePath);
        Task<Productos> GetProductoByIdAsync(int id);
        Task<IEnumerable<Productos>> GetProductosAsync();
        Task AddProductoAsync(Productos producto);
        Task UpdateProductoAsync(Productos producto);
        Task DeleteProductoAsync(Productos producto);
    }
}