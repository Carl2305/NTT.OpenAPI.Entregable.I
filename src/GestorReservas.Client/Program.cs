using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GestorReservas.Client
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("=================================================================");
            Console.WriteLine(" Demostración del Cliente .NET Generado desde Especificación OpenAPI");
            Console.WriteLine("=================================================================\n");

            var baseUrl = "http://localhost:5000";
            using var httpClient = new HttpClient();
            var client = new ReservasApiClient(baseUrl, httpClient);

            try
            {
                // 1. Autenticación para obtener Bearer Token
                Console.WriteLine("1. Autenticando mediante POST /api/Auth/login...");
                var loginRequest = new LoginRequestDto
                {
                    Username = "admin",
                    Password = "admin123"
                };

                var loginResult = await client.LoginAsync(loginRequest);
                Console.WriteLine($"   [ÉXITO] Token recibido: {loginResult.Token[..30]}...");
                Console.WriteLine($"   Tipo de Token: {loginResult.TokenType}, Expiración: {loginResult.ExpiresInMinutes} min\n");

                // Configurar encabezado Authorization con Bearer Token
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", loginResult.Token);

                // 2. Obtener todas las reservas
                Console.WriteLine("2. Consultando todas las reservas mediante GET /api/Reservas...");
                var reservas = await client.ReservasAllAsync(null);
                foreach (var r in reservas)
                {
                    Console.WriteLine($"   - Id {r.Id}: {r.NombreSala} | Fecha: {r.FechaReserva:yyyy-MM-dd} | Horario: {r.HoraInicio}-{r.HoraFin} | Responsable: {r.NombreResponsable} | Estado: {r.Estado}");
                }
                Console.WriteLine();

                // 3. Crear nueva reserva (Endpoint protegido)
                Console.WriteLine("3. Creando nueva reserva mediante POST /api/Reservas (Endpoint Protegido)...");
                var nuevaReservaDto = new CrearReservaDto
                {
                    NombreSala = "Sala C - Innovación Digital",
                    FechaReserva = DateTimeOffset.Now.AddDays(3),
                    HoraInicio = "10:00:00",
                    HoraFin = "11:30:00",
                    NombreResponsable = "Roberto Carlos",
                    CantidadAsistentes = 12,
                    MotivoReunion = "Demostración cliente .NET OpenAPI"
                };

                var nuevaReserva = await client.ReservasPOSTAsync(nuevaReservaDto);
                Console.WriteLine($"   [ÉXITO] Reserva creada con ID: {nuevaReserva.Id}");
                Console.WriteLine($"   Sala: {nuevaReserva.NombreSala}, Horario: {nuevaReserva.HoraInicio} - {nuevaReserva.HoraFin}\n");

                // 4. Obtener reserva recién creada por ID
                Console.WriteLine($"4. Consultando reserva recién creada por Id {nuevaReserva.Id}...");
                var reservaDetalle = await client.ReservasGETAsync(nuevaReserva.Id);
                Console.WriteLine($"   Detalle recuperado: {reservaDetalle.NombreSala} | Responsable: {reservaDetalle.NombreResponsable} | Estado: {reservaDetalle.Estado}\n");

                // 5. Actualizar reserva
                Console.WriteLine($"5. Actualizando reserva Id {nuevaReserva.Id} mediante PUT /api/Reservas/{nuevaReserva.Id}...");
                var actualizarDto = new ActualizarReservaDto
                {
                    NombreSala = "Sala C - Innovación Digital (Modificada)",
                    FechaReserva = nuevaReserva.FechaReserva,
                    HoraInicio = nuevaReserva.HoraInicio,
                    HoraFin = nuevaReserva.HoraFin,
                    NombreResponsable = nuevaReserva.NombreResponsable,
                    CantidadAsistentes = 15,
                    MotivoReunion = "Demostración cliente .NET OpenAPI (Actualizado)",
                    Estado = EstadoReserva.Confirmada
                };

                var reservaActualizada = await client.ReservasPUTAsync(nuevaReserva.Id, actualizarDto);
                Console.WriteLine($"   [ÉXITO] Estado actualizado a: {reservaActualizada.Estado}\n");

                // 6. Eliminar reserva
                Console.WriteLine($"6. Eliminando reserva Id {nuevaReserva.Id} mediante DELETE /api/Reservas/{nuevaReserva.Id}...");
                await client.ReservasDELETEAsync(nuevaReserva.Id);
                Console.WriteLine("   [ÉXITO] Reserva eliminada correctamente.\n");

                Console.WriteLine("=================================================================");
                Console.WriteLine(" Demostración completada exitosamente.");
                Console.WriteLine("=================================================================");
            }
            catch (ApiException ex)
            {
                Console.WriteLine($"[ERROR API] Código de estado: {ex.StatusCode}, Respuesta: {ex.Response}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR INESPERADO] {ex.Message}");
            }
        }
    }
}
