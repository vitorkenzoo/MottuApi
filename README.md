# 🏍️ Mottu API

## 📖 Descrição do Projeto

Mottu API é um sistema de back-end desenvolvido em **ASP.NET Core 8** para gerenciar o aluguel de motocicletas para a empresa **Mottu**.  
A API permite o cadastro de clientes e motos, além de controlar todo o ciclo de vida de uma locação, desde sua criação até a devolução, incluindo a lógica de cálculo de custos.

Este projeto foi construído com foco em boas práticas de desenvolvimento, utilizando uma arquitetura robusta e bem definida para garantir **manutenibilidade, testabilidade e escalabilidade**.

---

## 👥 Integrantes

- Vitor Kenzo Mizumoto - RM557245
- Adriano Barutti Pessuto - RM556760

---

## 🏗️ Arquitetura do Projeto

Foi adotada uma **Arquitetura em Camadas (Layered Architecture)**, inspirada nos princípios da **Clean Architecture**.  
Essa abordagem visa a **Separação de Responsabilidades (SoC)**, desacoplando a lógica de negócio das demais partes da aplicação.

### Estrutura de Pastas

- **/Core**: Entidades de negócio (Moto, Cliente, Locacao) e Interfaces para repositórios e serviços.  
- **/Infrastructure**: Implementação de acesso a dados com **Entity Framework Core**.  
- **/Services**: Contém a lógica de negócio, validações e cálculos de custo.  
- **/DTOs**: Objetos de transferência de dados, usados para requisições e respostas.  
- **/Controllers**: Camada mais externa que recebe requisições HTTP e retorna respostas.

### Benefícios

- **Manutenibilidade**: Código modular e fácil de modificar.  
- **Testabilidade**: Uso de interfaces permite testes unitários independentes.  
- **Desacoplamento**: Banco de dados pode ser trocado alterando apenas a camada *Infrastructure*.  

---

## 🛠️ Tecnologias Utilizadas

- **.NET 8**  
- **ASP.NET Core Web API**  
- **Entity Framework Core 9**  
- **Oracle Database**  
- **AutoMapper**  
- **Swagger/OpenAPI**  
- **Asp.Versioning.Mvc.ApiExplorer** (Versionamento de API)  
- **Microsoft.ML** (Machine Learning)  
- **xUnit** (Testes)  
- **Moq** (Mocking para testes)  
- **Microsoft.AspNetCore.Mvc.Testing** (Testes de integração)

---

## 🚀 Como Executar o Projeto

### ✅ Pré-requisitos

- .NET 8 SDK  
- Git  
- Banco de Dados Oracle (opcional para alguns endpoints)

### 📌 Passo a Passo

1. **Clonar o repositório:**
   ```bash
   git clone https://github.com/vitorkenzoo/Dotnet
   cd MottuApi
   ```

2. **Restaurar dependências:**
   ```bash
   dotnet restore
   ```

3. **Configurar conexão e API Key no `appsettings.json`:**
   ```json
   {
   "ConnectionStrings": {
       "OracleDb": "User Id=RM557256;Password=021005;Data Source=oracle.fiap.com.br:1521/ORCL"
     },
     "ApiKey": {
       "SecretKey": "MottuApi-Secret-Key-2024-Development"
     }
   }
   ```
   
   **⚠️ Importante**: A API Key configurada acima é apenas para desenvolvimento. Em produção, use uma chave segura e armazene-a em variáveis de ambiente ou em um gerenciador de segredos.

4. **Aplicar as Migrations (se usar banco de dados):**
   ```bash
   dotnet ef database update
   ```

5. **Compilar o projeto:**
   ```bash
   dotnet build
   ```

6. **Executar a aplicação:**
   ```bash
   dotnet run
   ```

