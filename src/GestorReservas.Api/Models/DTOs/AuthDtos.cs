using System.ComponentModel.DataAnnotations;

namespace GestorReservas.Api.Models.DTOs
{
    /// <summary>
    /// Solicitud de autenticación para obtener un token JWT.
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>
        /// Nombre de usuario (ej. admin o usuario).
        /// </summary>
        /// <example>admin</example>
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Contraseña del usuario.
        /// </summary>
        /// <example>admin123</example>
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Respuesta de autenticación con el Bearer Token generado.
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// Token JWT Bearer generado.
        /// </summary>
        /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de token emitido.
        /// </summary>
        /// <example>Bearer</example>
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// Duración del token en minutos.
        /// </summary>
        /// <example>60</example>
        public int ExpiresInMinutes { get; set; } = 60;
    }
}
