using AutoMapper;
using MottuApi.Core.Entities;
using MottuApi.DTOs.ClienteDtos;
using MottuApi.DTOs.LocacaoDtos;
using MottuApi.DTOs.MotoDtos;

namespace MottuApi.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // --- Mapeamentos para Moto ---

            // Mapeia da entidade Moto para o DTO de leitura.
            // O AutoMapper converte o enum MotoStatus para string automaticamente.
            CreateMap<Moto, ReadMotoDto>();

            // Mapeia do DTO de criação para a entidade Moto.
            CreateMap<CreateMotoDto, Moto>();

            // Mapeia do DTO de atualização para a entidade Moto.
            CreateMap<UpdateMotoDto, Moto>();


            // --- Mapeamentos para Cliente ---

            CreateMap<Cliente, ReadClienteDto>();
            CreateMap<CreateClienteDto, Cliente>();
            CreateMap<UpdateClienteDto, Cliente>();


            // --- Mapeamentos para Locacao ---

            // Mapeamento com configuração customizada para "achatar" os dados.
            CreateMap<Locacao, ReadLocacaoDto>()
                .ForMember(dest => dest.NomeCliente, opt => opt.MapFrom(src => src.Cliente.Nome))
                .ForMember(dest => dest.PlacaMoto, opt => opt.MapFrom(src => src.Moto.Placa));

            CreateMap<CreateLocacaoDto, Locacao>();
        }
    }
}