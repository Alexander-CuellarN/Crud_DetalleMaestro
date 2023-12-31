using System;
using System.Collections.Generic;

namespace Crud_DetalleMaestro.Models;

public partial class Pedido
{
    public int IdPedido { get; set; }

    public decimal? Total { get; set; }

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();
}
