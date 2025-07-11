using System;
using System.Collections.Generic;

namespace WebApi.Models.Context;

public partial class TblHabitacion
{
    public int Id { get; set; }

    public string? Habitacion { get; set; }

    public virtual ICollection<TblReserva> TblReservas { get; set; } = new List<TblReserva>();
}
