using System;
using System.Collections.Generic;

namespace BotilleriaDB.Models;

public partial class Cliente
{
    public int ClienteId { get; set; }

    public string NombreCliente { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public virtual Venta? Venta { get; set; }
}
