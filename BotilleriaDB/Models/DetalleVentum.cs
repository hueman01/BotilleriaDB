using System;
using System.Collections.Generic;

namespace BotilleriaDB.Models;

public partial class DetalleVentum
{
    public int DetalleVentaId { get; set; }

    public int VentaId { get; set; }

    public int ProductoId { get; set; }

    public int Cantidad { get; set; }

    public int PrecioUnitario { get; set; }

    public int TotalDetalle { get; set; }

    public virtual Producto? Producto { get; set; } 

    public virtual Usuario? Venta { get; set; } 

    public virtual Venta? VentaNavigation { get; set; }
}
