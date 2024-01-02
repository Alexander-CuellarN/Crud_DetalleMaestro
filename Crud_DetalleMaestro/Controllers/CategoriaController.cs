using Crud_DetalleMaestro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Crud_DetalleMaestro.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly CrudMaestroDetalleContext _context;
        public CategoriaController(CrudMaestroDetalleContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var categoriaList = await _context.Categoria.ToListAsync();
            ViewData["ClassActived"] = "Categoria";
            return View(categoriaList);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Categoria Categoria)
        {

            try
            {
                var categoria = new Categoria()
                {
                    Nombre = Categoria.Nombre
                };

                var newCategoria = _context.Categoria.Add(categoria);
                await _context.SaveChangesAsync();

                var categoriaViewModel = new
                {
                    idCategoria = newCategoria.Entity.IdCategria,
                    Nombre = newCategoria.Entity.Nombre
                };

                return Json(new { Response = "Se creo la categoria correctamente", categoriaViewModel });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListAll()
        {
            try
            {
                var categorias = await _context.Categoria.ToListAsync();
                return Json(new { Response = "No se ha podido crear el elemento", data = categorias });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoriaById(int id)
        {
            var categoria = await _context.Categoria.FindAsync(id);
            return Json(categoria);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Categoria categoria)
        {

            try
            {
                _context.Categoria.Update(categoria);
                await _context.SaveChangesAsync();

                return Json(new { Message = "Se ha modificado correctamente la categoria" });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var categoria = _context.Categoria.Find(id);
                _context.Categoria.Remove(categoria);
                await _context.SaveChangesAsync();

                return Json(new { Message = "Se elimino la categoria con exito" });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
