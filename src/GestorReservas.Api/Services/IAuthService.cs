using GestorReservas.Api.Models.DTOs;

namespace GestorReservas.Api.Services
{
    /// <summary>
    /// Interfaz para autenticación y generación de JWT Bearer Tokens.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Autentica las credenciales de usuario y retorna un token JWT válido.
        /// </summary>
        LoginResponseDto? Authenticate(LoginRequestDto request);
    }
}
