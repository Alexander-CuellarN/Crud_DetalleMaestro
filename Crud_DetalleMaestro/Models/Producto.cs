using System;
using System.Collections.Generic;

namespace Crud_DetalleMaestro.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string? Nombre { get; set; }

    public int? Stock { get; set; }

    public int? Categoria { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual Categoria? CategoriaNavigation { get; set; }

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();
}
