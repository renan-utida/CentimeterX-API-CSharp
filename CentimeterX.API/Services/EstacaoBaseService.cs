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

        // Calcula a distância euclidiana entre uma estação e as coordenadas informadas
        private double CalcularDistancia(EstacaoBase estacao, double latitude, double longitude)
        {
            return Math.Sqrt(
                Math.Pow(estacao.Latitude - latitude, 2) +
                Math.Pow(estacao.Longitude - longitude, 2));
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

                var resultado = new List<EstacaoBase>();
                var distancias = new Dictionary<int, double>();

                // Calcula distância euclidiana de cada estação em relação às coordenadas informadas
                foreach (var estacao in estacoes)
                {
                    distancias[estacao.IdEstacao] = CalcularDistancia(estacao, latitude, longitude);
                }

                // Ordena pelo dicionário de distâncias e monta a lista resultado
                resultado = estacoes
                    .OrderBy(e => distancias[e.IdEstacao])
                    .ToList();

                return resultado;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao listar estações por proximidade.", ex);
            }
        }
    }
}