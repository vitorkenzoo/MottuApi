using Microsoft.EntityFrameworkCore;
using MottuApi.Core.Entities;
using MottuApi.Core.Interfaces;
using MottuApi.Data;

namespace MottuApi.Infrastructure.Repositories
{
    /// <summary>
    /// Implementação do repositório de Locações, utilizando EF Core.
    /// </summary>
    public class LocacaoRepository : ILocacaoRepository
    {
        private readonly AppDbContext _context;

        public LocacaoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Locacao?> GetByIdAsync(int id)
        {
            // Eager loading: Inclui os dados do Cliente e da Moto na consulta
            // para que não precisemos fazer queries separadas depois.
            return await _context.Locacoes
                                 .Include(l => l.Cliente)
                                 .Include(l => l.Moto)
                                 .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<Locacao>> GetAllPaginatedAsync(int pageNumber, int pageSize)
        {
            return await _context.Locacoes
                                 .Include(l => l.Cliente)
                                 .Include(l => l.Moto)
                                 .AsNoTracking()
                                 .OrderByDescending(l => l.DataInicio) // Ordena pelas mais recentes
                                 .Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();
        }

        public async Task<Locacao?> GetActiveLocacaoByClienteIdAsync(int clienteId)
        {
            return await _context.Locacoes
                                 .FirstOrDefaultAsync(l =>
                                     l.ClienteId == clienteId &&
                                     l.Status == StatusLocacao.Ativa);
        }

        public async Task AddAsync(Locacao locacao)
        {
            await _context.Locacoes.AddAsync(locacao);
        }

        public void Update(Locacao locacao)
        {
            _context.Entry(locacao).State = EntityState.Modified;
        }



        public void Delete(Locacao locacao)
        {
            _context.Locacoes.Remove(locacao);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Locacoes.CountAsync();
        }

    }
}