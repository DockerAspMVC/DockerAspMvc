using System.Threading.Tasks;
using DockerMvc.Interface;
using DockerMvc.Models;
using DockerMvc.ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DockerMvc.Controllers
{
    [Authorize(Policy = "Admin")]
    public class ProductosController : Controller
    {
        private readonly IProductoService _productoService;

        public ProductosController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _productoService.GetProductosAsync();
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
                    Imagen = await _productoService.SaveImageAsync(productos.Imagen),
                    Stock = productos.Stock
                };
                await _productoService.AddProductoAsync(newProduct);
                TempData["mensaje"] = "El producto se agregó correctamente";
                return RedirectToAction("Index");
            }

            return View(productos);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var producto = await _productoService.GetProductoByIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            var productoViewModel = new ProductosViewModel
            {
                ProduId = producto.ProduId,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Stock = producto.Stock,
                Imagen = null // Aquí puedes cargar una vista previa si lo deseas
            };

            return View(productoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductosViewModel productos)
        {
            if (ModelState.IsValid)
            {
                var producto = await _productoService.GetProductoByIdAsync(productos.ProduId);
                if (producto == null)
                {
                    return NotFound();
                }

                producto.Nombre = productos.Nombre;
                producto.Descripcion = productos.Descripcion;
                producto.Precio = productos.Precio;
                producto.Stock = productos.Stock;

                if (productos.Imagen != null)
                {
                    producto.Imagen = await _productoService.UpdateImageAsync(productos.Imagen, producto.Imagen);
                }

                await _productoService.UpdateProductoAsync(producto);
                TempData["mensaje"] = "El producto se actualizó correctamente";
                return RedirectToAction("Index");
            }
            return View(productos);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _productoService.GetProductoByIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _productoService.GetProductoByIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            await _productoService.DeleteProductoAsync(producto);
            TempData["mensaje"] = "El producto se eliminó correctamente";
            return RedirectToAction("Index");
        }
    }
}
