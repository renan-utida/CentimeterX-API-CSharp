using CentimeterX.API.Data;
using CentimeterX.API.Models;
using CentimeterX.API.Models.Enums;
using CentimeterX.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CentimeterX.API.Controllers
{
    [Route("api/ocorrencias")]
    [ApiController]
    public class OcorrenciasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly OcorrenciaService _ocorrenciaService;
        private readonly ILogger<OcorrenciasController> _logger;

        public OcorrenciasController(
            AppDbContext context,
            OcorrenciaService ocorrenciaService,
            ILogger<OcorrenciasController> logger)
        {
            _context = context;
            _ocorrenciaService = ocorrenciaService;
            _logger = logger;
        }

        // GET: api/ocorrencias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ocorrencia>>> GetOcorrencias()
        {
            try
            {
                return await _context.Ocorrencias
                    .Include(o => o.Rover)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar ocorrências.");
                return StatusCode(500, new { mensagem = "Erro interno ao listar ocorrências." });
            }
        }

        // GET: api/ocorrencias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ocorrencia>> GetOcorrencia(int id)
        {
            try
            {
                var ocorrencia = await _context.Ocorrencias
                    .Include(o => o.Rover)
                    .FirstOrDefaultAsync(o => o.IdOcorrencia == id);

                if (ocorrencia == null)
                    return NotFound(new { mensagem = "Ocorrência não encontrada." });

                return ocorrencia;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar ocorrência ID {Id}.", id);
                return StatusCode(500, new { mensagem = "Erro interno ao buscar ocorrência." });
            }
        }

        // GET: api/ocorrencias/rover/3
        [HttpGet("rover/{roverId}")]
        public async Task<ActionResult<IEnumerable<Ocorrencia>>> GetOcorrenciasPorRover(int roverId)
        {
            try
            {
                var rover = await _context.Rovers.FindAsync(roverId);
                if (rover == null)
                    return NotFound(new { mensagem = "Rover não encontrado." });

                var ocorrencias = await _context.Ocorrencias
                    .Include(o => o.Rover)
                    .Where(o => o.IdRover == roverId)
                    .OrderByDescending(o => o.CriadaEm)
                    .ToListAsync();

                return Ok(ocorrencias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar ocorrências do rover ID {RoverId}.", roverId);
                return StatusCode(500, new { mensagem = "Erro interno ao listar ocorrências do rover." });
            }
        }

        // GET: api/ocorrencias/rover/3/filtro?tipo=PerdaDeSinal&inicio=2026-01-01&fim=2026-12-31
        [HttpGet("rover/{roverId}/filtro")]
        public async Task<ActionResult<IEnumerable<Ocorrencia>>> GetOcorrenciasPorTipoEPeriodo(
            int roverId,
            [FromQuery] TipoOcorrencia? tipo,
            [FromQuery] DateTime? inicio,
            [FromQuery] DateTime? fim)
        {
            try
            {
                var rover = await _context.Rovers.FindAsync(roverId);
                if (rover == null)
                    return NotFound(new { mensagem = "Rover não encontrado." });

                var ocorrencias = await _ocorrenciaService.ListarPorTipoEPeriodo(roverId, tipo, inicio, fim);
                return Ok(ocorrencias);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Erro de validação ao filtrar ocorrências do rover ID {RoverId}.", roverId);
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao filtrar ocorrências do rover ID {RoverId}.", roverId);
                return StatusCode(500, new { mensagem = "Erro interno ao filtrar ocorrências." });
            }
        }

        // POST: api/ocorrencias
        [HttpPost]
        public async Task<ActionResult<Ocorrencia>> PostOcorrencia(Ocorrencia ocorrencia)
        {
            try
            {
                // Verifica se o rover existe
                var rover = await _context.Rovers.FindAsync(ocorrencia.IdRover);
                if (rover == null)
                    return NotFound(new { mensagem = "Rover não encontrado." });

                // Valida coordenadas
                _ocorrenciaService.ValidarCoordenadas(ocorrencia.Latitude, ocorrencia.Longitude);

                // Valida se o rover pertence ao usuário dono do equipamento
                await _ocorrenciaService.ValidarPertencimentoRover(ocorrencia.IdRover, rover.IdUsuario);

                ocorrencia.CriadaEm = DateTime.UtcNow;

                _context.Ocorrencias.Add(ocorrencia);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetOcorrencia), new { id = ocorrencia.IdOcorrencia }, ocorrencia);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Erro de validação ao registrar ocorrência.");
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao salvar ocorrência no banco.");
                return StatusCode(500, new { mensagem = "Erro ao salvar ocorrência no banco de dados." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao registrar ocorrência.");
                return StatusCode(500, new { mensagem = "Erro interno ao registrar ocorrência." });
            }
        }

        // DELETE: api/ocorrencias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOcorrencia(int id)
        {
            try
            {
                var ocorrencia = await _context.Ocorrencias.FindAsync(id);

                if (ocorrencia == null)
                    return NotFound(new { mensagem = "Ocorrência não encontrada." });

                _context.Ocorrencias.Remove(ocorrencia);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao deletar ocorrência ID {Id}.", id);
                return StatusCode(500, new { mensagem = "Erro ao deletar ocorrência no banco de dados." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao deletar ocorrência ID {Id}.", id);
                return StatusCode(500, new { mensagem = "Erro interno ao deletar ocorrência." });
            }
        }
    }
}