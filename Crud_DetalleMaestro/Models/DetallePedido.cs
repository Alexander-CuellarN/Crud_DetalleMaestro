using System;
using System.Collections.Generic;

namespace Crud_DetalleMaestro.Models;

public partial class DetallePedido
{
    public int IdDetallePedido { get; set; }

    public int? IdPedido { get; set; }

    public int? IdProducto { get; set; }

    public decimal? Total { get; set; }

    public int? Cantidad { get; set; }

    public virtual Pedido? IdPedidoNavigation { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }
}
