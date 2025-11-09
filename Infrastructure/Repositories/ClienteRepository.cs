using Microsoft.EntityFrameworkCore;
using MottuApi.Core.Entities;
using MottuApi.Core.Interfaces;
using MottuApi.Data;

namespace MottuApi.Infrastructure.Repositories
{
    /// <summary>
    /// Implementação do repositório de Clientes, utilizando EF Core.
    /// </summary>
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _context;

        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cliente?> GetByIdAsync(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        public async Task<IEnumerable<Cliente>> GetAllPaginatedAsync(int pageNumber, int pageSize)
        {
            return await _context.Clientes
                                 .AsNoTracking() // Melhora a performance para consultas de apenas leitura
                                 .OrderBy(c => c.Id) // Ordena por ID para garantir resultados consistentes
                                 .Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();
        }

        public async Task<Cliente?> GetByCpfAsync(string cpf)
        {
            return await _context.Clientes.FirstOrDefaultAsync(c => c.Cpf == cpf);
        }

        public async Task<Cliente?> GetByCnhAsync(string cnh)
        {
            return await _context.Clientes.FirstOrDefaultAsync(c => c.NumeroCNH == cnh);
        }

        public async Task AddAsync(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
        }

        public void Update(Cliente cliente)
        {
            _context.Entry(cliente).State = EntityState.Modified;
        }

        public void Delete(Cliente cliente)
        {
            _context.Clientes.Remove(cliente);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Clientes.CountAsync();
        }

    }
}