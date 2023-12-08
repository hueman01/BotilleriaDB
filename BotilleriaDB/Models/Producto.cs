using System;
using System.Collections.Generic;

namespace BotilleriaDB.Models;

public partial class Producto
{
    public int ProductoId { get; set; }

    public string NombreProducto { get; set; } = null!;

    public int CategoriaId { get; set; }

    public string NombreProveedor { get; set; } = null!;

    public int Precio { get; set; }

    public int Stock { get; set; }

    public string? Imagen { get; set; }

    public virtual Categoria Categoria { get; set; } = null!;

    public virtual DetalleVentum? DetalleVentum { get; set; }
}
