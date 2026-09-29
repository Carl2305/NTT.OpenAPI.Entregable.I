using System;
using System.Collections.Generic;
using GestorReservas.Api.Models.Domain;
using GestorReservas.Api.Models.DTOs;

namespace GestorReservas.Api.Services
{
    /// <summary>
    /// Interfaz para la gestión del servicio de reservas de salas.
    /// </summary>
    public interface IReservaService
    {
        /// <summary>
        /// Obtiene todas las reservas, opcionalmente filtradas por fecha.
        /// </summary>
        IEnumerable<ReservaResponseDto> ObtenerTodas(DateOnly? fecha = null);

        /// <summary>
        /// Obtiene una reserva específica por su identificador único.
        /// </summary>
        ReservaResponseDto? ObtenerPorId(int id);

        /// <summary>
        /// Registra una nueva reserva en el sistema.
        /// </summary>
        ReservaResponseDto Crear(CrearReservaDto dto);

        /// <summary>
        /// Actualiza una reserva existente por su Id.
        /// </summary>
        ReservaResponseDto? Actualizar(int id, ActualizarReservaDto dto);

        /// <summary>
        /// Elimina o cancela una reserva existente por su Id.
        /// </summary>
        bool Eliminar(int id);
    }
}
