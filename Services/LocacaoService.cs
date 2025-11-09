using AutoMapper;
using MottuApi.Core.Entities;
using MottuApi.Core.Interfaces;
using MottuApi.Data;
using MottuApi.DTOs.LocacaoDtos;
using MottuApi.DTOs.Shared;

namespace MottuApi.Services
{
    public class LocacaoService : ILocacaoService
    {
        private readonly ILocacaoRepository _locacaoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IMotoRepository _motoRepository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public LocacaoService(
            ILocacaoRepository locacaoRepository,
            IClienteRepository clienteRepository,
            IMotoRepository motoRepository,
            IMapper mapper,
            AppDbContext context)
        {
            _locacaoRepository = locacaoRepository;
            _clienteRepository = clienteRepository;
            _motoRepository = motoRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ReadLocacaoDto?> GetByIdAsync(int id)
        {
            var locacao = await _locacaoRepository.GetByIdAsync(id);
            return _mapper.Map<ReadLocacaoDto>(locacao);
        }

        public async Task<PagedResult<ReadLocacaoDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var locacoes = await _locacaoRepository.GetAllPaginatedAsync(pageNumber, pageSize);
            var totalCount = await _locacaoRepository.GetCountAsync();
            var dtos = _mapper.Map<List<ReadLocacaoDto>>(locacoes);
            return new PagedResult<ReadLocacaoDto>(dtos, totalCount, pageNumber, pageSize);
        }

        public async Task<ReadLocacaoDto> CreateAsync(CreateLocacaoDto createLocacaoDto)
        {
            var cliente = await _clienteRepository.GetByIdAsync(createLocacaoDto.ClienteId) ??
                          throw new KeyNotFoundException("Cliente não encontrado.");

            if (cliente.TipoCNH == TipoCNH.B)
                throw new InvalidOperationException("Cliente não possui CNH do tipo 'A' ou 'AB'.");

            if (await _locacaoRepository.GetActiveLocacaoByClienteIdAsync(cliente.Id) != null)
                throw new InvalidOperationException("Cliente já possui uma locação ativa.");

            var moto = await _motoRepository.FindFirstAvailableAsync() ??
                       throw new InvalidOperationException("Nenhuma moto disponível para locação no momento.");

            var locacao = _mapper.Map<Locacao>(createLocacaoDto);
            locacao.DataInicio = DateTime.UtcNow;
            locacao.Status = StatusLocacao.Ativa;
            locacao.MotoId = moto.Id;
            locacao.ValorDiaria = CalculateDailyRate(locacao.DataInicio, locacao.DataFimPrevista);

            moto.Status = MotoStatus.Alugada;
            _motoRepository.Update(moto);

            await _locacaoRepository.AddAsync(locacao);
            await _context.SaveChangesAsync();

            var createdLocacao = await _locacaoRepository.GetByIdAsync(locacao.Id);
            return _mapper.Map<ReadLocacaoDto>(createdLocacao);
        }

        public async Task<ReadLocacaoDto> EndLocacaoAsync(int locacaoId, DateTime dataDevolucao)
        {
            var locacao = await _locacaoRepository.GetByIdAsync(locacaoId) ??
                          throw new KeyNotFoundException("Locação não encontrada.");

            if (locacao.Status == StatusLocacao.Concluida)
                throw new InvalidOperationException("Esta locação já foi concluída.");

            if (dataDevolucao < locacao.DataInicio)
                throw new InvalidOperationException("A data de devolução não pode ser anterior à data de início.");

            locacao.DataFimReal = dataDevolucao;
            locacao.Status = StatusLocacao.Concluida;
            locacao.ValorTotal = CalculateTotalCost(locacao);

            locacao.Moto.Status = MotoStatus.Disponivel;

            _locacaoRepository.Update(locacao);
            _motoRepository.Update(locacao.Moto);

            await _context.SaveChangesAsync();

            return _mapper.Map<ReadLocacaoDto>(locacao);
        }

        public async Task DeleteAsync(int id)
        {
            // Em um sistema real, transações como locações não seriam deletadas, mas sim canceladas.
            // Para fins de simplicidade do CRUD, a deleção é mantida.
            var locacao = await _locacaoRepository.GetByIdAsync(id) ??
                          throw new KeyNotFoundException("Locação não encontrada.");

            if (locacao.Status == StatusLocacao.Ativa)
                throw new InvalidOperationException("Não é possível deletar uma locação ativa. Finalize-a primeiro.");

            _locacaoRepository.Delete(locacao);
            await _context.SaveChangesAsync();
        }

        // --- Métodos Privados de Lógica de Negócio ---

        private decimal CalculateDailyRate(DateTime start, DateTime end)
        {
            var planDays = (end.Date - start.Date).Days;
            if (planDays <= 7) return 30.00m;
            if (planDays <= 15) return 28.00m;
            return 22.00m; // Planos de 30, 45, 50 dias
        }

        private decimal CalculateTotalCost(Locacao locacao)
        {
            var diasPrevistos = (locacao.DataFimPrevista.Date - locacao.DataInicio.Date).Days;
            var diasEfetivos = (locacao.DataFimReal!.Value.Date - locacao.DataInicio.Date).Days;

            if (diasEfetivos < diasPrevistos)
            {
                // Devolução adiantada com multa
                var diasRestantes = diasPrevistos - diasEfetivos;
                var custoEfetivo = diasEfetivos * locacao.ValorDiaria;
                var multaPercentual = diasPrevistos <= 7 ? 0.20m : 0.40m; // 20% para 7 dias, 40% para 15+
                var multa = (diasRestantes * locacao.ValorDiaria) * multaPercentual;
                return custoEfetivo + multa;
            }
            else
            {
                // Devolução no prazo ou atrasada
                var diasAtraso = Math.Max(0, diasEfetivos - diasPrevistos);
                var custoPrevisto = diasPrevistos * locacao.ValorDiaria;
                var custoAdicional = diasAtraso * 50.00m; // Taxa de R$50 por dia de atraso
                return custoPrevisto + custoAdicional;
            }
        }
    }
}