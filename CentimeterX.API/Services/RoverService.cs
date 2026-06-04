using CentimeterX.API.Data;
using CentimeterX.API.Models;
using CentimeterX.API.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace CentimeterX.API.Services
{
    public class RoverService
    {
        private readonly AppDbContext _context;

        public RoverService(AppDbContext context)
        {
            _context = context;
        }

        public async Task ValidarEstacaoDisponivel(int estacaoBaseId)
        {
            try
            {
                var estacao = await _context.EstacoesBase.FindAsync(estacaoBaseId);

                if (estacao == null)
                    throw new InvalidOperationException("Estação base não encontrada.");

                if (!estacao.Online)
                    throw new InvalidOperationException($"Estação base '{estacao.Nome}' está offline e não pode ser associada a um rover.");
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao validar estação base.", ex);
            }
        }

        public async Task ValidarNomeDuplicado(string nome, int usuarioId, int? ignorarRoverId = null)
        {
            try
            {
                var existe = await _context.Rovers
                    .AnyAsync(r => r.Nome == nome
                                && r.IdUsuario == usuarioId
                                && (ignorarRoverId == null || r.IdRover != ignorarRoverId));

                if (existe)
                    throw new InvalidOperationException($"Já existe um rover com o nome '{nome}' cadastrado para este usuário.");
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao validar nome do rover.", ex);
            }
        }

        public async Task AtualizarStatusPorUltimaSessao(int roverId)
        {
            try
            {
                var rover = await _context.Rovers.FindAsync(roverId);
                if (rover == null) return;

                var ultimaSessao = await _context.SessoesCorrecao
                    .Where(s => s.IdRover == roverId)
                    .OrderByDescending(s => s.IniciouEm)
                    .FirstOrDefaultAsync();

                if (ultimaSessao == null) return;

                // Se a última sessão foi encerrada há mais de 24 horas, marca como Offline
                if (ultimaSessao.EncerradoEm != null &&
                    DateTime.UtcNow - ultimaSessao.EncerradoEm.Value > TimeSpan.FromHours(24))
                {
                    rover.Status = StatusRover.Offline;
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Erro ao atualizar status do rover.", ex);
            }
        }
    }
}