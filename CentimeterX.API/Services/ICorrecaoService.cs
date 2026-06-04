using CentimeterX.API.Models;
using CentimeterX.API.Models.Enums;

namespace CentimeterX.API.Services
{
    public interface ICorrecaoService
    {
        // Inicia uma nova sessão de correção para um rover
        Task<SessaoCorrecao> IniciarSessao(int roverId, int estacaoBaseId, string sistemaSatelite);

        // Encerra uma sessão ativa registrando o horário de encerramento
        Task<SessaoCorrecao> EncerrarSessao(int sessaoId);

        // Retorna a precisão horizontal atual de uma sessão em centímetros
        Task<double> ObterPrecisao(int sessaoId);

        // Classifica o status de fix com base na precisão horizontal informada
        StatusFix ClassificarFix(double precisaoHorizontalCm);
    }
}