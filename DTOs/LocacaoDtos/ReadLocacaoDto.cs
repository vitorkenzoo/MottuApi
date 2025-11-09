using MottuApi.DTOs.Shared;

namespace MottuApi.DTOs.LocacaoDtos
{
    /// <summary>
    /// DTO para a leitura e retorno de dados de uma locação.
    /// Retorna um resumo com os dados principais da transação e dos envolvidos.
    /// </summary>
    public class ReadLocacaoDto
    {
        /// <summary>
        /// Identificador único da locação.
        /// </summary>
        /// <example>5001</example>
        public int Id { get; set; }

        /// <summary>
        /// Data e hora de início da locação.
        /// </summary>
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// Data e hora prevista para o término da locação.
        /// </summary>
        public DateTime DataFimPrevista { get; set; }

        /// <summary>
        /// Data e hora em que a moto foi devolvida. Será nulo se a locação ainda estiver ativa.
        /// </summary>
        public DateTime? DataFimReal { get; set; }

        /// <summary>
        /// Valor da diária da locação.
        /// </summary>
        /// <example>50.00</example>
        public decimal ValorDiaria { get; set; }

        /// <summary>
        /// Valor total pago. Será nulo até a devolução e cálculo final.
        /// </summary>
        /// <example>350.00</example>
        public decimal? ValorTotal { get; set; }

        /// <summary>
        /// Status atual da locação.
        /// </summary>
        /// <example>Ativa</example>
        public required string Status { get; set; }

        // --- Dados dos Relacionamentos ---

        /// <summary>
        /// ID do cliente que realizou a locação.
        /// </summary>
        /// <example>1</example>
        public int ClienteId { get; set; }

        /// <summary>
        /// Nome do cliente que realizou a locação.
        /// </summary>
        /// <example>Maria Souza</example>
        public required string NomeCliente { get; set; }

        /// <summary>
        /// ID da moto que foi alugada.
        /// </summary>
        /// <example>101</example>
        public int MotoId { get; set; }

        /// <summary>
        /// Placa da moto que foi alugada.
        /// </summary>
        /// <example>XYZ-1234</example>
        public required string PlacaMoto { get; set; }

        /// <summary>
        /// Lista de links HATEOAS relacionados à locação.
        /// </summary>
        public List<LinkDto> Links { get; set; } = new();
    }
}