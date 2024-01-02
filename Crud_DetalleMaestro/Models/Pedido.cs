using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Crud_DetalleMaestro.Models;

public partial class Pedido
{
    public int IdPedido { get; set; }

    [Required(ErrorMessage = "El Total de la compra es requerido")]
    public decimal? Total { get; set; }

    [Required(ErrorMessage = "El Nit del comprador es requerido")]
    public string Nit { get; set; }

    [Required(ErrorMessage = "El nombre del comprador es requerido")]
    public string Nombre { get; set; }
    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();
}
