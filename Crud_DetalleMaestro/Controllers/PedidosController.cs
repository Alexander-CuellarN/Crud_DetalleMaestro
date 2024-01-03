using Crud_DetalleMaestro.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Runtime.Intrinsics.Arm;

namespace Crud_DetalleMaestro.Controllers
{
    public class PedidosController : Controller
    {
        private readonly CrudMaestroDetalleContext _context;

        public PedidosController(CrudMaestroDetalleContext context)
        {
            _context = context;
        }

        // GET: PedidosController
        public ActionResult Index()
        {

            ViewData["ClassActived"] = "Pedidos";

            var allPedidos = _context.Pedidos
                            .ToList();

            return View(allPedidos);
        }

        // GET: PedidosController/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                var productosList = _context.DetallePedidos
                                    .Where(p => p.IdPedido == id)
                                    .Include(p => p.IdProductoNavigation)
                                    .Select(p => new
                                    {
                                        Producto = p.IdProductoNavigation.Nombre,
                                        price = p.IdProductoNavigation.Precio,
                                        quantity = p.Cantidad,
                                        total = p.Total
                                    })
                                    .ToList();

                return Ok(new { message = "", data = productosList });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Se ha producido un error listando los productos",
                    data = new { }
                });
            }
        }

        // GET: PedidosController/Create
        public ActionResult Create()
        {
            ViewData["ClassActived"] = "Pedidos";
            return View();
        }

        public struct DataPedido
        {
            public Pedido pedido { get; set; }
            public int[] productos { get; set; }
            public int[] productoQuantity { get; set; }
        }

        // POST: PedidosController/Create
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] DataPedido data)
        {
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var pedido = data.pedido;
                        var productos = data.productos;
                        var productoQuantity = data.productoQuantity;

                        var newPedido = _context.Pedidos.Add(pedido);
                        await _context.SaveChangesAsync();

                        for (int i = 0; i < productoQuantity.Length; i++)
                        {
                            var producto = _context.Productos.Find(productos[i]) ?? throw new Exception(message: "Uno o mas Productos de la lista no son reconocibles");

                            if (producto.Stock < productoQuantity[i])
                            {
                                throw new Exception(message: "Uno o mas Productos de la lista Superan el stock disponible");
                            }

                            producto.Stock -= productoQuantity[i];
                            _context.Productos.Update(producto);
                            await _context.SaveChangesAsync();

                            DetallePedido detallePedido = new DetallePedido()
                            {
                                Cantidad = productoQuantity[i],
                                IdPedido = newPedido.Entity.IdPedido,
                                IdProducto = productos[i],
                                Total = producto.Precio * productoQuantity[i],
                            };
                            _context.DetallePedidos.Add(detallePedido);
                            await _context.SaveChangesAsync();
                        }

                        transaction.Commit();
                        return Ok(new { message = "El pedido se ha creado exitosamente" });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return BadRequest(new { message = ex.Message });
                    }
                }
            }
            catch
            {
                return BadRequest(new { message = "ha ocurrido un problema gurdando el pedido" });
            }
        }
        // GET: PedidosController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewData["ClassActived"] = "Pedidos";

            var pedido = _context.Pedidos
                         .Include(p => p.DetallePedidos)
                         .ThenInclude(dp => dp.IdProductoNavigation)
                         .Where(p => p.IdPedido == id)
                         .FirstOrDefault();

            var productos = _context.Productos.ToList();
            ViewBag.Productos = productos;
            return View(pedido);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, [FromBody] DataPedido data)
        {
            try
            {
                using (var transation = _context.Database.BeginTransaction())
                {

                    try
                    {
                        var pedido = data.pedido;
                        var products = data.productos;
                        var productoQuantity = data.productoQuantity;

                        _context.Pedidos.Update(pedido);
                        await _context.SaveChangesAsync();

                        _context.DetallePedidos
                                .RemoveRange(_context.DetallePedidos
                                                      .Where(p => p.IdPedido == pedido.IdPedido)
                                                      .AsEnumerable()
                                                      .ToList());
                        await _context.SaveChangesAsync();

                        for (int i = 0; i < productoQuantity.Length; i++)
                        {
                            var producto = _context.Productos.Find(products[i]) ?? throw new Exception(message: "Uno o mas Productos de la lista no son reconocibles");

                            if (producto.Stock < productoQuantity[i])
                            {
                                throw new Exception(message: "Uno o mas Productos de la lista Superan el stock disponible");
                            }

                            producto.Stock -= productoQuantity[i];
                            _context.Productos.Update(producto);
                            await _context.SaveChangesAsync();

                            DetallePedido detallePedido = new()
                            {
                                Cantidad = productoQuantity[i],
                                IdPedido = pedido.IdPedido,
                                IdProducto = products[i],
                                Total = producto.Precio * productoQuantity[i],
                            };
                            _context.DetallePedidos.Add(detallePedido);
                            await _context.SaveChangesAsync();
                        }

                        transation.Commit();
                        return Ok(new
                        {
                            message = "El pedido se ha creado exitosamente",
                            data = new { }
                        });

                    }
                    catch (Exception ex)
                    {
                        await transation.RollbackAsync();
                        return BadRequest(new
                        {
                            message = ex.Message,
                            data = new { }
                        });
                    }
                }
            }
            catch
            {
                return BadRequest(new
                {
                    message = "ha ocurrido un error en la modificacion del pedido",
                    data = new { }
                });
            }
        }

        // POST: PedidosController/Delete/5
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var pedidoFounded = _context.Pedidos.Find(id) ?? throw new Exception("No se ha encontrado el pedido a eliminar");
                    
                    var detailsPedido = _context.DetallePedidos.Where(p => p.IdPedido ==  id).AsEnumerable().ToList();
                    _context.DetallePedidos.RemoveRange(detailsPedido);
                    _context.Pedidos.Remove(pedidoFounded);
                    await _context.SaveChangesAsync();

                    transaction.Commit();
                    return Ok(new { message = "El pedido se ha eliminado con exito", data = new { } });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(new { message = ex.Message, data = new { } });
                }
            }
        }
    }
}
