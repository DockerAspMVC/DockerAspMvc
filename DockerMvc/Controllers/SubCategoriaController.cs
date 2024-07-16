using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DockerMvc.Data;
using DockerMvc.Interface;
using DockerMvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DockerMvc.Controllers
{
    public class SubCategoriaController : Controller
    {
        private readonly BaseDbContext _context;
        private readonly ISubCategoriaService _subCategoriaService;

        public SubCategoriaController(BaseDbContext context, ISubCategoriaService subCategoriaService)
        {
            _context = context;
            _subCategoriaService = subCategoriaService;
        }

        public async Task<IActionResult> Index()
        {
            var subCategorias = await _subCategoriaService.GetSubCategoriasAsync();
            return View(subCategorias);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categorias = new SelectList(await _context.Categorias.ToListAsync(), "CatId", "Name");
            ViewBag.Productos = await _context.Productos.Select(p => new SelectListItem
            {
                Value = p.ProduId.ToString(),
                Text = p.Nombre
            }).ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SubCategoria subCategoria, List<int> selectedProducts)
        {
            if (ModelState.IsValid)
            {
                subCategoria.SubCategoriaProductos = new List<SubCategoriaProducto>();

                foreach (var productId in selectedProducts)
                {
                    subCategoria.SubCategoriaProductos.Add(new SubCategoriaProducto
                    {
                        ProductoId = productId,
                        SubCategoriaSubId = subCategoria.SubId
                    });
                }

                await _subCategoriaService.CreateSubCategoriaAsync(subCategoria);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categorias = new SelectList(await _context.Categorias.ToListAsync(), "CatId", "Name", subCategoria.CatId);
            ViewBag.Productos = await _context.Productos.Select(p => new SelectListItem
            {
                Value = p.ProduId.ToString(),
                Text = p.Nombre
            }).ToListAsync();
            return View(subCategoria);
        }



    }
}
