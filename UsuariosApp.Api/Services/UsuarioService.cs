using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UsuariosApp.Api.Contexts;
using UsuariosApp.Api.DTO.Request;
using UsuariosApp.Api.DTO.Response;
using UsuariosApp.Api.Entities;
using UsuariosApp.Api.Settings;

namespace UsuariosApp.API.Services
{
    public class UsuarioService
    {
        private readonly DataContext _dataContext;
        private readonly JwtSettings _jwtSettings;

        public UsuarioService(DataContext dataContext, JwtSettings jwtSettings)
        {
            _dataContext = dataContext;
            _jwtSettings = jwtSettings;
        }

        public async Task<CriarUsuarioResponseDTO> CriarAsync(CriarUsuarioRequestDTO dto)
        {
            if (_dataContext.Usuarios.Any(u => u.Email.Equals(dto.Email)))
                throw new ApplicationException("O email informado já está em uso.");

            var usuario = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = dto.Nome,
                Email = dto.Email,
                Senha = dto.Senha
            };

            await _dataContext.Usuarios.AddAsync(usuario);
            await _dataContext.SaveChangesAsync();

            return new CriarUsuarioResponseDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email
            };
        }

        public async Task<AutenticarUsuarioResponseDTO> AutenticarAsync(AutenticarUsuarioRequestDTO dto)
        {
            var usuario = await _dataContext.Usuarios.FirstOrDefaultAsync(u => u.Email.Equals(dto.Email) && u.Senha.Equals(dto.Senha));
            if (usuario == null)
                throw new ApplicationException("Usuário não encontrado.");

            return new AutenticarUsuarioResponseDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                AccessToken = GenerateToken(usuario.Email),
                RefreshToken = GenerateRefreshToken()
            };
        }

        private string GenerateToken(string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, email)
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings.AccessTokenExpiration)),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}




