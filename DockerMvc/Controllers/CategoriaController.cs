using System.Linq;
using DockerMvc.Data;
using Microsoft.AspNetCore.Mvc;

namespace DockerMvc.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly BaseDbContext _context;

        public CategoriaController(BaseDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categorias = _context.Categorias.ToList();
            return View(categorias);
        }
        
    }
}