using CentimeterX.API.Data;
using CentimeterX.API.Models;
using CentimeterX.API.Models.Enums;
using CentimeterX.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CentimeterX.API.Controllers
{
    [Route("api/rovers")]
    [ApiController]
    public class RoversController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly RoverService _roverService;
        private readonly ILogger<RoversController> _logger;

        public RoversController(
            AppDbContext context,
            RoverService roverService,
            ILogger<RoversController> logger)
        {
            _context = context;
            _roverService = roverService;
            _logger = logger;
        }

        // GET: api/rovers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rover>>> GetRovers()
        {
            try
            {
                return await _context.Rovers
                    .Include(r => r.EstacaoBase)
                    .Include(r => r.Usuario)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar rovers.");
                return StatusCode(500, new { mensagem = "Erro interno ao listar rovers." });
            }
        }

        // GET: api/rovers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rover>> GetRover(int id)
        {
            try
            {
                var rover = await _context.Rovers
                    .Include(r => r.EstacaoBase)
                    .Include(r => r.Usuario)
                    .FirstOrDefaultAsync(r => r.IdRover == id);

                if (rover == null)
                    return NotFound(new { mensagem = "Rover não encontrado." });

                // Atualiza status com base na última sessão antes de retornar
                await _roverService.AtualizarStatusPorUltimaSessao(id);

                return rover;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar rover ID {Id}.", id);
                return StatusCode(500, new { mensagem = "Erro interno ao buscar rover." });
            }
        }

        // POST: api/rovers/maquina-agricola
        [HttpPost("maquina-agricola")]
        public async Task<ActionResult<MaquinaAgricola>> PostMaquinaAgricola(MaquinaAgricola maquina)
        {
            try
            {
                await _roverService.ValidarEstacaoDisponivel(maquina.IdEstacaoBase);
                await _roverService.ValidarNomeDuplicado(maquina.Nome, maquina.IdUsuario);

                maquina.DataCadastro = DateTime.UtcNow;

                _context.MaquinasAgricolas.Add(maquina);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetRover),
                    new { id = maquina.IdRover }, maquina);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Erro de validação ao cadastrar máquina agrícola.");
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao salvar máquina agrícola no banco.");
                return StatusCode(500, new { mensagem = "Erro ao salvar máquina agrícola no banco de dados." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao cadastrar máquina agrícola.");
                return StatusCode(500, new { mensagem = "Erro interno ao cadastrar máquina agrícola." });
            }
        }

        // POST: api/rovers/drone
        [HttpPost("drone")]
        public async Task<ActionResult<Drone>> PostDrone(Drone drone)
        {
            try
            {
                await _roverService.ValidarEstacaoDisponivel(drone.IdEstacaoBase);
                await _roverService.ValidarNomeDuplicado(drone.Nome, drone.IdUsuario);

                drone.DataCadastro = DateTime.UtcNow;

                _context.Drones.Add(drone);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetRover),
                    new { id = drone.IdRover }, drone);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Erro de validação ao cadastrar drone.");
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao salvar drone no banco.");
                return StatusCode(500, new { mensagem = "Erro ao salvar drone no banco de dados." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao cadastrar drone.");
                return StatusCode(500, new { mensagem = "Erro interno ao cadastrar drone." });
            }
        }

        // POST: api/rovers/veiculo-autonomo
        [HttpPost("veiculo-autonomo")]
        public async Task<ActionResult<VeiculoAutonomo>> PostVeiculoAutonomo(VeiculoAutonomo veiculo)
        {
            try
            {
                await _roverService.ValidarEstacaoDisponivel(veiculo.IdEstacaoBase);
                await _roverService.ValidarNomeDuplicado(veiculo.Nome, veiculo.IdUsuario);

                veiculo.DataCadastro = DateTime.UtcNow;

                _context.VeiculosAutonomos.Add(veiculo);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetRover),
                    new { id = veiculo.IdRover }, veiculo);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Erro de validação ao cadastrar veículo autônomo.");
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao salvar veículo autônomo no banco.");
                return StatusCode(500, new { mensagem = "Erro ao salvar veículo autônomo no banco de dados." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao cadastrar veículo autônomo.");
                return StatusCode(500, new { mensagem = "Erro interno ao cadastrar veículo autônomo." });
            }
        }

        // PUT: api/rovers/maquina-agricola/5
        [HttpPut("maquina-agricola/{id}")]
        public async Task<IActionResult> PutMaquinaAgricola(int id, MaquinaAgricola maquina)
        {
            try
            {
                if (id != maquina.IdRover)
                    return BadRequest(new { mensagem = "ID da URL não confere com o ID do corpo." });

                var existente = await _context.MaquinasAgricolas.FindAsync(id);
                if (existente == null)
                    return NotFound(new { mensagem = "Máquina agrícola não encontrada." });

                await _roverService.ValidarEstacaoDisponivel(maquina.IdEstacaoBase);
                await _roverService.ValidarNomeDuplicado(maquina.Nome, maquina.IdUsuario, id);

                _context.Entry(existente).CurrentValues.SetValues(maquina);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Erro de validação ao atualizar máquina agrícola ID {Id}.", id);
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao atualizar máquina agrícola ID {Id} no banco.", id);
                return StatusCode(500, new { mensagem = "Erro ao atualizar máquina agrícola no banco de dados." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar máquina agrícola ID {Id}.", id);
                return StatusCode(500, new { mensagem = "Erro interno ao atualizar máquina agrícola." });
            }
        }

        // PUT: api/rovers/drone/5
        [HttpPut("drone/{id}")]
        public async Task<IActionResult> PutDrone(int id, Drone drone)
        {
            try
            {
                if (id != drone.IdRover)
                    return BadRequest(new { mensagem = "ID da URL não confere com o ID do corpo." });

                var existente = await _context.Drones.FindAsync(id);
                if (existente == null)
                    return NotFound(new { mensagem = "Drone não encontrado." });

                await _roverService.ValidarEstacaoDisponivel(drone.IdEstacaoBase);
                await _roverService.ValidarNomeDuplicado(drone.Nome, drone.IdUsuario, id);

                _context.Entry(existente).CurrentValues.SetValues(drone);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Erro de validação ao atualizar drone ID {Id}.", id);
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao atualizar drone ID {Id} no banco.", id);
                return StatusCode(500, new { mensagem = "Erro ao atualizar drone no banco de dados." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar drone ID {Id}.", id);
                return StatusCode(500, new { mensagem = "Erro interno ao atualizar drone." });
            }
        }

        // PUT: api/rovers/veiculo-autonomo/5
        [HttpPut("veiculo-autonomo/{id}")]
        public async Task<IActionResult> PutVeiculoAutonomo(int id, VeiculoAutonomo veiculo)
        {
            try
            {
                if (id != veiculo.IdRover)
                    return BadRequest(new { mensagem = "ID da URL não confere com o ID do corpo." });

                var existente = await _context.VeiculosAutonomos.FindAsync(id);
                if (existente == null)
                    return NotFound(new { mensagem = "Veículo autônomo não encontrado." });

                await _roverService.ValidarEstacaoDisponivel(veiculo.IdEstacaoBase);
                await _roverService.ValidarNomeDuplicado(veiculo.Nome, veiculo.IdUsuario, id);

                _context.Entry(existente).CurrentValues.SetValues(veiculo);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Erro de validação ao atualizar veículo autônomo ID {Id}.", id);
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao atualizar veículo autônomo ID {Id} no banco.", id);
                return StatusCode(500, new { mensagem = "Erro ao atualizar veículo autônomo no banco de dados." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar veículo autônomo ID {Id}.", id);
                return StatusCode(500, new { mensagem = "Erro interno ao atualizar veículo autônomo." });
            }
        }

        // DELETE: api/rovers/maquina-agricola/5
        [HttpDelete("maquina-agricola/{id}")]
        public async Task<IActionResult> DeleteMaquinaAgricola(int id)
        {
            try
            {
                var maquina = await _context.MaquinasAgricolas.FindAsync(id);

                if (maquina == null)
                    return NotFound(new { mensagem = "Máquina agrícola não encontrada." });

                _context.MaquinasAgricolas.Remove(maquina);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao deletar máquina agrícola ID {Id}.", id);
                return StatusCode(500, new { mensagem = "Erro ao deletar máquina agrícola. Verifique se existem sessões ou ocorrências associadas." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao deletar máquina agrícola ID {Id}.", id);
                return StatusCode(500, new { mensagem = "Erro interno ao deletar máquina agrícola." });
            }
        }

        // DELETE: api/rovers/drone/5
        [HttpDelete("drone/{id}")]
        public async Task<IActionResult> DeleteDrone(int id)
        {
            try
            {
                var drone = await _context.Drones.FindAsync(id);

                if (drone == null)
                    return NotFound(new { mensagem = "Drone não encontrado." });

                _context.Drones.Remove(drone);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao deletar drone ID {Id}.", id);
                return StatusCode(500, new { mensagem = "Erro ao deletar drone. Verifique se existem sessões ou ocorrências associadas." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao deletar drone ID {Id}.", id);
                return StatusCode(500, new { mensagem = "Erro interno ao deletar drone." });
            }
        }

        // DELETE: api/rovers/veiculo-autonomo/5
        [HttpDelete("veiculo-autonomo/{id}")]
        public async Task<IActionResult> DeleteVeiculoAutonomo(int id)
        {
            try
            {
                var veiculo = await _context.VeiculosAutonomos.FindAsync(id);

                if (veiculo == null)
                    return NotFound(new { mensagem = "Veículo autônomo não encontrado." });

                _context.VeiculosAutonomos.Remove(veiculo);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao deletar veículo autônomo ID {Id}.", id);
                return StatusCode(500, new { mensagem = "Erro ao deletar veículo autônomo. Verifique se existem sessões ou ocorrências associadas." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao deletar veículo autônomo ID {Id}.", id);
                return StatusCode(500, new { mensagem = "Erro interno ao deletar veículo autônomo." });
            }
        }
    }
}