using CentimeterX.API.Data;
using CentimeterX.API.Models;
using CentimeterX.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CentimeterX.API.Controllers
{
    [Route("api/estacoes")]
    [ApiController]
    public class EstacoesBaseController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly EstacaoBaseService _estacaoBaseService;
        private readonly ILogger<EstacoesBaseController> _logger;

        public EstacoesBaseController(
            AppDbContext context,
            EstacaoBaseService estacaoBaseService,
            ILogger<EstacoesBaseController> logger)
        {
            _context = context;
            _estacaoBaseService = estacaoBaseService;
            _logger = logger;
        }

        // GET: api/estacoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstacaoBase>>> GetEstacoes()
        {
            try
            {
                return await _context.EstacoesBase.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar estações base.");
                return StatusCode(500, new { mensagem = "Erro interno ao listar estações base." });
            }
        }

        // GET: api/estacoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstacaoBase>> GetEstacao(int id)
        {
            try
            {
                var estacao = await _context.EstacoesBase.FindAsync(id);

                if (estacao == null)
                    return NotFound(new { mensagem = "Estação base não encontrada." });

                return estacao;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar estação base ID {Id}.", id);
                return StatusCode(500, new { mensagem = "Erro interno ao buscar estação base." });
            }
        }

        // GET: api/estacoes/proximidade?latitude=-23.5&longitude=-46.6
        [HttpGet("proximidade")]
        public async Task<ActionResult<IEnumerable<EstacaoBase>>> GetEstacoesPorProximidade(
            [FromQuery] double latitude,
            [FromQuery] double longitude)
        {
            try
            {
                var estacoes = await _estacaoBaseService.ListarPorProximidade(latitude, longitude);
                return Ok(estacoes);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Erro de validação ao listar estações por proximidade.");
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar estações por proximidade.");
                return StatusCode(500, new { mensagem = "Erro interno ao listar estações por proximidade." });
            }
        }

        // POST: api/estacoes
        [HttpPost]
        public async Task<ActionResult<EstacaoBase>> PostEstacao(EstacaoBase estacao)
        {
            try
            {
                estacao.UltimaAtualizacao = DateTime.UtcNow;

                // Valida coordenadas
                if (estacao.Latitude < GnssConstants.LATITUDE_MIN || estacao.Latitude > GnssConstants.LATITUDE_MAX)
                    return BadRequest(new { mensagem = $"Latitude inválida: {estacao.Latitude}. Deve estar entre {GnssConstants.LATITUDE_MIN} e {GnssConstants.LATITUDE_MAX}." });

                if (estacao.Longitude < GnssConstants.LONGITUDE_MIN || estacao.Longitude > GnssConstants.LONGITUDE_MAX)
                    return BadRequest(new { mensagem = $"Longitude inválida: {estacao.Longitude}. Deve estar entre {GnssConstants.LONGITUDE_MIN} e {GnssConstants.LONGITUDE_MAX}." });

                _context.EstacoesBase.Add(estacao);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetEstacao), new { id = estacao.IdEstacao }, estacao);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao salvar estação base no banco.");
                return StatusCode(500, new { mensagem = "Erro ao salvar estação base no banco de dados." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao cadastrar estação base.");
                return StatusCode(500, new { mensagem = "Erro interno ao cadastrar estação base." });
            }
        }

        // PUT: api/estacoes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstacao(int id, EstacaoBase estacao)
        {
            try
            {
                if (id != estacao.IdEstacao)
                    return BadRequest(new { mensagem = "ID da URL não confere com o ID do corpo." });

                // Valida coordenadas
                if (estacao.Latitude < GnssConstants.LATITUDE_MIN || estacao.Latitude > GnssConstants.LATITUDE_MAX)
                    return BadRequest(new { mensagem = $"Latitude inválida: {estacao.Latitude}. Deve estar entre {GnssConstants.LATITUDE_MIN} e {GnssConstants.LATITUDE_MAX}." });

                if (estacao.Longitude < GnssConstants.LONGITUDE_MIN || estacao.Longitude > GnssConstants.LONGITUDE_MAX)
                    return BadRequest(new { mensagem = $"Longitude inválida: {estacao.Longitude}. Deve estar entre {GnssConstants.LONGITUDE_MIN} e {GnssConstants.LONGITUDE_MAX}." });

                estacao.UltimaAtualizacao = DateTime.UtcNow;

                var estacaoExistente = await _context.EstacoesBase.FindAsync(id);
                if (estacaoExistente == null)
                    return NotFound(new { mensagem = "Estação base não encontrada." });

                _context.Entry(estacaoExistente).CurrentValues.SetValues(estacao);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstacaoExists(id))
                    return NotFound(new { mensagem = "Estação base não encontrada." });
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao atualizar estação base ID {Id}.", id);
                return StatusCode(500, new { mensagem = "Erro ao atualizar estação base no banco de dados." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar estação base ID {Id}.", id);
                return StatusCode(500, new { mensagem = "Erro interno ao atualizar estação base." });
            }
        }

        // DELETE: api/estacoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstacao(int id)
        {
            try
            {
                var estacao = await _context.EstacoesBase.FindAsync(id);

                if (estacao == null)
                    return NotFound(new { mensagem = "Estação base não encontrada." });

                _context.EstacoesBase.Remove(estacao);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao deletar estação base ID {Id}.", id);
                return StatusCode(500, new { mensagem = "Erro ao deletar estação base. Verifique se existem rovers associados." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao deletar estação base ID {Id}.", id);
                return StatusCode(500, new { mensagem = "Erro interno ao deletar estação base." });
            }
        }

        private bool EstacaoExists(int id)
        {
            return _context.EstacoesBase.Any(e => e.IdEstacao == id);
        }
    }
}