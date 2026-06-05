using CentimeterX.API.Data;
using CentimeterX.API.Models;
using CentimeterX.API.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace CentimeterX.API.Services
{
    public class SessaoCorrecaoService : ICorrecaoService
    {
        private readonly AppDbContext _context;

        public SessaoCorrecaoService(AppDbContext context)
        {
            _context = context;
        }

        // Verifica se a sessão ainda está ativa (não foi encerrada)
        private bool SessaoEstaAtiva(SessaoCorrecao sessao)
        {
            return sessao.EncerradoEm == null;
        }

        public StatusFix ClassificarFix(double precisaoHorizontalCm)
        {
            switch (precisaoHorizontalCm)
            {
                case <= GnssConstants.FIX_THRESHOLD_CM:
                    return StatusFix.FIX;
                case <= GnssConstants.FLOAT_THRESHOLD_CM:
                    return StatusFix.FLOAT;
                default:
                    return StatusFix.SINGLE;
            }
        }

        public async Task<double> ObterPrecisao(int sessaoId)
        {
            try
            {
                var sessao = await _context.SessoesCorrecao.FindAsync(sessaoId);

                if (sessao == null)
                    throw new InvalidOperationException($"Sessão {sessaoId} não encontrada.");

                return sessao.PrecisaoHorizontalCm;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao obter precisão da sessão {sessaoId}.", ex);
            }
        }

        public async Task<SessaoCorrecao> IniciarSessao(int roverId, int estacaoBaseId, string sistemaSatelite)
        {
            try
            {
                // Verifica se o rover existe e está ativo
                var rover = await _context.Rovers.FindAsync(roverId);
                if (rover == null)
                    throw new InvalidOperationException("Rover não encontrado.");

                if (rover.Status != StatusRover.Ativo)
                    throw new InvalidOperationException($"Rover '{rover.Nome}' não está ativo. Status atual: {rover.Status}.");

                // Bloqueia nova sessão se já houver uma ativa para o mesmo rover
                var sessaoAtiva = await _context.SessoesCorrecao
                    .FirstOrDefaultAsync(s => s.IdRover == roverId && s.EncerradoEm == null);

                if (sessaoAtiva != null)
                    throw new InvalidOperationException($"Rover '{rover.Nome}' já possui uma sessão ativa (ID: {sessaoAtiva.IdSessao}). Encerre-a antes de iniciar nova sessão.");

                // Verifica se a estação base existe
                var estacao = await _context.EstacoesBase.FindAsync(estacaoBaseId);
                if (estacao == null)
                    throw new InvalidOperationException("Estação base não encontrada.");

                if (!estacao.Online)
                    throw new InvalidOperationException($"Estação base '{estacao.Nome}' está offline.");

                var sessao = new SessaoCorrecao
                {
                    IdRover = roverId,
                    IdEstacaoBase = estacaoBaseId,
                    SistemaSatelite = sistemaSatelite,
                    StatusFix = StatusFix.SINGLE,
                    PrecisaoHorizontalCm = 0,
                    PrecisaoVerticalCm = 0,
                    IniciouEm = DateTime.UtcNow
                };

                _context.SessoesCorrecao.Add(sessao);
                await _context.SaveChangesAsync();

                return sessao;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Erro ao salvar a sessão no banco de dados.", ex);
            }
        }

        public async Task<SessaoCorrecao> EncerrarSessao(int sessaoId)
        {
            try
            {
                var sessao = await _context.SessoesCorrecao.FindAsync(sessaoId);

                if (sessao == null)
                    throw new InvalidOperationException($"Sessão {sessaoId} não encontrada.");

                if (!SessaoEstaAtiva(sessao))
                    throw new InvalidOperationException($"Sessão {sessaoId} já foi encerrada em {sessao.EncerradoEm}.");

                sessao.EncerradoEm = DateTime.UtcNow;
                sessao.StatusFix = ClassificarFix(sessao.PrecisaoHorizontalCm);

                await _context.SaveChangesAsync();

                return sessao;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Erro ao encerrar a sessão no banco de dados.", ex);
            }
        }

        
    }
}