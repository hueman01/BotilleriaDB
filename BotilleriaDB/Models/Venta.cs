using System;
using System.Collections.Generic;

namespace BotilleriaDB.Models;

public partial class Venta
{
    public int VentaId { get; set; }

    public string? FechaVenta { get; set; }

    public string NombreCliente { get; set; } = null!;

    public int TotalVenta { get; set; }

    public virtual ICollection<DetalleVentum> DetalleVenta { get; set; } = new List<DetalleVentum>();

    public virtual Cliente? VentaNavigation { get; set; }
}
