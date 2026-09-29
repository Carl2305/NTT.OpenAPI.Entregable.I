using System;

namespace GestorReservas.Api.Models.Domain
{
    /// <summary>
    /// Modelo de dominio que representa una reserva de sala de reuniones.
    /// </summary>
    public class Reserva
    {
        /// <summary>
        /// Identificador único de la reserva.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre de la sala de reuniones.
        /// </summary>
        public string NombreSala { get; set; } = string.Empty;

        /// <summary>
        /// Fecha programada para la reserva.
        /// </summary>
        public DateOnly FechaReserva { get; set; }

        /// <summary>
        /// Hora de inicio de la reunión.
        /// </summary>
        public TimeSpan HoraInicio { get; set; }

        /// <summary>
        /// Hora de finalización de la reunión.
        /// </summary>
        public TimeSpan HoraFin { get; set; }

        /// <summary>
        /// Nombre de la persona responsable de la reserva.
        /// </summary>
        public string NombreResponsable { get; set; } = string.Empty;

        /// <summary>
        /// Cantidad de personas asistentes a la reunión.
        /// </summary>
        public int CantidadAsistentes { get; set; }

        /// <summary>
        /// Asunto o motivo de la reunión.
        /// </summary>
        public string MotivoReunion { get; set; } = string.Empty;

        /// <summary>
        /// Estado actual de la reserva (Pendiente, Confirmada, Cancelada).
        /// </summary>
        public EstadoReserva Estado { get; set; } = EstadoReserva.Pendiente;
    }
}
