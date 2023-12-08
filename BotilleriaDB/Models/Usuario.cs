using System;
using System.Collections.Generic;

namespace BotilleriaDB.Models;

public partial class Usuario
{
    public int Iduser { get; set; }

    public string Email { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string? Nombre { get; set; }

    public virtual ICollection<DetalleVentum> DetalleVenta { get; set; } = new List<DetalleVentum>();
}
