using System.Collections.Generic;
using System.Threading.Tasks;
using DockerMvc.Models;

namespace DockerMvc.Interface
{
    public interface ISubCategoriaService
    {
        Task<List<SubCategoria>> GetSubCategoriasAsync();
        Task<SubCategoria> GetSubCategoriaByIdAsync(int id);
        Task CreateSubCategoriaAsync(SubCategoria subCategoria);
        Task UpdateSubCategoriaAsync(SubCategoria subCategoria);
        Task DeleteSubCategoriaAsync(int id);
    }
}