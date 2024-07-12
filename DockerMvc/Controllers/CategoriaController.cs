using System.Threading.Tasks;
using DockerMvc.Interface;
using DockerMvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DockerMvc.Controllers
{
    [Authorize(Policy = "Admin")]
    public class CategoriaController : Controller
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        public async Task<IActionResult> Index()
        {
            var categorias = await _categoriaService.GetCategoriasAsync();
            return View(categorias);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                await _categoriaService.CreateCategoriaAsync(categoria);
                TempData["mensaje"] = "La categoría se agregó correctamente";
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var categoria = await _categoriaService.GetCategoriaByIdAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                await _categoriaService.UpdateCategoriaAsync(categoria);
                TempData["mensaje"] = "La categoría se actualizó correctamente";
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var categoria = await _categoriaService.GetCategoriaByIdAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoriaService.DeleteCategoriaAsync(id);
            TempData["mensaje"] = "La categoría se eliminó correctamente";
            return RedirectToAction(nameof(Index));
        }
    }
}
