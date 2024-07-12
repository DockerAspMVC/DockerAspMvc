using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DockerMvc.Data;
using DockerMvc.Models;
using DockerMvc.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DockerMvc.Controllers
{
    [Authorize(Policy = "Admin")]
    public class SubCategoriaController : Controller
    {
        private readonly BaseDbContext _context;
        private readonly ISubCategoriaService _subCategoriaService;

        public SubCategoriaController(BaseDbContext context, ISubCategoriaService subCategoriaService)
        {
            _context = context;
            _subCategoriaService = subCategoriaService;
        }

        // GET: SubCategoria
        public async Task<IActionResult> Index()
        {
            var subCategorias = await _subCategoriaService.GetSubCategoriasAsync();
            return View(subCategorias);
        }

        // GET: SubCategoria/Details/5
        

        // GET: SubCategoria/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Productos = await _context.Productos.ToListAsync();
            ViewBag.Categorias = await _context.Categorias.ToListAsync();

            return View();
        }

        // POST: SubCategoria/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCategoria subCategoria, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Guardar la imagen si se proporciona
                    subCategoria.Image = await _subCategoriaService.SaveImageAsync(Image);

                    _context.Add(subCategoria);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error al crear la SubCategoría: {ex.Message}");
                }
            }

            ViewBag.Productos = await _context.Productos.ToListAsync();
            ViewBag.Categorias = await _context.Categorias.ToListAsync();

            return View(subCategoria);
        }

        // GET: SubCategoria/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategoria = await _subCategoriaService.GetSubCategoriaByIdAsync(id.Value);
            if (subCategoria == null)
            {
                return NotFound();
            }

            ViewBag.Productos = await _context.Productos.ToListAsync();
            ViewBag.Categorias = await _context.Categorias.ToListAsync();

            return View(subCategoria);
        }

        // POST: SubCategoria/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubCategoria subCategoria, IFormFile Image)
        {
            if (id != subCategoria.SubId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Actualizar la imagen si se proporciona
                    subCategoria.Image = await _subCategoriaService.UpdateImageAsync(Image, subCategoria.Image);

                    _context.Update(subCategoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubCategoriaExists(subCategoria.SubId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Productos = await _context.Productos.ToListAsync();
            ViewBag.Categorias = await _context.Categorias.ToListAsync();

            return View(subCategoria);
        }

        // GET: SubCategoria/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategoria = await _subCategoriaService.GetSubCategoriaByIdAsync(id.Value);
            if (subCategoria == null)
            {
                return NotFound();
            }

            return View(subCategoria);
        }

        // POST: SubCategoria/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subCategoria = await _subCategoriaService.GetSubCategoriaByIdAsync(id);
            if (subCategoria == null)
            {
                return NotFound();
            }

            await _subCategoriaService.DeleteSubCategoriaAsync(id);

            return RedirectToAction(nameof(Index));
        }

        private bool SubCategoriaExists(int id)
        {
            return _context.SubCategorias.Any(e => e.SubId == id);
        }
    }
}
