using System.ComponentModel.DataAnnotations;

namespace MottuApi.DTOs.ClienteDtos
{
    /// <summary>
    /// DTO para receber os dados necessários para a atualização de um cliente.
    /// Note que apenas o nome pode ser alterado. Outros dados como CPF e CNH
    /// são considerados imutáveis após o cadastro.
    /// </summary>
    public class UpdateClienteDto
    {
        /// <summary>
        /// Nome completo do cliente.
        /// </summary>
        /// <example>Maria Souza Santos</example>
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 150 caracteres.")]
        public required string Nome { get; set; }
    }
}