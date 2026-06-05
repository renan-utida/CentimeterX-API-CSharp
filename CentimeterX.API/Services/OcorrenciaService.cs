using CentimeterX.API.Data;
using CentimeterX.API.Models;
using CentimeterX.API.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace CentimeterX.API.Services
{
    public class OcorrenciaService
    {
        private readonly AppDbContext _context;

        public OcorrenciaService(AppDbContext context)
        {
            _context = context;
        }

        // Verifica se a latitude está dentro dos limites geográficos válidos
        private bool LatitudeValida(double latitude)
        {
            return latitude >= GnssConstants.LATITUDE_MIN && latitude <= GnssConstants.LATITUDE_MAX;
        }

        // Verifica se a longitude está dentro dos limites geográficos válidos
        private bool LongitudeValida(double longitude)
        {
            return longitude >= GnssConstants.LONGITUDE_MIN && longitude <= GnssConstants.LONGITUDE_MAX;
        }

        public void ValidarCoordenadas(double latitude, double longitude)
        {
            if (!LatitudeValida(latitude))
                throw new InvalidOperationException(
                    $"Latitude inválida: {latitude}. Deve estar entre {GnssConstants.LATITUDE_MIN} e {GnssConstants.LATITUDE_MAX}.");

            if (!LongitudeValida(longitude))
                throw new InvalidOperationException(
                    $"Longitude inválida: {longitude}. Deve estar entre {GnssConstants.LONGITUDE_MIN} e {GnssConstants.LONGITUDE_MAX}.");
        }

        public async Task ValidarPertencimentoRover(int roverId, int usuarioId)
        {
            try
            {
                var rover = await _context.Rovers
                    .FirstOrDefaultAsync(r => r.IdRover == roverId && r.IdUsuario == usuarioId);

                if (rover == null)
                    throw new InvalidOperationException("Rover não encontrado ou não pertence ao usuário informado.");
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao validar pertencimento do rover.", ex);
            }
        }

        public async Task<List<Ocorrencia>> ListarPorTipoEPeriodo(
            int roverId,
            TipoOcorrencia? tipo,
            DateTime? inicio,
            DateTime? fim)
        {
            try
            {
                var query = _context.Ocorrencias
                    .Where(o => o.IdRover == roverId);

                if (tipo.HasValue)
                    query = query.Where(o => o.Tipo == tipo.Value);

                if (inicio.HasValue)
                    query = query.Where(o => o.CriadaEm >= inicio.Value);

                if (fim.HasValue)
                    query = query.Where(o => o.CriadaEm <= fim.Value);

                return await query
                    .OrderByDescending(o => o.CriadaEm)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao listar ocorrências por tipo e período.", ex);
            }
        }
    }
}