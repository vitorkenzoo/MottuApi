using AutoMapper;
using MottuApi.Core.Entities;
using MottuApi.Core.Interfaces;
using MottuApi.Data;
using MottuApi.DTOs.ClienteDtos;
using MottuApi.DTOs.Shared;

namespace MottuApi.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly ILocacaoRepository _locacaoRepository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public ClienteService(IClienteRepository clienteRepository, ILocacaoRepository locacaoRepository, IMapper mapper, AppDbContext context)
        {
            _clienteRepository = clienteRepository;
            _locacaoRepository = locacaoRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ReadClienteDto?> GetByIdAsync(int id)
        {
            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente == null) return null;

            return _mapper.Map<ReadClienteDto>(cliente);
        }

        public async Task<PagedResult<ReadClienteDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var clientes = await _clienteRepository.GetAllPaginatedAsync(pageNumber, pageSize);
            var totalCount = await _clienteRepository.GetCountAsync();

            var clienteDtos = _mapper.Map<List<ReadClienteDto>>(clientes);

            return new PagedResult<ReadClienteDto>(clienteDtos, totalCount, pageNumber, pageSize);
        }

        public async Task<ReadClienteDto> CreateAsync(CreateClienteDto createClienteDto)
        {
            // Regra 1: Verificar duplicidade de CPF e CNH
            if (await _clienteRepository.GetByCpfAsync(createClienteDto.Cpf) != null)
                throw new InvalidOperationException("Este CPF já está cadastrado.");
            if (await _clienteRepository.GetByCnhAsync(createClienteDto.NumeroCNH) != null)
                throw new InvalidOperationException("Esta CNH já está cadastrada.");

            // Regra 2: Validar tipo de CNH
            if (!Enum.TryParse<TipoCNH>(createClienteDto.TipoCNH, true, out var tipoCnh) || tipoCnh == TipoCNH.B)
                throw new InvalidOperationException("O tipo de CNH deve ser 'A' ou 'AB'.");

            // Regra 3: Validar idade (exemplo: maior de 18)
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var age = today.Year - createClienteDto.DataNascimento.Year;
            if (createClienteDto.DataNascimento > today.AddYears(-age)) age--;
            if (age < 18)
                throw new InvalidOperationException("O cliente deve ter no mínimo 18 anos.");

            var cliente = _mapper.Map<Cliente>(createClienteDto);

            await _clienteRepository.AddAsync(cliente);
            await _context.SaveChangesAsync();

            return _mapper.Map<ReadClienteDto>(cliente);
        }

        public async Task UpdateAsync(int id, UpdateClienteDto updateClienteDto)
        {
            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente == null)
                throw new KeyNotFoundException("Cliente não encontrado.");

            // Regra de negócio: Não permitir alteração se houver locação ativa
            var locacaoAtiva = await _locacaoRepository.GetActiveLocacaoByClienteIdAsync(id);
            if (locacaoAtiva != null)
                throw new InvalidOperationException("Não é possível alterar dados de um cliente com locação ativa.");

            _mapper.Map(updateClienteDto, cliente);
            _clienteRepository.Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente == null)
                throw new KeyNotFoundException("Cliente não encontrado.");

            // Regra de negócio: Não permitir exclusão se houver locação ativa
            var locacaoAtiva = await _locacaoRepository.GetActiveLocacaoByClienteIdAsync(id);
            if (locacaoAtiva != null)
                throw new InvalidOperationException("Não é possível excluir um cliente com locação ativa.");

            _clienteRepository.Delete(cliente);
            await _context.SaveChangesAsync();
        }
    }
}