7. **Acessar a API:**
   - **API Base**: [http://localhost:5020](http://localhost:5020)
   - **Swagger (Documentação)**: [http://localhost:5020/swagger](http://localhost:5020/swagger)
   - **Health Check**: [http://localhost:5020/health](http://localhost:5020/health)

---

## 🔐 Segurança (API Key)

A API utiliza autenticação via **API Key** para proteger os endpoints. Todas as requisições (exceto `/health` e `/swagger`) devem incluir o header `X-API-KEY` com a chave configurada no `appsettings.json`.

### Como usar a API Key

**No Swagger UI:**
1. Acesse a documentação Swagger: `http://localhost:5020/swagger`
2. Clique no botão **"Authorize"** (🔒 cadeado) no topo da página
3. Insira a API Key: `MottuApi-Secret-Key-2024-Development`
4. Clique em **"Authorize"** e depois em **"Close"**

**Em requisições HTTP:**
```http
GET /api/v1/clientes
X-API-KEY: MottuApi-Secret-Key-2024-Development
```

**Exemplo com cURL:**
```bash
curl -H "X-API-KEY: MottuApi-Secret-Key-2024-Development" \
     http://localhost:5020/api/v1/clientes
```

**⚠️ Endpoints que NÃO requerem API Key:**
- `/health` - Health Check
- `/swagger` - Documentação Swagger

---

## 📌 Versionamento da API

A API utiliza versionamento baseado em URL. A versão atual é **v1.0**.

### Estrutura de Rotas

- **v1.0**: `/api/v1/[controller]`
  - Exemplo: `/api/v1/clientes`
  - Exemplo: `/api/v1/motos`
  - Exemplo: `/api/v1/locacoes`

### Selecionar Versão no Swagger

No Swagger UI, você verá um dropdown no topo da página permitindo selecionar a versão da API (atualmente apenas **v1**).

---

## 🏥 Health Checks

A API possui um endpoint de Health Check que verifica a conectividade com o banco de dados Oracle.

### Endpoint
```http
GET /health
```

### Respostas
- **200 OK**: Banco de dados está saudável
- **503 Service Unavailable**: Banco de dados não está acessível

**⚠️ Nota**: Este endpoint **NÃO requer** API Key.

### Exemplo
```bash
curl http://localhost:5020/health
```

---

## 🤖 Machine Learning (ML.NET)

A API inclui um endpoint de predição de risco de clientes usando **ML.NET**. O modelo foi treinado para estimar o risco (Alto/Baixo) baseado na idade e tipo de CNH do cliente.

### Endpoint
```http
POST /api/v1/clientes/estimar-risco
```

### Request Body
```json
{
  "idade": 25,
  "tipoCNH": "A"
}
```

### Response
```json
{
  "risco": "Baixo",
  "idade": 25,
  "tipoCNH": "A"
}
```

### Exemplo de Uso
```bash
curl -X POST http://localhost:5020/api/v1/clientes/estimar-risco \
     -H "Content-Type: application/json" \
     -H "X-API-KEY: MottuApi-Secret-Key-2024-Development" \
     -d '{
       "idade": 25,
       "tipoCNH": "A"
     }'
```

### Modelo de Machine Learning
- **Arquivo de treino**: `Data/dados_treino.csv`
- **Modelo treinado**: `Model.zip` (gerado automaticamente na primeira execução)
- **Algoritmo**: SDCA Maximum Entropy (Classificação Multiclasse)
- **Features**: Idade, TipoCNH
- **Target**: Risco (Alto/Baixo)

**✅ Este endpoint funciona SEM necessidade de banco de dados!**

---

## 📌 Exemplos de Uso dos Endpoints

**⚠️ Lembre-se**: Todos os endpoints abaixo (exceto `/health`) requerem o header `X-API-KEY`.

### Criar Cliente
```http
POST /api/v1/clientes
```
```json
{
  "nome": "João da Silva",
  "cpf": "12345678900",
  "dataNascimento": "1990-05-20",
  "numeroCNH": "98765432100",
  "tipoCNH": "A"
}
```

### Criar Moto
```http
POST /api/v1/motos
```
```json
{
  "ano": 2024,
  "modelo": "Honda CB 300F",
  "placa": "ABC1D23"
}
```

### Iniciar Locação
```http
POST /api/v1/locacoes
```
```json
{
  "clienteId": 1,
  "dataFimPrevista": "2025-10-15T10:00:00Z"
}
```

### Finalizar Locação
```http
PUT /api/v1/locacoes/1/finalizar
```
```json
{
  "dataDevolucao": "2025-10-14T09:30:00Z"
}
```

### Estimar Risco do Cliente (ML.NET)
```http
POST /api/v1/clientes/estimar-risco
```
```json
{
  "idade": 30,
  "tipoCNH": "AB"
}
```

---

## 🧪 Como Testar a API

### 🎯 Teste Rápido (Funciona Sem Banco de Dados!)

Execute este comando enquanto a API está rodando:

```bash
# Teste o ML.NET (funciona sem banco!)
curl -X POST http://localhost:5020/api/v1/clientes/estimar-risco \
     -H "Content-Type: application/json" \
     -H "X-API-KEY: MottuApi-Secret-Key-2024-Development" \
     -d '{"idade": 25, "tipoCNH": "A"}'
```

**Resposta esperada:**
```json
{
  "risco": "Baixo",
  "idade": 25,
  "tipoCNH": "A"
}
```

### 1️⃣ Testar via Swagger (Recomendado - Mais Fácil)

1. **Abra o navegador** e acesse:
   ```
   http://localhost:5020/swagger
   ```

2. **Configure a API Key**:
   - Clique no botão **"Authorize"** (🔒 cadeado) no topo da página
   - No campo de valor, digite: `MottuApi-Secret-Key-2024-Development`
   - Clique em **"Authorize"** e depois em **"Close"**

3. **Teste os endpoints**:
   - Clique em qualquer endpoint (ex: `POST /api/v1/clientes/estimar-risco`)
   - Clique em **"Try it out"**
   - Preencha os dados ou cole o JSON de exemplo
   - Clique em **"Execute"**
   - Veja a resposta!

### 2️⃣ Testar via Linha de Comando (cURL)

#### Teste 1: Health Check (não precisa de API Key)
```bash
curl http://localhost:5020/health
```
**Nota:** Pode retornar "Unhealthy" se o Oracle não estiver acessível, mas o endpoint funciona.

#### Teste 2: Estimar Risco (ML.NET) - Funciona sem banco!
```bash
curl -X POST http://localhost:5020/api/v1/clientes/estimar-risco \
     -H "Content-Type: application/json" \
     -H "X-API-KEY: MottuApi-Secret-Key-2024-Development" \
     -d '{
       "idade": 30,
       "tipoCNH": "AB"
     }'
```

#### Teste 3: Listar Clientes (precisa de API Key e banco)
```bash
curl -H "X-API-KEY: MottuApi-Secret-Key-2024-Development" \
     http://localhost:5020/api/v1/clientes
```

#### Teste 4: Criar um Cliente
```bash
curl -X POST http://localhost:5020/api/v1/clientes \
     -H "Content-Type: application/json" \
     -H "X-API-KEY: MottuApi-Secret-Key-2024-Development" \
     -d '{
       "nome": "João Silva",
       "cpf": "12345678900",
       "dataNascimento": "1990-05-20",
       "numeroCNH": "98765432100",
       "tipoCNH": "A"
     }'
```

#### Teste 5: Criar uma Moto
```bash
curl -X POST http://localhost:5020/api/v1/motos \
     -H "Content-Type: application/json" \
     -H "X-API-KEY: MottuApi-Secret-Key-2024-Development" \
     -d '{
       "ano": 2024,
       "modelo": "Honda CB 300F",
       "placa": "ABC1D23"
     }'
```

#### Teste 6: Testar sem API Key (deve retornar 401)
```bash
curl http://localhost:5020/api/v1/clientes
```
**Resposta esperada:**
```
API Key não fornecida. Por favor, inclua o header X-API-KEY.
```

#### Teste 7: Testar com API Key inválida
```bash
curl -H "X-API-KEY: chave-errada" \
     http://localhost:5020/api/v1/clientes
```
**Resposta esperada:**
```
API Key inválida.
```

### 3️⃣ Endpoints que Funcionam SEM Banco de Dados

✅ **POST /api/v1/clientes/estimar-risco** - ML.NET (funciona!)  
✅ **GET /health** - Health Check (funciona, mas pode mostrar Unhealthy)  
✅ **GET /swagger** - Documentação (funciona!)

### 4️⃣ Endpoints que PRECISAM do Banco de Dados

⚠️ Estes endpoints precisam de conexão com Oracle:
- `GET /api/v1/clientes` - Listar clientes
- `POST /api/v1/clientes` - Criar cliente
- `GET /api/v1/motos` - Listar motos
- `POST /api/v1/motos` - Criar moto
- `GET /api/v1/locacoes` - Listar locações
- `POST /api/v1/locacoes` - Criar locação

**Para testar estes endpoints:**
1. Configure a conexão Oracle no `appsettings.json`
2. Certifique-se de que o Oracle está acessível
3. Execute as migrations: `dotnet ef database update`

### 📝 Exemplos de Respostas

#### ✅ Sucesso (200 OK)
```json
{
  "items": [...],
  "totalCount": 10,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 1
}
```

#### ❌ Erro 401 (Unauthorized - API Key inválida)
```
API Key inválida.
```

#### ❌ Erro 400 (Bad Request - Dados inválidos)
```json
{
  "message": "Cliente não possui CNH do tipo 'A' ou 'AB'."
}
```

#### ❌ Erro 404 (Not Found)
```
Not Found
```

---

## 🧪 Testes Automatizados

O projeto foi desenhado para ser altamente testável, isolando a lógica de negócio em serviços.  
O projeto de testes utiliza **xUnit**, **Moq** para mocks e **Microsoft.AspNetCore.Mvc.Testing** para testes de integração.

### Estrutura de Testes

- **Testes Unitários** (`MottuApi.Tests/Services/`):
  - Testam a lógica de negócio dos serviços isoladamente
  - Utilizam Moq para simular dependências (repositórios, mappers)
  - Exemplos: `LocacaoServiceTests.cs`

- **Testes de Integração** (`MottuApi.Tests/Integration/`):
  - Testam os controllers de ponta a ponta
  - Utilizam banco de dados em memória (InMemory)
  - Fazem chamadas HTTP reais aos endpoints
  - Exemplos: `ClientesControllerTests.cs`, `LocacoesControllerTests.cs`, `HealthCheckTests.cs`

### Executar os Testes

**Compilar o projeto de testes:**
```bash
dotnet build MottuApi.Tests/MottuApi.Tests.csproj
```

**Rodar todos os testes:**
```bash
dotnet test
```

**Rodar apenas testes unitários:**
```bash
dotnet test --filter "FullyQualifiedName~Services"
```

**Rodar apenas testes de integração:**
```bash
dotnet test --filter "FullyQualifiedName~Integration"
```

**Rodar com cobertura de código (requer `coverlet.msbuild`):**
```bash
dotnet test /p:CollectCoverage=true
```

### Exemplos de Testes Implementados

**Testes Unitários:**
- ✅ Validação de CNH tipo B (deve bloquear)
- ✅ Validação de cliente com locação ativa (deve bloquear)
- ✅ Cálculo de multa por atraso
- ✅ Bloqueio de deleção de locação ativa

**Testes de Integração:**
- ✅ Criação de cliente com dados válidos (retorna 201)
- ✅ Requisição sem API Key (retorna 401)
- ✅ Busca de cliente inexistente (retorna 404)
- ✅ Health Check sem API Key (retorna 200)

---

## 🔍 Verificar se Está Funcionando

1. **Health Check deve retornar 200**:
   ```bash
   curl http://localhost:5020/health
   ```

2. **Swagger deve abrir sem erros**:
   - Acesse: `http://localhost:5020/swagger`
   - Deve ver a interface do Swagger com todos os endpoints

3. **API Key deve funcionar**:
   - Tente acessar um endpoint sem API Key → deve retornar 401
   - Tente com API Key → deve funcionar

4. **ML.NET deve funcionar**:
   ```bash
   curl -X POST http://localhost:5020/api/v1/clientes/estimar-risco \
        -H "Content-Type: application/json" \
        -H "X-API-KEY: MottuApi-Secret-Key-2024-Development" \
        -d '{"idade": 25, "tipoCNH": "A"}'
   ```
   Deve retornar: `{"risco":"Baixo","idade":25,"tipoCNH":"A"}`

---

## 🐛 Solução de Problemas

### Erro: "Cannot connect to database"
- Verifique a string de conexão no `appsettings.json`
- Certifique-se de que o Oracle está acessível
- **Nota**: Alguns endpoints funcionam sem banco (ML.NET, Health Check)

### Erro: "API Key não configurada"
- Verifique se o `appsettings.json` tem a seção `ApiKey:SecretKey`

### Erro ao treinar modelo ML.NET
- Certifique-se de que o arquivo `Data/dados_treino.csv` existe
- O modelo será criado automaticamente na primeira execução

### Porta já em uso
- Altere a porta no `Properties/launchSettings.json`
- Ou pare o processo que está usando a porta 5020

### Erros de compilação nos testes
- Execute: `dotnet build MottuApi.Tests/MottuApi.Tests.csproj`
- Verifique se todos os pacotes NuGet estão instalados

---

## 📚 Próximos Passos

1. ✅ **Teste o ML.NET** - Já funciona sem banco!
2. ✅ **Use o Swagger** - Interface visual para testar tudo
3. ✅ **Teste via cURL/Postman** - Para integração contínua
4. ✅ **Execute os testes automatizados** - `dotnet test`
5. ⚠️ **Configure o Oracle** - Para testar os endpoints de CRUD
6. ✅ **Verifique os logs** - Mantenha `dotnet run` aberto para ver requisições

---

## 💡 Dicas

- **Mantenha o terminal com `dotnet run` aberto** para ver os logs em tempo real
- **Use o Swagger** para explorar todos os endpoints de forma visual
- **O endpoint de ML.NET funciona sem banco** - perfeito para testes rápidos!
- **Health Check pode mostrar "Unhealthy"** se o Oracle não estiver acessível, mas isso não impede o uso de outros endpoints

---

## 📄 Licença

Este projeto foi desenvolvido para fins educacionais.

---

**Desenvolvido com ❤️ usando ASP.NET Core 8**
