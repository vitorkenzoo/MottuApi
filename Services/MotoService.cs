using AutoMapper;
using MottuApi.Core.Entities;
using MottuApi.Core.Interfaces;
using MottuApi.Data;
using MottuApi.DTOs.MotoDtos;
using MottuApi.DTOs.Shared;

namespace MottuApi.Services
{
    public class MotoService : IMotoService
    {
        private readonly IMotoRepository _motoRepository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public MotoService(IMotoRepository motoRepository, IMapper mapper, AppDbContext context)
        {
            _motoRepository = motoRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ReadMotoDto?> GetByIdAsync(int id)
        {
            var moto = await _motoRepository.GetByIdAsync(id);
            if (moto == null) return null;

            return _mapper.Map<ReadMotoDto>(moto);
        }

        public async Task<PagedResult<ReadMotoDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var motos = await _motoRepository.GetAllPaginatedAsync(pageNumber, pageSize);
            var totalCount = await _motoRepository.GetCountAsync();

            var motoDtos = _mapper.Map<List<ReadMotoDto>>(motos);

            return new PagedResult<ReadMotoDto>(motoDtos, totalCount, pageNumber, pageSize);
        }

        public async Task<ReadMotoDto> CreateAsync(CreateMotoDto createMotoDto)
        {
            // Regra de negócio: Verifica se a placa já existe
            var existingMoto = await _motoRepository.GetByPlacaAsync(createMotoDto.Placa);
            if (existingMoto != null)
            {
                throw new InvalidOperationException("Uma moto com esta placa já está cadastrada.");
            }

            var moto = _mapper.Map<Moto>(createMotoDto);

            // Regra de negócio: Define o status inicial
            moto.Status = MotoStatus.Disponivel;

            await _motoRepository.AddAsync(moto);
            await _context.SaveChangesAsync(); // Efetiva a transação

            return _mapper.Map<ReadMotoDto>(moto);
        }

        public async Task UpdateAsync(int id, UpdateMotoDto updateMotoDto)
        {
            var moto = await _motoRepository.GetByIdAsync(id);
            if (moto == null)
            {
                throw new KeyNotFoundException("Moto não encontrada.");
            }

            // Regra de negócio: Impede a alteração de uma moto alugada
            if (moto.Status == MotoStatus.Alugada)
            {
                throw new InvalidOperationException("Não é possível alterar os dados de uma moto que está alugada.");
            }

            _mapper.Map(updateMotoDto, moto);
            _motoRepository.Update(moto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var moto = await _motoRepository.GetByIdAsync(id);
            if (moto == null)
            {
                throw new KeyNotFoundException("Moto não encontrada.");
            }

            // Regra de negócio: Impede a exclusão de uma moto alugada
            if (moto.Status == MotoStatus.Alugada)
            {
                throw new InvalidOperationException("Não é possível remover uma moto que está alugada.");
            }

            _motoRepository.Delete(moto);
            await _context.SaveChangesAsync();
        }
    }
}