using System.ComponentModel.DataAnnotations;

namespace MottuApi.DTOs.LocacaoDtos
{
    /// <summary>
    /// DTO para receber os dados necessários para a criação de uma nova locação.
    /// O cliente informa quem ele é e a data de devolução desejada. O sistema
    /// se encarrega de alocar uma moto disponível e registrar a transação.
    /// </summary>
    public class CreateLocacaoDto
    {
        /// <summary>
        /// ID do cliente que está realizando a locação.
        /// </summary>
        /// <example>1</example>
        [Required(ErrorMessage = "O ID do cliente é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O ID do cliente deve ser um número válido.")]
        public int ClienteId { get; set; }

        /// <summary>
        /// Data e hora prevista para o término da locação.
        /// Esta data deve ser futura em relação à data de início.
        /// </summary>
        /// <example>2025-10-01T10:00:00Z</example>
        [Required(ErrorMessage = "A data final prevista é obrigatória.")]
        public DateTime DataFimPrevista { get; set; }
    }
}