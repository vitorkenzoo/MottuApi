using System.ComponentModel.DataAnnotations;

namespace MottuApi.DTOs.MotoDtos
{
    /// <summary>
    /// DTO para receber os dados necessários para a atualização de uma moto.
    /// Este é o objeto esperado no corpo (body) de requisições PUT.
    /// </summary>
    public class UpdateMotoDto
    {
        /// <summary>
        /// Ano de fabricação da moto.
        /// </summary>
        /// <example>2025</example>
        [Required(ErrorMessage = "O ano da moto é obrigatório.")]
        [Range(1990, 2026, ErrorMessage = "O ano de fabricação deve ser válido.")]
        public int Ano { get; set; }

        /// <summary>
        /// Modelo da moto.
        /// </summary>
        /// <example>Honda CG 160 Titan</example>
        [Required(ErrorMessage = "O modelo da moto é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O modelo deve ter entre 3 e 100 caracteres.")]
        public required string Modelo { get; set; }

        /// <summary>
        /// Status atual da moto. Valores válidos: "Disponivel", "Alugada", "EmManutencao".
        /// A lógica de negócio no serviço validará as transições de status permitidas.
        /// </summary>
        /// <example>EmManutencao</example>
        [Required(ErrorMessage = "O status é obrigatório.")]
        public required string Status { get; set; }
    }
}