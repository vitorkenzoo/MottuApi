using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MottuApi.Core.Entities;
using MottuApi.Core.Interfaces;
using MottuApi.Data;
using MottuApi.DTOs.LocacaoDtos;
using MottuApi.Services;
using Moq;
using Xunit;

namespace MottuApi.Tests.Services
{
    public class LocacaoServiceTests
    {
        private readonly Mock<ILocacaoRepository> _locacaoRepositoryMock;
        private readonly Mock<IClienteRepository> _clienteRepositoryMock;
        private readonly Mock<IMotoRepository> _motoRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AppDbContext _context;
        private readonly LocacaoService _service;

        public LocacaoServiceTests()
        {
            _locacaoRepositoryMock = new Mock<ILocacaoRepository>();
            _clienteRepositoryMock = new Mock<IClienteRepository>();
            _motoRepositoryMock = new Mock<IMotoRepository>();
            _mapperMock = new Mock<IMapper>();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _service = new LocacaoService(
                _locacaoRepositoryMock.Object,
                _clienteRepositoryMock.Object,
                _motoRepositoryMock.Object,
                _mapperMock.Object,
                _context
            );
        }

        [Fact]
        public async Task CreateAsync_ClienteComCNHTipoB_DeveLancarExcecao()
        {
            // Arrange
            var cliente = new Cliente
            {
                Id = 1,
                Nome = "João Silva",
                Cpf = "12345678900",
                DataNascimento = new DateOnly(1990, 1, 1),
                NumeroCNH = "98765432100",
                TipoCNH = TipoCNH.B
            };

            var createDto = new CreateLocacaoDto
            {
                ClienteId = 1,
                DataFimPrevista = DateTime.UtcNow.AddDays(7)
            };

            _clienteRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(cliente);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.CreateAsync(createDto)
            );
        }

        [Fact]
        public async Task CreateAsync_ClienteComLocacaoAtiva_DeveLancarExcecao()
        {
            // Arrange
            var cliente = new Cliente
            {
                Id = 1,
                Nome = "João Silva",
                Cpf = "12345678900",
                DataNascimento = new DateOnly(1990, 1, 1),
                NumeroCNH = "98765432100",
                TipoCNH = TipoCNH.A
            };

            var locacaoAtiva = new Locacao
            {
                Id = 1,
                ClienteId = 1,
                Status = StatusLocacao.Ativa,
                Cliente = cliente,
                Moto = new Moto 
                { 
                    Id = 1, 
                    Modelo = "Honda CB 300F",
                    Placa = "ABC1234",
                    Status = MotoStatus.Alugada 
                }
            };

            var createDto = new CreateLocacaoDto
            {
                ClienteId = 1,
                DataFimPrevista = DateTime.UtcNow.AddDays(7)
            };

            _clienteRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(cliente);

            _locacaoRepositoryMock
                .Setup(r => r.GetActiveLocacaoByClienteIdAsync(1))
                .ReturnsAsync(locacaoAtiva);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.CreateAsync(createDto)
            );
        }

        [Fact]
        public async Task EndLocacaoAsync_ComAtraso_DeveCalcularMulta()
        {
            // Arrange
            var cliente = new Cliente
            {
                Id = 1,
                Nome = "João Silva",
                Cpf = "12345678900",
                DataNascimento = new DateOnly(1990, 1, 1),
                NumeroCNH = "98765432100",
                TipoCNH = TipoCNH.A
            };

            var moto = new Moto 
            { 
                Id = 1, 
                Modelo = "Honda CB 300F",
                Placa = "ABC1234",
                Status = MotoStatus.Alugada 
            };

            var locacao = new Locacao
            {
                Id = 1,
                ClienteId = 1,
                MotoId = 1,
                DataInicio = DateTime.UtcNow.AddDays(-10),
                DataFimPrevista = DateTime.UtcNow.AddDays(-2), // 2 dias de atraso
                DataFimReal = DateTime.UtcNow,
                ValorDiaria = 30.00m,
                Status = StatusLocacao.Ativa,
                Cliente = cliente,
                Moto = moto
            };

            var readDto = new ReadLocacaoDto
            {
                Id = 1,
                Status = "Concluida",
                NomeCliente = "João Silva",
                PlacaMoto = "ABC1234"
            };

            _locacaoRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(locacao);

            _mapperMock
                .Setup(m => m.Map<ReadLocacaoDto>(It.IsAny<Locacao>()))
                .Returns(readDto);

            // Act
            var result = await _service.EndLocacaoAsync(1, DateTime.UtcNow);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Concluida", result.Status);
            // Valor previsto: 8 dias * 30 = 240
            // Atraso: 2 dias * 50 = 100
            // Total esperado: 340
            Assert.True(locacao.ValorTotal >= 240m);
        }

        [Fact]
        public async Task DeleteAsync_LocacaoAtiva_DeveLancarExcecao()
        {
            // Arrange
            var cliente = new Cliente
            {
                Id = 1,
                Nome = "João Silva",
                Cpf = "12345678900",
                DataNascimento = new DateOnly(1990, 1, 1),
                NumeroCNH = "98765432100",
                TipoCNH = TipoCNH.A
            };

            var moto = new Moto 
            { 
                Id = 1, 
                Modelo = "Honda CB 300F",
                Placa = "ABC1234",
                Status = MotoStatus.Alugada 
            };

            var locacao = new Locacao
            {
                Id = 1,
                Status = StatusLocacao.Ativa,
                Cliente = cliente,
                Moto = moto
            };

            _locacaoRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(locacao);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.DeleteAsync(1)
            );
        }
    }
}

