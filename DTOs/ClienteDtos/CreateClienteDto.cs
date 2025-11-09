using System.ComponentModel.DataAnnotations;

namespace MottuApi.DTOs.ClienteDtos
{
    /// <summary>
    /// DTO para receber os dados necessários para a criação de um novo cliente.
    /// Este é o objeto esperado no corpo de requisições POST para /clientes.
    /// </summary>
    public class CreateClienteDto
    {
        /// <summary>
        /// Nome completo do cliente.
        /// </summary>
        /// <example>Carlos Pereira</example>
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 150 caracteres.")]
        public required string Nome { get; set; }

        /// <summary>
        /// CPF do cliente (somente números).
        /// </summary>
        /// <example>12345678900</example>
        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve conter exatamente 11 dígitos.")]
        public required string Cpf { get; set; }

        /// <summary>
        /// Data de nascimento do cliente. A idade mínima será validada pela lógica de negócio.
        /// </summary>
        /// <example>1992-11-30</example>
        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        public DateOnly DataNascimento { get; set; }

        /// <summary>
        /// Número da CNH do cliente.
        /// </summary>
        /// <example>98765432100</example>
        [Required(ErrorMessage = "O número da CNH é obrigatório.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "A CNH deve conter exatamente 11 dígitos.")]
        public required string NumeroCNH { get; set; }

        /// <summary>
        /// Tipo da CNH do cliente. Valores válidos: "A", "B", "AB".
        /// </summary>
        /// <example>A</example>
        [Required(ErrorMessage = "O tipo da CNH é obrigatório.")]
        public required string TipoCNH { get; set; }
    }
}