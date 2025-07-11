using System;
using System.Collections.Generic;

namespace WebApi.Models.Context;

public partial class TblReserva
{
    public int Id { get; set; }

    public DateOnly? FechaIngreso { get; set; }

    public DateOnly? FechaSalida { get; set; }

    public int? IdUser { get; set; }

    public int? IdHabitacion { get; set; }

    public virtual TblHabitacion? IdHabitacionNavigation { get; set; }

    public virtual TblUsuario? IdUserNavigation { get; set; }
}
