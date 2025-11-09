using MottuApi.DTOs.Shared;

namespace MottuApi.DTOs.MotoDtos
{
    /// <summary>
    /// DTO para a leitura e retorno de dados de uma moto.
    /// Este é o objeto que será serializado como JSON e retornado nos endpoints GET.
    /// </summary>
    public class ReadMotoDto
    {
        /// <summary>
        /// Identificador único da moto.
        /// </summary>
        /// <example>101</example>
        public int Id { get; set; }

        /// <summary>
        /// Ano de fabricação da moto.
        /// </summary>
        /// <example>2024</example>
        public int Ano { get; set; }

        /// <summary>
        /// Modelo da moto.
        /// </summary>
        /// <example>Honda Pop 110i</example>
        public required string Modelo { get; set; }

        /// <summary>
        /// Placa de identificação da moto.
        /// </summary>
        /// <example>XYZ-1234</example>
        public required string Placa { get; set; }

        /// <summary>
        /// Status atual da moto.
        /// </summary>
        /// <example>Disponivel</example>
        public required string Status { get; set; }

        /// <summary>
        /// Lista de links HATEOAS relacionados à moto.
        /// </summary>
        public List<LinkDto> Links { get; set; } = new();
    }
}