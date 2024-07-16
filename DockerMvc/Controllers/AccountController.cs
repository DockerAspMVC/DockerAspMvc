using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using DockerMvc.Data;
using DockerMvc.Interface;
using DockerMvc.Models;
using DockerMvc.ModelView;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using BCrypt.Net;
using DockerMvc.Extensions;

namespace DockerMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly BaseDbContext _context;
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        private readonly IDataProtector _protector;

        public AccountController(BaseDbContext context, IAuthService authService, IEmailService emailService, IDataProtectionProvider dataProtectionProvider)
        {
            _context = context;
            _authService = authService;
            _emailService = emailService;
            _protector = dataProtectionProvider.CreateProtector("DockerMvc.AccountController");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
{
    const int MaxFailedAttempts = 3;
    const int LockoutDuration = 3; // en segundos

    if (ModelState.IsValid)
    {
        var user = await _context.Profiles.SingleOrDefaultAsync(u => u.ProEmail == model.Email);

        if (user != null)
        {
            string userKey = user.ProId.ToString(); // Assuming user.ProId is unique

            // Verificar si el usuario está bloqueado temporalmente
            var failedAttempts = HttpContext.Session.GetInt32("FailedLoginAttempts", userKey) ?? 0;
            var lockoutEndTime = HttpContext.Session.GetDateTime("LockoutEndTime", userKey);
            if (failedAttempts >= MaxFailedAttempts && lockoutEndTime.HasValue && DateTime.UtcNow < lockoutEndTime.Value)
            {
                ModelState.AddModelError(string.Empty, "Tu cuenta está bloqueada temporalmente. Inténtalo más tarde.");
                return View(model);
            }
            else if (lockoutEndTime.HasValue && DateTime.UtcNow >= lockoutEndTime.Value)
            {
                // Reiniciar el bloqueo después de que el tiempo haya pasado
                HttpContext.Session.Remove($"FailedLoginAttempts_{userKey}");
                HttpContext.Session.Remove($"LockoutEndTime_{userKey}");
            }

            var isValidPassword = BCrypt.Net.BCrypt.Verify(model.Password, user.ProPassword);

            if (isValidPassword)
            {
                // Reiniciar el contador de intentos fallidos
                HttpContext.Session.Remove($"FailedLoginAttempts_{userKey}");
                HttpContext.Session.Remove($"LockoutEndTime_{userKey}");

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

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);

                if (roles.Contains("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (roles.Contains("User"))
                {
                    return RedirectToAction("Index", "User");
                }
            }
            else
            {
                // Incrementar el contador de intentos fallidos
                failedAttempts++;
                HttpContext.Session.SetInt32("FailedLoginAttempts", failedAttempts, userKey);

                if (failedAttempts >= MaxFailedAttempts)
                {
                    HttpContext.Session.SetDateTime("LockoutEndTime", DateTime.UtcNow.AddSeconds(LockoutDuration), userKey);
                    ModelState.AddModelError(string.Empty, "Tu cuenta está bloqueada temporalmente por múltiples intentos fallidos. Inténtalo más tarde.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Intento de inicio de sesión no válido. Intentos restantes: {MaxFailedAttempts - failedAttempts}");
                }
            }
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Intento de inicio de sesión no válido.");
        }
    }

    return View(model);
}

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

                var profile = new Profile
                {
                    ProName = model.Name,
                    ProLastname = model.Lastname,
                    ProEmail = model.Email,
                    ProPassword = hashedPassword,
                    ProImage = await SaveImageAsync(model.Image)
                };

                _context.Add(profile);
                await _context.SaveChangesAsync();

                // Asignar rol al usuario
                var userRole = await _context.Roles.SingleOrDefaultAsync(r => r.RoleName == "User");
                if (userRole != null)
                {
                    var roleProfile = new RoleProfile
                    {
                        ProId = profile.ProId,
                        RoleId = userRole.RoleId
                    };

                    _context.Add(roleProfile);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Index", "User");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _context.Profiles.SingleOrDefaultAsync(u => u.ProEmail == model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "No se encontró ningún usuario con ese correo electrónico.");
                return View(model);
            }

            // Generar token de restablecimiento
            var token = _protector.Protect($"{user.ProEmail}|{DateTime.UtcNow.AddHours(1)}");

            var resetLink = Url.Action("ResetPassword", "Account", new { token = token }, Request.Scheme);

            // Log the reset link to verify it
            Console.WriteLine(resetLink);

            // Aquí deberías enviar el enlace por correo electrónico
            await _emailService.SendEmailAsync(model.Email, "Reset Password",
                $"Por favor restablece tu contraseña usando este enlace: {resetLink}");

            // Redirigir a la vista de confirmación de olvido de contraseña
            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            try
            {
                var unprotectedToken = _protector.Unprotect(token);
                var tokenParts = unprotectedToken.Split('|');
                var email = tokenParts[0];
                var expiry = DateTime.Parse(tokenParts[1]);

                if (expiry < DateTime.UtcNow)
                {
                    return RedirectToAction("Index", "Home");
                }

                var model = new ResetPasswordViewModel { Token = token, Email = email };
                return View(model);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var unprotectedToken = _protector.Unprotect(model.Token);
                var tokenParts = unprotectedToken.Split('|');
                var email = tokenParts[0];
                var expiry = DateTime.Parse(tokenParts[1]);

                if (expiry < DateTime.UtcNow)
                {
                    ModelState.AddModelError(string.Empty, "Token inválido o expirado.");
                    return View(model);
                }

                var user = await _context.Profiles.SingleOrDefaultAsync(u => u.ProEmail == email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "No se encontró ningún usuario con ese correo electrónico.");
                    return View(model);
                }

                user.ProPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
                await _context.SaveChangesAsync();

                return RedirectToAction("ResetPasswordConfirmation");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Token inválido o expirado.");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
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
