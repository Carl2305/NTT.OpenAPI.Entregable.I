using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GestorReservas.Api.Models.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GestorReservas.Api.Services
{
    /// <summary>
    /// Implementación del servicio de autenticación JWT.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public LoginResponseDto? Authenticate(LoginRequestDto request)
        {
            // Para fines demostrativos aceptamos credenciales "admin" / "admin123" o cualquier usuario no vacío
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return null;
            }

            var secretKey = _configuration["Jwt:SecretKey"] ?? "SuperSecretKeyForOpenApiDemoDoc2026RequirementPassphrase!";
            var issuer = _configuration["Jwt:Issuer"] ?? "GestorReservasApi";
            var audience = _configuration["Jwt:Audience"] ?? "GestorReservasClients";

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, request.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, request.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var expirationMinutes = 60;
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtString = tokenHandler.WriteToken(token);

            return new LoginResponseDto
            {
                Token = jwtString,
                TokenType = "Bearer",
                ExpiresInMinutes = expirationMinutes
            };
        }
    }
}
