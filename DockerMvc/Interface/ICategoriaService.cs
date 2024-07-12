using System.Collections.Generic;
using System.Threading.Tasks;
using DockerMvc.Models;

namespace DockerMvc.Interface
{
    public interface ICategoriaService
    {
        Task<IEnumerable<Categoria>> GetCategoriasAsync();
        Task<Categoria> GetCategoriaByIdAsync(int id);
        Task CreateCategoriaAsync(Categoria categoria);
        Task UpdateCategoriaAsync(Categoria categoria);
        Task DeleteCategoriaAsync(int id);
    }
}