namespace WebApi.Models.DTOs.Reserva
{
    public class ReservaDto
    {
        public DateOnly fechaIngreso { get; set; }
        public DateOnly fechaSalida { get; set; }
        public int idUsuario { get; set; }
        public int idHabitacion { get; set; }
    }
}
