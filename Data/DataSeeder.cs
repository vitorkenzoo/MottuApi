using Microsoft.EntityFrameworkCore;
using MottuApi.Core.Entities;
using MottuApi.Data;

namespace MottuApi.Data
{
    /// <summary>
    /// Classe responsável por popular o banco de dados com dados iniciais (seed data).
    /// </summary>
    public static class DataSeeder
    {
        /// <summary>
        /// Popula o banco de dados com dados iniciais se as tabelas estiverem vazias.
        /// </summary>
        public static async Task SeedAsync(AppDbContext context)
        {
            // --- Seed de Clientes ---
            // Verifica se a tabela Clientes está vazIA antes de popular
            if (await context.Clientes.CountAsync() == 0)
            {
                var clientes = new List<Cliente>
                {
                    new Cliente
                    {
                        Nome = "João Silva",
                        Cpf = "123.456.789-00",
                        DataNascimento = new DateOnly(1990, 5, 20),
                        NumeroCNH = "98765432100",
                        TipoCNH = TipoCNH.AB
                    },
                    new Cliente
                    {
                        Nome = "Maria Santos",
                        Cpf = "234.567.890-11",
                        DataNascimento = new DateOnly(1985, 8, 15),
                        NumeroCNH = "87654321099",
                        TipoCNH = TipoCNH.A
                    },
                    new Cliente
                    {
                        Nome = "Pedro Oliveira",
                        Cpf = "345.678.901-22",
                        DataNascimento = new DateOnly(1992, 3, 10),
                        NumeroCNH = "76543210988",
                        TipoCNH = TipoCNH.A
                    },
                    new Cliente
                    {
                        Nome = "Ana Costa",
                        Cpf = "456.789.012-33",
                        DataNascimento = new DateOnly(1988, 11, 25),
                        NumeroCNH = "65432109877",
                        TipoCNH = TipoCNH.AB
                    },
                    new Cliente
                    {
                        Nome = "Carlos Pereira",
                        Cpf = "567.890.123-44",
                        DataNascimento = new DateOnly(1995, 7, 5),
                        NumeroCNH = "54321098766",
                        TipoCNH = TipoCNH.B
                    }
                };

                await context.Clientes.AddRangeAsync(clientes);
                await context.SaveChangesAsync();
            }

            // --- Seed de Motos ---
            // Verifica se a tabela Motos está vazIA antes de popular
            if (await context.Motos.CountAsync() == 0)
            {
                var motos = new List<Moto>
                {
                    new Moto { Ano = 2024, Modelo = "Honda Pop 110i", Placa = "ABC-1234", Status = MotoStatus.Disponivel },
                    new Moto { Ano = 2023, Modelo = "Yamaha Factor 150", Placa = "DEF-5678", Status = MotoStatus.Disponivel },
                    new Moto { Ano = 2024, Modelo = "Honda CG 160", Placa = "GHI-9012", Status = MotoStatus.Disponivel },
                    new Moto { Ano = 2023, Modelo = "Yamaha NMAX", Placa = "JKL-3456", Status = MotoStatus.Disponivel },
                    new Moto { Ano = 2024, Modelo = "Honda Biz 125", Placa = "MNO-7890", Status = MotoStatus.Disponivel },
                    new Moto { Ano = 2023, Modelo = "Yamaha Fazer 250", Placa = "PQR-1357", Status = MotoStatus.Disponivel },
                    new Moto { Ano = 2024, Modelo = "Honda PCX 160", Placa = "STU-2468", Status = MotoStatus.Disponivel },
                    new Moto { Ano = 2022, Modelo = "Yamaha XRE 300", Placa = "VWX-3691", Status = MotoStatus.EmManutencao }
                };

                await context.Motos.AddRangeAsync(motos);
                await context.SaveChangesAsync();
            }

            // --- Seed de Locações ---
            // Verifica se a tabela Locacoes está vazIA antes de popular
            if (await context.Locacoes.CountAsync() == 0)
            {
                // Precisamos dos clientes e motos que já estão no banco
                var clientesDoBanco = await context.Clientes.ToListAsync();
                var motosDoBanco = await context.Motos.ToListAsync();

                // Garante que temos dados para criar locações
                if (clientesDoBanco.Any() && motosDoBanco.Any())
                {
                    var locacoes = new List<Locacao>
                    {
                        new Locacao
                        {
                            Cliente = clientesDoBanco[0], // João Silva
                            Moto = motosDoBanco[0], // Honda Pop 110i
                            DataInicio = DateTime.Now.AddDays(-2),
                            DataFimPrevista = DateTime.Now.AddDays(5),
                            ValorDiaria = 50.00m,
                            Status = StatusLocacao.Ativa
                        },
                        new Locacao
                        {
                            Cliente = clientesDoBanco[1], // Maria Santos
                            Moto = motosDoBanco[1], // Yamaha Factor 150
                            DataInicio = DateTime.Now.AddDays(-10),
                            DataFimPrevista = DateTime.Now.AddDays(-3),
                            DataFimReal = DateTime.Now.AddDays(-3),
                            ValorDiaria = 60.00m,
                            ValorTotal = 420.00m, // 7 dias
                            Status = StatusLocacao.Concluida
                        },
                        new Locacao
                        {
                            Cliente = clientesDoBanco[2], // Pedro Oliveira
                            Moto = motosDoBanco[2], // Honda CG 160
                            DataInicio = DateTime.Now.AddDays(-1),
                            DataFimPrevista = DateTime.Now.AddDays(6),
                            ValorDiaria = 55.00m,
                            Status = StatusLocacao.Ativa
                        },
                        new Locacao
                        {
                            Cliente = clientesDoBanco[3], // Ana Costa
                            Moto = motosDoBanco[3], // Yamaha NMAX
                            DataInicio = DateTime.Now.AddDays(-15),
                            DataFimPrevista = DateTime.Now.AddDays(-8),
                            DataFimReal = DateTime.Now.AddDays(-8),
                            ValorDiaria = 65.00m,
                            ValorTotal = 455.00m, // 7 dias
                            Status = StatusLocacao.Concluida
                        }
                    };

                    await context.Locacoes.AddRangeAsync(locacoes);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}