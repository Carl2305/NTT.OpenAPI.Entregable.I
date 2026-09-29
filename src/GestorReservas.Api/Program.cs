using System;
using System.IO;
using System.Reflection;
using System.Text;
using GestorReservas.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios de la aplicación
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Registro de servicios propios de la aplicación
builder.Services.AddSingleton<IReservaService, ReservaService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Configuración de Autenticación JWT Bearer
var secretKey = builder.Configuration["Jwt:SecretKey"] ?? "SuperSecretKeyForOpenApiDemoDoc2026RequirementPassphrase!";
var issuer = builder.Configuration["Jwt:Issuer"] ?? "GestorReservasApi";
var audience = builder.Configuration["Jwt:Audience"] ?? "GestorReservasClients";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// Configuración de OpenAPI / Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API de Gestión de Reservas de Salas",
        Version = "v1",
        Description = "API REST desarrollada en ASP.NET Core para administrar las reservas de salas de reuniones de una organización. Permite consultar, registrar, actualizar y cancelar reservas con documentación OpenAPI y seguridad JWT Bearer.",
        Contact = new OpenApiContact
        {
            Name = "Equipo de Desarrollo NTT / Capacitación OpenAPI",
            Email = "contacto@organizacion.com"
        }
    });

    // Carga de comentarios XML para enriquecer la documentación
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }

    // Configuración del esquema de seguridad JWT en OpenAPI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Esquema de autenticación mediante JWT Bearer Token.\n\nEjemplo: Obtenga un token mediante el endpoint `POST /api/auth/login` e ingréselo a continuación en la opción Authorize (con o sin el prefijo 'Bearer ').",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configurar el pipeline HTTP
if (app.Environment.IsDevelopment() || true) // Habilitar Swagger siempre para evaluación del entregable
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Gestión de Reservas de Salas v1");
        c.RoutePrefix = "swagger"; // Accesible en http://localhost:5000/swagger
        c.DocumentTitle = "Documentación API - Gestión de Reservas de Salas";
    });
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
