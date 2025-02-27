using Microsoft.AspNetCore.Mvc;
using ProjetoPlanta_Backend.Models;
using ProjetoPlanta_Backend.Data;
using Google.Cloud.Firestore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ProjetoPlanta_Backend.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly FirestoreService _service;
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _service = new FirestoreService();
            _configuration = configuration;
        }

        [HttpPost("account/login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginViewModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.email) || string.IsNullOrWhiteSpace(model.senha))
                {
                    return BadRequest(new { message = "Email e senha são obrigatórios" });
                }

                var administradores = await _service.getAllDocsAsync<UsuarioLoginViewModel>("Administradores");
                var admin = administradores.FirstOrDefault(u => u.email == model.email);

                if (admin == null || admin.senha != model.senha)
                {
                    return Unauthorized(new { message = "Usuário ou senha inválidos" });
                }

                var token = GerarToken(admin);

                return Ok(new { message = "Login realizado com sucesso", token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno no servidor", detalhes = ex.Message });
            }
        }

        private string GerarToken(UsuarioLoginViewModel usuario)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, usuario.email)
            };

            var credenciais = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credenciais
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
