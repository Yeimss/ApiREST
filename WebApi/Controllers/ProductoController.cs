using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly DbapiJwtContext _dbContext;
        public ProductoController(DbapiJwtContext dbapiJwtContext)
        {
            _dbContext = dbapiJwtContext;
        }
        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var lista = await _dbContext.Productos.ToListAsync();
            return Ok(new
            {
                success = true,
                message = lista.Any() ? "Información encontrada" : "No se ha encontrado información",
                data = lista
            });
        }
    }
}
