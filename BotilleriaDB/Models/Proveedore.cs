using System;
using System.Collections.Generic;

namespace BotilleriaDB.Models;

public partial class Proveedore
{
    public int ProveedorId { get; set; }

    public string NombreProveedor { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public virtual Producto? Proveedor { get; set; } 
}
