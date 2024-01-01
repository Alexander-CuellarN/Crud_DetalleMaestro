using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Crud_DetalleMaestro.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    [Required(ErrorMessage ="El campo nombre es obligatorio")]
    public string? Nombre { get; set; }

    [Required(ErrorMessage = "El campo Stock es obligatorio")]
    public int? Stock { get; set; }

    [Required]
    [RegularExpression("(^[0-9]+)", ErrorMessage ="Selecione una categoria Valida") ]
    public int? Categoria { get; set; }

    [Required]
    public decimal? Precio { get; set; }
    public DateTime? FechaCreacion { get; set; }

    public virtual Categoria? CategoriaNavigation { get; set; }

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();
}
