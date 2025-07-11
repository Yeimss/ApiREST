using System;
using System.Collections.Generic;

namespace WebApi.Models.Context;

public partial class TblUsuario
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string NickName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string HashPassword { get; set; } = null!;

    public string? Salt { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? Status { get; set; }

    public int? IdRol { get; set; }

    public virtual TblRol? IdRolNavigation { get; set; }

    public virtual ICollection<TblReserva> TblReservas { get; set; } = new List<TblReserva>();
}
