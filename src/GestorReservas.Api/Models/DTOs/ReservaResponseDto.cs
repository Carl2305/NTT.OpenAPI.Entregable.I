using System;
using GestorReservas.Api.Models.Domain;

namespace GestorReservas.Api.Models.DTOs
{
    /// <summary>
    /// DTO de respuesta que contiene la información completa de una reserva.
    /// </summary>
    public class ReservaResponseDto
    {
        /// <summary>
        /// Identificador único de la reserva.
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Nombre de la sala reservada.
        /// </summary>
        /// <example>Sala A - Innovación</example>
        public string NombreSala { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de la reserva (YYYY-MM-DD).
        /// </summary>
        /// <example>2026-10-15</example>
        public DateOnly FechaReserva { get; set; }

        /// <summary>
        /// Hora de inicio de la reunión (HH:mm:ss).
        /// </summary>
        /// <example>09:00:00</example>
        public TimeSpan HoraInicio { get; set; }

        /// <summary>
        /// Hora de finalización de la reunión (HH:mm:ss).
        /// </summary>
        /// <example>10:30:00</example>
        public TimeSpan HoraFin { get; set; }

        /// <summary>
        /// Nombre de la persona responsable.
        /// </summary>
        /// <example>Carlos Mendoza</example>
        public string NombreResponsable { get; set; } = string.Empty;

        /// <summary>
        /// Cantidad de asistentes previstos.
        /// </summary>
        /// <example>8</example>
        public int CantidadAsistentes { get; set; }

        /// <summary>
        /// Motivo o agenda de la reunión.
        /// </summary>
        /// <example>Reunión de planificación trimestral</example>
        public string MotivoReunion { get; set; } = string.Empty;

        /// <summary>
        /// Estado actual de la reserva (Pendiente, Confirmada, Cancelada).
        /// </summary>
        /// <example>Confirmada</example>
        public EstadoReserva Estado { get; set; }
    }
}
