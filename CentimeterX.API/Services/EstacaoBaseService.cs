using CentimeterX.API.Data;
using CentimeterX.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CentimeterX.API.Services
{
    public class EstacaoBaseService
    {
        private readonly AppDbContext _context;

        public EstacaoBaseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task ValidarDisponibilidade(int estacaoId)
        {
            try
            {
                var estacao = await _context.EstacoesBase.FindAsync(estacaoId);

                if (estacao == null)
                    throw new InvalidOperationException("Estação base não encontrada.");

                if (!estacao.Online)
                    throw new InvalidOperationException($"Estação base '{estacao.Nome}' está offline.");
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao validar disponibilidade da estação.", ex);
            }
        }

        public async Task<List<EstacaoBase>> ListarPorProximidade(double latitude, double longitude)
        {
            try
            {
                var estacoes = await _context.EstacoesBase.ToListAsync();

                // Ordena por distância euclidiana simples em relação às coordenadas informadas
                var ordenadas = estacoes
                    .OrderBy(e => Math.Sqrt(
                        Math.Pow(e.Latitude - latitude, 2) +
                        Math.Pow(e.Longitude - longitude, 2)))
                    .ToList();

                return ordenadas;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao listar estações por proximidade.", ex);
            }
        }
    }
}