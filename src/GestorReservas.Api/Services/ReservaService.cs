using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GestorReservas.Api.Models.Domain;
using GestorReservas.Api.Models.DTOs;

namespace GestorReservas.Api.Services
{
    /// <summary>
    /// Implementación del servicio de reservas en memoria.
    /// </summary>
    public class ReservaService : IReservaService
    {
        private readonly ConcurrentDictionary<int, Reserva> _reservas = new();
        private int _nextId = 1;
        private readonly object _lock = new();

        public ReservaService()
        {
            SeedData();
        }

        private void SeedData()
        {
            var seedList = new List<Reserva>
            {
                new Reserva
                {
                    Id = _nextId++,
                    NombreSala = "Sala A - Innovación",
                    FechaReserva = DateOnly.FromDateTime(DateTime.Today),
                    HoraInicio = new TimeSpan(9, 0, 0),
                    HoraFin = new TimeSpan(10, 30, 0),
                    NombreResponsable = "Carlos Mendoza",
                    CantidadAsistentes = 8,
                    MotivoReunion = "Reunión de planificación trimestral de Arquitectura",
                    Estado = EstadoReserva.Confirmada
                },
                new Reserva
                {
                    Id = _nextId++,
                    NombreSala = "Sala B - Ejecutiva",
                    FechaReserva = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                    HoraInicio = new TimeSpan(11, 0, 0),
                    HoraFin = new TimeSpan(12, 0, 0),
                    NombreResponsable = "Ana María Torres",
                    CantidadAsistentes = 4,
                    MotivoReunion = "Entrevista de Selección de Personal",
                    Estado = EstadoReserva.Pendiente
                },
                new Reserva
                {
                    Id = _nextId++,
                    NombreSala = "Auditorio Principal",
                    FechaReserva = DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
                    HoraInicio = new TimeSpan(15, 0, 0),
                    HoraFin = new TimeSpan(17, 0, 0),
                    NombreResponsable = "Luis Fernando Gómez",
                    CantidadAsistentes = 50,
                    MotivoReunion = "Capacitación Técnica sobre OpenAPI y ASP.NET Core",
                    Estado = EstadoReserva.Confirmada
                }
            };

            foreach (var reserva in seedList)
            {
                _reservas[reserva.Id] = reserva;
            }
        }

        public IEnumerable<ReservaResponseDto> ObtenerTodas(DateOnly? fecha = null)
        {
            var query = _reservas.Values.AsEnumerable();

            if (fecha.HasValue)
            {
                query = query.Where(r => r.FechaReserva == fecha.Value);
            }

            return query.OrderBy(r => r.FechaReserva)
                        .ThenBy(r => r.HoraInicio)
                        .Select(MapToDto);
        }

        public ReservaResponseDto? ObtenerPorId(int id)
        {
            if (_reservas.TryGetValue(id, out var reserva))
            {
                return MapToDto(reserva);
            }
            return null;
        }

        public ReservaResponseDto Crear(CrearReservaDto dto)
        {
            int newId;
            lock (_lock)
            {
                newId = _nextId++;
            }

            var reserva = new Reserva
            {
                Id = newId,
                NombreSala = dto.NombreSala.Trim(),
                FechaReserva = dto.FechaReserva,
                HoraInicio = dto.HoraInicio,
                HoraFin = dto.HoraFin,
                NombreResponsable = dto.NombreResponsable.Trim(),
                CantidadAsistentes = dto.CantidadAsistentes,
                MotivoReunion = dto.MotivoReunion.Trim(),
                Estado = EstadoReserva.Pendiente
            };

            _reservas[newId] = reserva;
            return MapToDto(reserva);
        }

        public ReservaResponseDto? Actualizar(int id, ActualizarReservaDto dto)
        {
            if (!_reservas.ContainsKey(id))
            {
                return null;
            }

            var reservaActualizada = new Reserva
            {
                Id = id,
                NombreSala = dto.NombreSala.Trim(),
                FechaReserva = dto.FechaReserva,
                HoraInicio = dto.HoraInicio,
                HoraFin = dto.HoraFin,
                NombreResponsable = dto.NombreResponsable.Trim(),
                CantidadAsistentes = dto.CantidadAsistentes,
                MotivoReunion = dto.MotivoReunion.Trim(),
                Estado = dto.Estado
            };

            _reservas[id] = reservaActualizada;
            return MapToDto(reservaActualizada);
        }

        public bool Eliminar(int id)
        {
            return _reservas.TryRemove(id, out _);
        }

        private static ReservaResponseDto MapToDto(Reserva reserva)
        {
            return new ReservaResponseDto
            {
                Id = reserva.Id,
                NombreSala = reserva.NombreSala,
                FechaReserva = reserva.FechaReserva,
                HoraInicio = reserva.HoraInicio,
                HoraFin = reserva.HoraFin,
                NombreResponsable = reserva.NombreResponsable,
                CantidadAsistentes = reserva.CantidadAsistentes,
                MotivoReunion = reserva.MotivoReunion,
                Estado = reserva.Estado
            };
        }
    }
}
