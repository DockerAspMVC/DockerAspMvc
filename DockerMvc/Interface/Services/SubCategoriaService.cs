using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DockerMvc.Data;
using DockerMvc.Interface;
using DockerMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace DockerMvc.Services
{
    public class SubCategoriaService : ISubCategoriaService
    {
        private readonly BaseDbContext _context;

        public SubCategoriaService(BaseDbContext context)
        {
            _context = context;
        }

        public async Task<List<SubCategoria>> GetSubCategoriasAsync()
        {
            return await _context.SubCategorias
                .Include(sc => sc.Categoria)
                .Include(sc => sc.SubCategoriaProductos)
                .ThenInclude(scp => scp.Productos)
                .ToListAsync();
        }

        public async Task<SubCategoria> GetSubCategoriaByIdAsync(int id)
        {
            return await _context.SubCategorias
                .Include(sc => sc.Categoria)
                .Include(sc => sc.SubCategoriaProductos)
                .ThenInclude(scp => scp.Productos)
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