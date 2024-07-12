using System.Linq;
using System.Security.Claims;
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
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

            if (userEmail == null)
            {
                return NotFound();
            }

            var userProfile = await _context.Profiles.FirstOrDefaultAsync(p => p.ProEmail == userEmail);

            if (userProfile == null)
            {
                return NotFound();
            }

            return View(userProfile);
        }
    }
}