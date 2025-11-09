using System.ComponentModel.DataAnnotations;

namespace MottuApi.DTOs.MotoDtos
{
    /// <summary>
    /// DTO para receber os dados necessários para a criação de uma nova moto.
    /// Este é o objeto esperado no corpo (body) de requisições POST.
    /// </summary>
    public class CreateMotoDto
    {
        /// <summary>
        /// Ano de fabricação da moto. Deve ser um ano válido.
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
        /// Placa de identificação da moto (deve ser única).
        /// </summary>
        /// <example>NEW-2025</example>
        [Required(ErrorMessage = "A placa da moto é obrigatória.")]
        [StringLength(10, MinimumLength = 7, ErrorMessage = "A placa deve ter entre 7 e 10 caracteres.")]
        public required string Placa { get; set; }
    }
}