using GestorReservas.Api.Models.DTOs;
using GestorReservas.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestorReservas.Api.Controllers
{
    /// <summary>
    /// Controlador para la autenticación y emisión de tokens JWT Bearer.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Genera un token de acceso JWT Bearer para autorizar peticiones protegidas.
        /// </summary>
        /// <param name="request">Credenciales de acceso del usuario (ej. admin / admin123).</param>
        /// <returns>Objeto con el token JWT de acceso.</returns>
        /// <response code="200">Retorna el token JWT generado exitosamente.</response>
        /// <response code="400">Si los datos de la solicitud de login son inválidos.</response>
        /// <response code="401">Si las credenciales ingresadas son incorrectas.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _authService.Authenticate(request);
            if (result == null)
            {
                return Unauthorized(new { mensaje = "Credenciales inválidas. Ingrese un usuario y contraseña válidos." });
            }

            return Ok(result);
        }
    }
}
