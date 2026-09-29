using System.Text.Json.Serialization;

namespace GestorReservas.Api.Models.Domain
{
    /// <summary>
    /// Representa los posibles estados de una reserva de sala.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EstadoReserva
    {
        /// <summary>
        /// La reserva ha sido solicitada y está pendiente de confirmación.
        /// </summary>
        Pendiente,

        /// <summary>
        /// La reserva ha sido confirmada formalmente.
        /// </summary>
        Confirmada,

        /// <summary>
        /// La reserva ha sido cancelada.
        /// </summary>
        Cancelada
    }
}
