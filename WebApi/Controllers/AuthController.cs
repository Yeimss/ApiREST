using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Custom;
using WebApi.Models;
using WebApi.Models.DTOs.Usuario;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DbapiJwtContext _dbContext;
        private readonly Utils _utils;
        public AuthController(DbapiJwtContext dbcontext, Utils utils)
        {
            _dbContext = dbcontext;
            _utils = utils;
        }
        [HttpPost]
        [Route("Registro")]
        public async Task<IActionResult> Registro(UsuarioDto user)
        {
            var modeloUsuario = new TblUsuario
            {
                Nombre = user.Nombre,
                Correo = user.Correo,
                Clave = _utils.encriptarSHA256(user.Clave),
            };
            await _dbContext.TblUsuarios.AddAsync(modeloUsuario);
            await _dbContext.SaveChangesAsync();
            if(modeloUsuario.IdUsuario != 0)
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
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            TblUsuario? usuario = await _dbContext.TblUsuarios
                .Where(user => 
                user.Correo!.ToLower().Equals(login.Correo.ToLower()) 
                && user.Clave!.Equals(_utils.encriptarSHA256(login.Clave)))
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
