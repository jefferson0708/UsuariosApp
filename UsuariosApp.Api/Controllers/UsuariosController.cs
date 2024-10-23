using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsuariosApp.Api.DTO.Request;
using UsuariosApp.Api.DTO.Response;

using UsuariosApp.API.Services;

namespace UsuariosApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuariosController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("criar")]
        [ProducesResponseType(typeof(CriarUsuarioResponseDTO), 201)]
        public async Task<IActionResult> Criar([FromBody] CriarUsuarioRequestDTO dto)
        {
            try
            {
                return StatusCode(201, await _usuarioService.CriarAsync(dto));
            }
            catch (ApplicationException e)
            {
                return StatusCode(400, new { e.Message });
            }
        }

        [HttpPost("autenticar")]
        [ProducesResponseType(typeof(AutenticarUsuarioResponseDTO), 200)]
        public async Task<IActionResult> Autenticar([FromBody] AutenticarUsuarioRequestDTO dto)
        {
            try
            {
                return StatusCode(200, await _usuarioService.AutenticarAsync(dto));
            }
            catch (ApplicationException e)
            {
                return StatusCode(401, new { e.Message });
            }
        }


        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(AutenticarUsuarioResponseDTO), 200)]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            try
            {
                var response = await _usuarioService.RefreshTokenAsync(refreshToken);
                return Ok(response);
            }
            catch (ApplicationException e)
            {
                return StatusCode(400, new { e.Message });
            }
        }

    }
}



