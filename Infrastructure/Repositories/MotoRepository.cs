using Microsoft.EntityFrameworkCore;
using MottuApi.Core.Entities;
using MottuApi.Core.Interfaces;
using MottuApi.Data;

namespace MottuApi.Infrastructure.Repositories
{
    /// <summary>
    /// Implementação do repositório de Motos.
    /// Esta classe utiliza o Entity Framework Core para interagir com o banco de dados.
    /// </summary>
    public class MotoRepository : IMotoRepository
    {
        private readonly AppDbContext _context;

        public MotoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Moto?> GetByIdAsync(int id)
        {
            return await _context.Motos.FindAsync(id);
        }

        public async Task<IEnumerable<Moto>> GetAllPaginatedAsync(int pageNumber, int pageSize)
        {
            // Pula os itens das páginas anteriores e pega a quantidade de itens da página atual.
            return await _context.Motos
                                 .OrderBy(m => m.Id) // Ordena por ID para garantir resultados consistentes
                                 .Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();
        }

        public async Task<Moto?> GetByPlacaAsync(string placa)
        {
            // FirstOrDefaultAsync retorna o primeiro elemento que satisfaz a condição, ou nulo se nenhum for encontrado.
            return await _context.Motos.FirstOrDefaultAsync(m => m.Placa == placa);
        }

        public async Task AddAsync(Moto moto)
        {
            // Adiciona a entidade ao contexto. O EF Core irá rastrear essa entidade.
            await _context.Motos.AddAsync(moto);
        }

        public void Update(Moto moto)
        {
            // Apenas marca a entidade como modificada. A gravação no banco ocorrerá no SaveChanges.
            _context.Entry(moto).State = EntityState.Modified;
        }

        public void Delete(Moto moto)
        {
            // Apenas marca a entidade como deletada. A remoção do banco ocorrerá no SaveChanges.
            _context.Motos.Remove(moto);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Motos.CountAsync();
        }

        public async Task<Moto?> FindFirstAvailableAsync()
        {
            return await _context.Motos.FirstOrDefaultAsync(m => m.Status == MotoStatus.Disponivel);
        }

    }
}