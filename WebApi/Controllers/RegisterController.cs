using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApi.Custom;
using WebApi.Models;
using WebApi.Models.Context;
using WebApi.Models.DTOs.Reserva;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly dbContext _dbContext;
        public RegisterController(dbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var lista = await _dbContext.TblReservas
                .Where(p => p.IdUser == userId)
                .Select(l => new
                {
                    id = l.Id,
                    idHabitacion = l.IdHabitacion,
                    idUser = l.IdUser,
                    fechaIngreso = l.FechaIngreso,
                    fechaSalida = l.FechaSalida
                })
                .ToListAsync();


            return Ok(new
            {
                success = true,
                message = lista.Any() ? "Información encontrada" : "No se ha encontrado información",
                data = lista
            });
        }
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(ReservaDto reserva)
        {
            try
            {
                var habitacionesReservadas = await _dbContext.TblReservas
                    .Where(r => 
                    (r.IdHabitacion == reserva.idHabitacion) 
                    && (
                        (r.FechaIngreso >= reserva.fechaIngreso && r.FechaIngreso <= reserva.fechaSalida) 
                        || (r.FechaSalida >= reserva.fechaIngreso && r.FechaIngreso <= reserva.fechaSalida)
                        ))
                    .ToListAsync();

                if (habitacionesReservadas.Any())
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "La habitación ya está reservada"
                    });
                }
                var modeloReserva = new TblReserva
                {
                    FechaIngreso = reserva.fechaIngreso,
                    FechaSalida = reserva.fechaSalida,
                    IdUser = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value),
                    IdHabitacion = reserva.idHabitacion
                };
                await _dbContext.TblReservas.AddAsync(modeloReserva);
                await _dbContext.SaveChangesAsync();
                if (modeloReserva.Id != 0)
                {
                    return Ok(new
                    {
                        message = "Registro correcto",
                        success = true
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        message = "No se pudo realizar el registro",
                        success = false
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Ha ocurrido un error inesperado",
                    errorMessagee = ex.Message,
                    success = false
                });
            }
        }
        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(ReservaDto reserva)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                var habitacionesReservadas = await _dbContext.TblReservas
                   .Where(r =>
                   (r.IdHabitacion == reserva.idHabitacion)
                   && (
                       (r.FechaIngreso >= reserva.fechaIngreso && r.FechaIngreso <= reserva.fechaSalida)
                       || (r.FechaSalida >= reserva.fechaIngreso && r.FechaIngreso <= reserva.fechaSalida)
                       ))
                   .ToListAsync();

                if (habitacionesReservadas.Any())
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "La habitación ya está reservada",
                        reservadaPorMi = habitacionesReservadas.Any(r => r.IdUser == userId),
                        reservadaPorOtroUsuario = habitacionesReservadas.Any(r => r.IdUser != userId)
                    });
                }

                var reservaAEditar = await _dbContext.TblReservas.FirstOrDefaultAsync (r => r.Id == reserva.id);
                if (reservaAEditar != null)
                {
                    reservaAEditar.FechaIngreso = reserva.fechaIngreso;
                    reservaAEditar.FechaSalida = reserva.fechaSalida;
                    reservaAEditar.IdUser = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                    reservaAEditar.IdHabitacion = reserva.idHabitacion;
                    await _dbContext.SaveChangesAsync();

                    return Ok(new
                    {
                        message = "Actualización realizada correctamente",
                        success = true
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        message = "No se encontró una reserva con ese identificador",
                        success = false
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Ha ocurrido un error inesperado",
                    errorMessagee = ex.Message,
                    success = false
                });
            }
        }
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(ReservaDto reserva)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                var reservaAEliminar = await _dbContext.TblReservas.FirstOrDefaultAsync(r => r.Id == reserva.id && r.IdUser == userId);
                if (reservaAEliminar != null)
                {
                    _dbContext.TblReservas.Remove(reservaAEliminar);
                    await _dbContext.SaveChangesAsync();

                    return Ok(new
                    {
                        message = "Reserva eliminada correctamente",
                        success = true
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        message = "No se encontró una reserva con ese identificador",
                        success = false
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Ha ocurrido un error inesperado",
                    errorMessagee = ex.Message,
                    success = false
                });
            }
        }

    }
}
