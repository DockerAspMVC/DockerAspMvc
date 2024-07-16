using System.Linq;
using System.Threading.Tasks;
using DockerMvc.Data;
using DockerMvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DockerMvc.Controllers
{
    [Authorize(Policy = "User")]
    public class UserController : Controller
    {
        private readonly BaseDbContext _context;

        public UserController(BaseDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Cargar las subcategorías con productos asociados
            var subCategoriasConProductos = await _context.SubCategorias
                .Include(sc => sc.Categoria) // Incluir la categoría asociada a la subcategoría
                .Include(sc => sc.SubCategoriaProductos) // Incluir productos asociados a las subcategorías
                .ThenInclude(scp => scp.Productos) // Incluir los detalles de los productos
                .ToListAsync();

            return View(subCategoriasConProductos); // Pasar subcategorías con productos a la vista
        }
    }
}