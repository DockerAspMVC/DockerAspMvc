using System.Collections.Generic;
using System.Threading.Tasks;
using DockerMvc.Data;
using DockerMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace DockerMvc.Interface.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly BaseDbContext _context;

        public CategoriaService(BaseDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Categoria>> GetCategoriasAsync()
        {
            return await _context.Categorias.ToListAsync();
        }

        public async Task<Categoria> GetCategoriaByIdAsync(int id)
        {
            return await _context.Categorias.FindAsync(id);
        }

        public async Task CreateCategoriaAsync(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoriaAsync(Categoria categoria)
        {
            _context.Categorias.Update(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoriaAsync(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
            }
        }
    }
}