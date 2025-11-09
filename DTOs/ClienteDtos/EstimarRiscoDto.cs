using System.ComponentModel.DataAnnotations;

namespace MottuApi.DTOs.ClienteDtos
{
    /// <summary>
    /// DTO para estimar o risco de um cliente
    /// </summary>
    public class EstimarRiscoDto
    {
        /// <summary>
        /// Idade do cliente
        /// </summary>
        /// <example>25</example>
        [Required]
        [Range(18, 100, ErrorMessage = "A idade deve estar entre 18 e 100 anos.")]
        public int Idade { get; set; }

        /// <summary>
        /// Tipo da CNH (A, B, AB)
        /// </summary>
        /// <example>A</example>
        [Required]
        public required string TipoCNH { get; set; }
    }

    /// <summary>
    /// DTO de resposta com a estimativa de risco
    /// </summary>
    public class RiscoEstimadoDto
    {
        /// <summary>
        /// Nível de risco estimado (Alto ou Baixo)
        /// </summary>
        /// <example>Baixo</example>
        public string Risco { get; set; } = string.Empty;

        /// <summary>
        /// Idade do cliente analisado
        /// </summary>
        /// <example>25</example>
        public int Idade { get; set; }

        /// <summary>
        /// Tipo de CNH analisado
        /// </summary>
        /// <example>A</example>
        public string TipoCNH { get; set; } = string.Empty;
    }
}


