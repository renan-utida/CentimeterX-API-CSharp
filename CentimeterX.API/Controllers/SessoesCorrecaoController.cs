using CentimeterX.API.Data;
using CentimeterX.API.Models;
using CentimeterX.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CentimeterX.API.Controllers
{
    [Route("api/sessoes")]
    [ApiController]
    public class SessoesCorrecaoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICorrecaoService _correcaoService;
        private readonly ILogger<SessoesCorrecaoController> _logger;

        public SessoesCorrecaoController(
            AppDbContext context,
            ICorrecaoService correcaoService,
            ILogger<SessoesCorrecaoController> logger)
        {
            _context = context;
            _correcaoService = correcaoService;
            _logger = logger;
        }

        // GET: api/sessoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SessaoCorrecao>>> GetSessoes()
        {
            try
            {
                return await _context.SessoesCorrecao
                    .Include(s => s.Rover)
                    .Include(s => s.EstacaoBase)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar sessões de correção.");
                return StatusCode(500, new { mensagem = "Erro interno ao listar sessões de correção." });
            }
        }

        // GET: api/sessoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SessaoCorrecao>> GetSessao(int id)
        {
            try
            {
                var sessao = await _context.SessoesCorrecao
                    .Include(s => s.Rover)
                    .Include(s => s.EstacaoBase)
                    .FirstOrDefaultAsync(s => s.IdSessao == id);

                if (sessao == null)
                    return NotFound(new { mensagem = "Sessão de correção não encontrada." });

                return sessao;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar sessão ID {Id}.", id);
                return StatusCode(500, new { mensagem = "Erro interno ao buscar sessão de correção." });
            }
        }

        // GET: api/sessoes/rover/3
        [HttpGet("rover/{roverId}")]
        public async Task<ActionResult<IEnumerable<SessaoCorrecao>>> GetSessoesPorRover(int roverId)
        {
            try
            {
                var rover = await _context.Rovers.FindAsync(roverId);
                if (rover == null)
                    return NotFound(new { mensagem = "Rover não encontrado." });

                var sessoes = await _context.SessoesCorrecao
                    .Include(s => s.EstacaoBase)
                    .Where(s => s.IdRover == roverId)
                    .OrderByDescending(s => s.IniciouEm)
                    .ToListAsync();

                return Ok(sessoes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar sessões do rover ID {RoverId}.", roverId);
                return StatusCode(500, new { mensagem = "Erro interno ao listar sessões do rover." });
            }
        }

        // POST: api/sessoes
        [HttpPost]
        public async Task<ActionResult<SessaoCorrecao>> PostSessao(SessaoCorrecao sessao)
        {
            try
            {
                var sessaoCriada = await _correcaoService.IniciarSessao(
                    sessao.IdRover,
                    sessao.IdEstacaoBase,
                    sessao.SistemaSatelite);

                return CreatedAtAction(nameof(GetSessao),
                    new { id = sessaoCriada.IdSessao }, sessaoCriada);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Erro de validação ao iniciar sessão de correção.");
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao salvar sessão de correção no banco.");
                return StatusCode(500, new { mensagem = "Erro ao salvar sessão de correção no banco de dados." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao iniciar sessão de correção.");
                return StatusCode(500, new { mensagem = "Erro interno ao iniciar sessão de correção." });
            }
        }

        // PUT: api/sessoes/5/encerrar
        [HttpPut("{id}/encerrar")]
        public async Task<ActionResult<SessaoCorrecao>> EncerrarSessao(int id)
        {
            try
            {
                var sessaoEncerrada = await _correcaoService.EncerrarSessao(id);

                return Ok(new
                {
                    mensagem = "Sessão encerrada com sucesso.",
                    sessao = sessaoEncerrada,
                    statusFix = sessaoEncerrada.StatusFix.ToString(),
                    precisaoHorizontalCm = sessaoEncerrada.PrecisaoHorizontalCm,
                    encerradoEm = sessaoEncerrada.EncerradoEm
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Erro de validação ao encerrar sessão ID {Id}.", id);
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao encerrar sessão ID {Id} no banco.", id);
                return StatusCode(500, new { mensagem = "Erro ao encerrar sessão no banco de dados." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao encerrar sessão ID {Id}.", id);
                return StatusCode(500, new { mensagem = "Erro interno ao encerrar sessão de correção." });
            }
        }

        // PUT: api/sessoes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSessao(int id, SessaoCorrecao sessao)
        {
            try
            {
                if (id != sessao.IdSessao)
                    return BadRequest(new { mensagem = "ID da URL não confere com o ID do corpo." });

                var sessaoExistente = await _context.SessoesCorrecao.FindAsync(id);
                if (sessaoExistente == null)
                    return NotFound(new { mensagem = "Sessão de correção não encontrada." });

                // Reclassifica o fix ao atualizar a precisão
                sessao.StatusFix = _correcaoService.ClassificarFix(sessao.PrecisaoHorizontalCm);

                _context.Entry(sessaoExistente).CurrentValues.SetValues(sessao);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Erro de validação ao atualizar sessão ID {Id}.", id);
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao atualizar sessão ID {Id} no banco.", id);
                return StatusCode(500, new { mensagem = "Erro ao atualizar sessão no banco de dados." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar sessão ID {Id}.", id);
                return StatusCode(500, new { mensagem = "Erro interno ao atualizar sessão de correção." });
            }
        }

        // DELETE: api/sessoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSessao(int id)
        {
            try
            {
                var sessao = await _context.SessoesCorrecao.FindAsync(id);

                if (sessao == null)
                    return NotFound(new { mensagem = "Sessão de correção não encontrada." });

                _context.SessoesCorrecao.Remove(sessao);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao deletar sessão ID {Id}.", id);
                return StatusCode(500, new { mensagem = "Erro ao deletar sessão. Verifique dependências existentes." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao deletar sessão ID {Id}.", id);
                return StatusCode(500, new { mensagem = "Erro interno ao deletar sessão de correção." });
            }
        }
    }
}