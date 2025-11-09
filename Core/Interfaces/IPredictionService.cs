namespace MottuApi.Core.Interfaces
{
    /// <summary>
    /// Interface para o serviço de predição de risco de clientes usando ML.NET
    /// </summary>
    public interface IPredictionService
    {
        /// <summary>
        /// Estima o risco de um cliente baseado na idade e tipo de CNH
        /// </summary>
        /// <param name="idade">Idade do cliente</param>
        /// <param name="tipoCNH">Tipo da CNH (A, B, AB)</param>
        /// <returns>Retorna "Alto" ou "Baixo" indicando o nível de risco</returns>
        string EstimarRisco(int idade, string tipoCNH);
    }
}


