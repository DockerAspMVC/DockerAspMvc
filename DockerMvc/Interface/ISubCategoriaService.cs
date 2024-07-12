using System.Collections.Generic;
using System.Threading.Tasks;
using DockerMvc.Controllers;
using Microsoft.AspNetCore.Http;
using SubCategoria = DockerMvc.Models.SubCategoria;

namespace DockerMvc.Interface
{
    public interface ISubCategoriaService
    {
        Task<string> SaveImageAsync(IFormFile image);
        Task<string> UpdateImageAsync(IFormFile newImage, string currentImagePath);
        Task<IEnumerable<SubCategoria>> GetSubCategoriasAsync();
        Task<SubCategoria> GetSubCategoriaByIdAsync(int id);
        Task CreateSubCategoriaAsync(SubCategoria subCategoria);
        Task UpdateSubCategoriaAsync(SubCategoria subCategoria);
        Task DeleteSubCategoriaAsync(int id);
    }
}