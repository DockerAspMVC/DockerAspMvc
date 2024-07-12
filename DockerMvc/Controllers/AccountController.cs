using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using DockerMvc.Data;
using DockerMvc.Models;
using DockerMvc.ModelView;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DockerMvc.Interface;

namespace DockerMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly BaseDbContext _context;
        private readonly IAuthService _authService;

        public AccountController(BaseDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _authService.Authenticate(model.Email, model.Password);

                if (user != null)
                {
                    var roles = await _authService.GetRoles(user.ProId);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.ProName),
                        new Claim(ClaimTypes.Email, user.ProEmail)
                    };

                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    if (roles.Contains("Admin"))
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (roles.Contains("User"))
                    {
                        return RedirectToAction("Index", "User");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }        


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var profile = new Profile
                {
                    ProName = model.Name,
                    ProLastname = model.Lastname,
                    ProEmail = model.Email,
                    ProPassword = model.Password,
                    ProImage = await SaveImageAsync(model.Image) 
                };

                _context.Add(profile);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "User");
            }

            return View(model);
        }


        private async Task<string> SaveImageAsync(IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                var fileExtension = Path.GetExtension(image.FileName).ToLower();
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError(string.Empty, "Only image files (jpg, jpeg, png, gif) are allowed.");
                    return "/images/default-image.jpg";
                }

                var fileName = Path.GetFileName(image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                return $"/images/{fileName}";
            }

            return "/images/default-image.jpg";
        }



        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
