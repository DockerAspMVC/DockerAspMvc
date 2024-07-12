using System.Linq;
using DockerMvc.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DockerMvc.Controllers
{
    [Authorize(Policy = "Admin")]
    public class AdminController : Controller
    {
        private readonly BaseDbContext _context;
        public IActionResult Index()
        {
            return View();
        }
        
       
        
       
    }
}