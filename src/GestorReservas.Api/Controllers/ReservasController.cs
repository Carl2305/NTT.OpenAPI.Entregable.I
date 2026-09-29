using System;
using System.Collections.Generic;
using GestorReservas.Api.Models.DTOs;
using GestorReservas.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestorReservas.Api.Controllers
{
    /// <summary>
    /// API REST para la administración de reservas de salas de reuniones.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ReservasController : ControllerBase
    {
        private readonly IReservaService _reservaService;

        public ReservasController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        /// <summary>
        /// Obtiene el listado completo de reservas registradas, con opción de filtrado por fecha.
        /// </summary>
        /// <param name="fecha">Fecha opcional para filtrar las reservas (formato YYYY-MM-DD).</param>
        /// <returns>Lista de reservas encontradas.</returns>
        /// <response code="200">Retorna el listado de reservas solicitadas.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ReservaResponseDto>), StatusCodes.Status200OK)]
        public IActionResult ObtenerTodas([FromQuery] DateOnly? fecha)
        {
            var result = _reservaService.ObtenerTodas(fecha);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el detalle de una reserva específica mediante su identificador único (Id).
        /// </summary>
        /// <param name="id">Identificador numérico de la reserva.</param>
        /// <returns>Detalle de la reserva solicitada.</returns>
        /// <response code="200">Retorna la reserva encontrada.</response>
        /// <response code="404">Si la reserva solicitada no existe.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ReservaResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ObtenerPorId([FromRoute] int id)
        {
            var reserva = _reservaService.ObtenerPorId(id);
            if (reserva == null)
            {
                return NotFound(new { mensaje = $"No se encontró la reserva con Id {id}." });
            }
            return Ok(reserva);
        }

        /// <summary>
        /// Registra una nueva reserva de sala de reuniones.
        /// </summary>
        /// <remarks>
        /// Este endpoint requiere autenticación mediante Bearer Token/JWT.
        /// 
        /// **Validaciones aplicadas:**
        /// - La hora de fin debe ser posterior a la hora de inicio.
        /// - La cantidad de asistentes debe ser mayor a cero.
        /// - Todos los campos obligatorios deben estar presentes.
        /// </remarks>
        /// <param name="dto">Objeto con la información necesaria para crear la reserva.</param>
        /// <returns>La reserva creada con su Id asignado.</returns>
        /// <response code="201">Reserva creada con éxito.</response>
        /// <response code="400">Si los datos proporcionados violan las reglas de validación.</response>
        /// <response code="401">Si no se proporciona un Bearer Token válido.</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ReservaResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Crear([FromBody] CrearReservaDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reservaCreada = _reservaService.Crear(dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = reservaCreada.Id }, reservaCreada);
        }

        /// <summary>
        /// Actualiza la información de una reserva existente.
        /// </summary>
        /// <remarks>
        /// Este endpoint requiere autenticación mediante Bearer Token/JWT.
        /// </remarks>
        /// <param name="id">Identificador numérico de la reserva a actualizar.</param>
        /// <param name="dto">Datos actualizados de la reserva.</param>
        /// <returns>La reserva con sus datos actualizados.</returns>
        /// <response code="200">Reserva actualizada correctamente.</response>
        /// <response code="400">Si la información enviada es inválida o no cumple las reglas de negocio.</response>
        /// <response code="401">Si el cliente no está autenticado con Bearer Token.</response>
        /// <response code="404">Si no existe la reserva con el Id especificado.</response>
        [HttpPut("{id:int}")]
        [Authorize]
        [ProducesResponseType(typeof(ReservaResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Actualizar([FromRoute] int id, [FromBody] ActualizarReservaDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reservaActualizada = _reservaService.Actualizar(id, dto);
            if (reservaActualizada == null)
            {
                return NotFound(new { mensaje = $"No se encontró la reserva con Id {id} para actualizar." });
            }

            return Ok(reservaActualizada);
        }

        /// <summary>
        /// Cancela o elimina una reserva existente según su Id.
        /// </summary>
        /// <remarks>
        /// Este endpoint requiere autenticación mediante Bearer Token/JWT.
        /// </remarks>
        /// <param name="id">Identificador de la reserva a eliminar.</param>
        /// <returns>Confirmación de la eliminación.</returns>
        /// <response code="200">Si la reserva fue eliminada correctamente.</response>
        /// <response code="401">Si el cliente no posee un Bearer Token JWT válido.</response>
        /// <response code="404">Si no se encontró la reserva con el Id proporcionado.</response>
        [HttpDelete("{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Eliminar([FromRoute] int id)
        {
            var eliminada = _reservaService.Eliminar(id);
            if (!eliminada)
            {
                return NotFound(new { mensaje = $"No se pudo eliminar. La reserva con Id {id} no existe." });
            }

            return Ok(new { mensaje = $"La reserva con Id {id} fue eliminada exitosamente." });
        }
    }
}
