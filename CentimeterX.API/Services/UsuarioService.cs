using CentimeterX.API.Data;
using CentimeterX.API.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace CentimeterX.API.Services
{
    public class UsuarioService
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        // Normaliza o e-mail removendo espaços e convertendo para minúsculas
        private string NormalizarEmail(string email)
        {
            return email.Trim().ToLowerInvariant();
        }

        public async Task ValidarEmailDuplicado(string email, int? ignorarUsuarioId = null)
        {
            try
            {
                var emailNormalizado = NormalizarEmail(email);

                var existe = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == emailNormalizado
                               && (ignorarUsuarioId == null || u.IdUsuario != ignorarUsuarioId)) != null;

                if (existe)
                    throw new InvalidOperationException($"O e-mail '{email}' já está cadastrado.");
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao validar e-mail.", ex);
            }
        }

        public void ValidarAutoperfil(int usuarioLogadoId, int usuarioAlvoId, PerfilUsuario novoPerfil)
        {
            if (usuarioLogadoId == usuarioAlvoId && novoPerfil != PerfilUsuario.Administrador)
                throw new InvalidOperationException("Um administrador não pode rebaixar seu próprio perfil.");
        }

        public async Task<List<Models.Usuario>> ListarPorPerfil(PerfilUsuario perfil)
        {
            try
            {
                return await _context.Usuarios
                    .Where(u => u.Perfil == perfil)
                    .OrderBy(u => u.Nome)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao listar usuários por perfil.", ex);
            }
        }
    }
}