using System;
using System.Collections.Generic;

namespace WebApi.Models.Context;

public partial class TblRol
{
    public int Id { get; set; }

    public string? Rol { get; set; }

    public virtual ICollection<TblUsuario> TblUsuarios { get; set; } = new List<TblUsuario>();
}
