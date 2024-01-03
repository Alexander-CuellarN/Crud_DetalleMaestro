using Crud_DetalleMaestro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace Crud_DetalleMaestro.Controllers
{
    public class ProductoController : Controller
    {
        private readonly CrudMaestroDetalleContext _context;

        public ProductoController(CrudMaestroDetalleContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewData["ClassActived"] = "Producto";

            var prodcutos = _context.Productos.Include(p => p.CategoriaNavigation).ToList();
            return View(prodcutos);
        }

        public IActionResult Create()
        {
            ViewData["ClassActived"] = "Producto";
            ViewBag.Categorias = _context.Categoria.ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Producto producto)
        {
            ViewData["ClassActived"] = "Producto";
            ViewBag.Categorias = _context.Categoria.ToList();

            if (!ModelState.IsValid)
                return View(producto);

            _context.Productos.Add(producto);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Update(int id)
        {
            ViewData["ClassActived"] = "Producto";
            var producto = _context.Productos
                            .Include(p => p.CategoriaNavigation)
                            .FirstOrDefault(p => p.IdProducto == id);

            if (producto == null)
                return BadRequest();

            ViewBag.Categorias = _context.Categoria.ToList();
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, Producto producto)
        {
            var product = _context.Productos.Find(id);

            if (product == null)
                return BadRequest();

            if (product.IdProducto != id)
                return BadRequest();

            ViewBag.Categorias = _context.Categoria.ToList();

            if (!ModelState.IsValid)
                return View(producto);

            product.Nombre = producto.Nombre;
            product.Categoria = product.Categoria;
            product.Stock = producto.Stock;
            product.Precio = producto.Precio;

            _context.Productos.Update(product);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = _context.Productos.Find(id);
                _context.Productos.Remove(product);
                await _context.SaveChangesAsync();
                return Json(new { response = "Se elimino correctamente el producto" });

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GellAllProducts()
        {
            try
            {
                var productos = await _context.Productos.ToListAsync();
                return Json(new { message = "Se encontraro los Prductos", data = productos });
            }
            catch (Exception)
            {
                List<Producto> productos = new List<Producto>();
                return Json(new { message = "no se han encontraro los Prductos", data = productos });
            }
        }

    }
}
