using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DockerMvc.Data;
using DockerMvc.Models;
using DockerMvc.ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DockerMvc.Controllers
{
    [Authorize(Policy = "Admin")]
    public class ProductosController : Controller
    {
        private readonly BaseDbContext _context;
        public ProductosController(BaseDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var productos = _context.Productos.ToList();
            return View(productos);
        }
        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductosViewModel productos)
        {
            if (ModelState.IsValid)
            {
                var newProduct = new Productos
                {
                    Nombre = productos.Nombre,
                    Descripcion = productos.Descripcion,
                    Precio = productos.Precio,
                    Imagen = await SaveImageAsync(productos.Imagen),
                    Stock = productos.Stock
                };
                _context.Productos.Add(newProduct);
                _context.SaveChanges();
                TempData["mensaje"] = "El libro se agrego correctamente";
                return RedirectToAction("Index");
                
            }

            return View(productos);
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
        
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productos = _context.Productos.Find(id);
            if (productos == null)
            {
                return NotFound();
            }
            return View(productos);
        }
        
        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, [Bind("ProduId, Nombre, Descripcion, Imagen, Precio, Stock")] Productos producto, IFormFile nuevaImagen)
{
    if (id != producto.ProduId)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        try
        {
            if (nuevaImagen != null && nuevaImagen.Length > 0)
            {
                var fileExtension = Path.GetExtension(nuevaImagen.FileName).ToLower();
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError(string.Empty, "Solo se permiten archivos de imagen (jpg, jpeg, png, gif).");
                    return View(producto);
                }
                if (!string.IsNullOrEmpty(producto.Imagen) && producto.Imagen != "/images/default-image.jpg")
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", producto.Imagen.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                producto.Imagen = await SaveImageAsync(nuevaImagen);
            }

            _context.Update(producto);
            await _context.SaveChangesAsync();
            TempData["mensaje"] = "El producto se actualizó correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductoExists(producto.ProduId))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
    }
    return View(producto);
}


private bool ProductoExists(int id)
{
    return _context.Productos.Any(e => e.ProduId == id);
}
        
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productos = _context.Productos.Find(id);
            if (productos == null)
            {
                return NotFound();
            }
            return View(productos);
        }
        
        [HttpPost]
        public IActionResult Delete(Productos productos)
        {
            _context.Productos.Remove(productos);
            _context.SaveChanges();
            TempData["mensaje"] = "El libro se elimino correctamente";
            return RedirectToAction("Index");
        }
        
        
    }
}