namespace WebApi.Models.DTOs.Usuario
{
    public class UsuarioDto : LoginDto
    {
        public string name { get; set; }
        public string lastName { get; set; }
        public string nickName { get; set; }
    }
}
