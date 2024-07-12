using DockerMvc.Data;
using Microsoft.AspNetCore.Mvc;

namespace DockerMvc.Controllers
{
    public class SubCategoria : Controller
    {
        private readonly BaseDbContext _context;

        public SubCategoria(BaseDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        
    }
}