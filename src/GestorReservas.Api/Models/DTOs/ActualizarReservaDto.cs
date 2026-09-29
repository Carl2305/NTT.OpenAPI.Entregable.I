using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GestorReservas.Api.Models.Domain;

namespace GestorReservas.Api.Models.DTOs
{
    /// <summary>
    /// DTO para la actualización de una reserva existente.
    /// </summary>
    public class ActualizarReservaDto : IValidatableObject
    {
        /// <summary>
        /// Nombre de la sala reservada.
        /// </summary>
        /// <example>Sala B - Ejecutiva</example>
        [Required(ErrorMessage = "El nombre de la sala es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre de la sala debe tener entre 3 y 100 caracteres.")]
        public string NombreSala { get; set; } = string.Empty;

        /// <summary>
        /// Fecha programada para la reserva.
        /// </summary>
        /// <example>2026-10-20</example>
        [Required(ErrorMessage = "La fecha de reserva es obligatoria.")]
        public DateOnly FechaReserva { get; set; }

        /// <summary>
        /// Hora de inicio de la reunión.
        /// </summary>
        /// <example>14:00:00</example>
        [Required(ErrorMessage = "La hora de inicio es obligatoria.")]
        public TimeSpan HoraInicio { get; set; }

        /// <summary>
        /// Hora de fin de la reunión. Debe ser posterior a la hora de inicio.
        /// </summary>
        /// <example>16:00:00</example>
        [Required(ErrorMessage = "La hora de fin es obligatoria.")]
        public TimeSpan HoraFin { get; set; }

        /// <summary>
        /// Nombre de la persona responsable.
        /// </summary>
        /// <example>Ana María Torres</example>
        [Required(ErrorMessage = "El nombre del responsable es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre del responsable debe tener entre 3 y 100 caracteres.")]
        public string NombreResponsable { get; set; } = string.Empty;

        /// <summary>
        /// Cantidad de asistentes previstos.
        /// </summary>
        /// <example>10</example>
        [Required(ErrorMessage = "La cantidad de asistentes es obligatoria.")]
        [Range(1, 500, ErrorMessage = "La cantidad de asistentes debe ser mayor a 0 y como máximo 500.")]
        public int CantidadAsistentes { get; set; }

        /// <summary>
        /// Motivo o descripción de la reunión.
        /// </summary>
        /// <example>Reunión ampliada de presentación de avance de OpenAPI</example>
        [Required(ErrorMessage = "El motivo de la reunión es obligatorio.")]
        [StringLength(250, MinimumLength = 5, ErrorMessage = "El motivo debe tener entre 5 y 250 caracteres.")]
        public string MotivoReunion { get; set; } = string.Empty;

        /// <summary>
        /// Nuevo estado de la reserva (Pendiente, Confirmada, Cancelada).
        /// </summary>
        /// <example>Confirmada</example>
        [Required(ErrorMessage = "El estado de la reserva es obligatorio.")]
        public EstadoReserva Estado { get; set; }

        /// <summary>
        /// Validación de reglas de negocio para la actualización.
        /// </summary>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (HoraFin <= HoraInicio)
            {
                yield return new ValidationResult(
                    "La hora de fin debe ser posterior a la hora de inicio.",
                    new[] { nameof(HoraFin), nameof(HoraInicio) }
                );
            }

            if (CantidadAsistentes <= 0)
            {
                yield return new ValidationResult(
                    "La cantidad de asistentes debe ser mayor a cero.",
                    new[] { nameof(CantidadAsistentes) }
                );
            }
        }
    }
}
