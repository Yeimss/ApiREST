using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Custom;
using WebApi.Models.Context;
using WebApi.Models.DTOs.Usuario;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly dbContext _dbContext;
        private readonly Utils _utils;
        public AuthController(dbContext dbcontext, Utils utils)
        {
            _dbContext = dbcontext;
            _utils = utils;
        }
        [HttpPost]
        [Route("Registro")]
        public async Task<IActionResult> Registro(UsuarioDto user)
        {
            try
            {

                var modeloUsuario = new TblUsuario
                {
                    Name = user.name,
                    LastName = user.lastName,
                    NickName = user.nickName,
                    Email = user.email,
                    HashPassword = _utils.encriptarSHA256(user.password),
                    IdRol = 2
                };
                await _dbContext.TblUsuarios.AddAsync(modeloUsuario);
                await _dbContext.SaveChangesAsync();
                if(modeloUsuario.Id != 0)
                {
                    return Ok(new
                    {
                        message = "Registro correcto",
                        success = true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        message = "No se pudo realizar el registro",
                        success = false
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new {
                    message = "Ha ocurrido un error inesperado",
                    errorMessagee = ex.Message,
                    success = false
                });
            }
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            TblUsuario? usuario = await _dbContext.TblUsuarios
                .Where(user => 
                user.Email!.ToLower().Equals(login.email.ToLower()) 
                && user.HashPassword!.Equals(_utils.encriptarSHA256(login.password)))
                .FirstOrDefaultAsync();
            if (usuario == null)    
            {
                return Ok(new
                {
                    token = "",
                    success = false,
                    message = "Credenciales incorrectas"
                });
            }
            else
            {
                return Ok(new
                {
                    token = _utils.generarJWT(usuario),
                    success = true,
                    message = "Token generado correctamente"
                });
            }
        }
    }
}
