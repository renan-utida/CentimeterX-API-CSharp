using CentimeterX.API.Data;
using CentimeterX.API.Models;
using CentimeterX.API.Models.Enums;
using CentimeterX.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CentimeterX.API.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UsuarioService _usuarioService;
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(
            AppDbContext context,
            UsuarioService usuarioService,
            ILogger<UsuariosController> logger)
        {
            _context = context;
            _usuarioService = usuarioService;
            _logger = logger;
        }

        // GET: api/usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            try
            {
                return await _context.Usuarios.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar usuários.");
                return StatusCode(500, new { mensagem = "Erro interno ao listar usuários." });
            }
        }

        // GET: api/usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);

                if (usuario == null)
                    return NotFound(new { mensagem = "Usuário não encontrado." });

                return usuario;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar usuário ID {Id}.", id);
                return StatusCode(500, new { mensagem = "Erro interno ao buscar usuário." });
            }
        }

        // GET: api/usuarios/perfil/Operador
        [HttpGet("perfil/{perfil}")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuariosPorPerfil(PerfilUsuario perfil)
        {
            try
            {
                var usuarios = await _usuarioService.ListarPorPerfil(perfil);
                return Ok(usuarios);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Erro de validação ao listar usuários por perfil.");
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar usuários por perfil {Perfil}.", perfil);
                return StatusCode(500, new { mensagem = "Erro interno ao listar usuários por perfil." });
            }
        }

        // POST: api/usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            try
            {
                await _usuarioService.ValidarEmailDuplicado(usuario.Email);

                usuario.CriadoEm = DateTime.UtcNow;

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUsuario), new { id = usuario.IdUsuario }, usuario);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Erro de validação ao cadastrar usuário.");
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao salvar usuário no banco.");
                return StatusCode(500, new { mensagem = "Erro ao salvar usuário no banco de dados." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao cadastrar usuário.");
                return StatusCode(500, new { mensagem = "Erro interno ao cadastrar usuário." });
            }
        }

        // PUT: api/usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            try
            {
                if (id != usuario.IdUsuario)
                    return BadRequest(new { mensagem = "ID da URL não confere com o ID do corpo." });

                var usuarioExistente = await _context.Usuarios.FindAsync(id);
                if (usuarioExistente == null)
                    return NotFound(new { mensagem = "Usuário não encontrado." });

                // Valida e-mail duplicado ignorando o próprio usuário
                await _usuarioService.ValidarEmailDuplicado(usuario.Email, id);

                // Protege contra administrador rebaixando o próprio perfil
                _usuarioService.ValidarAutoperfil(id, usuario.IdUsuario, usuario.Perfil);

                _context.Entry(usuarioExistente).CurrentValues.SetValues(usuario);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Erro de validação ao atualizar usuário ID {Id}.", id);
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao atualizar usuário ID {Id} no banco.", id);
                return StatusCode(500, new { mensagem = "Erro ao atualizar usuário no banco de dados." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao atualizar usuário ID {Id}.", id);
                return StatusCode(500, new { mensagem = "Erro interno ao atualizar usuário." });
            }
        }

        // DELETE: api/usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);

                if (usuario == null)
                    return NotFound(new { mensagem = "Usuário não encontrado." });

                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao deletar usuário ID {Id}.", id);
                return StatusCode(500, new { mensagem = "Erro ao deletar usuário. Verifique se existem rovers associados." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao deletar usuário ID {Id}.", id);
                return StatusCode(500, new { mensagem = "Erro interno ao deletar usuário." });
            }
        }
    }
}