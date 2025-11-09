#!/bin/bash

# Script para testar a API Mottu
# Uso: ./test-api.sh

API_URL="http://localhost:5020"
API_KEY="MottuApi-Secret-Key-2024-Development"

echo "🧪 Testando API Mottu"
echo "===================="
echo ""

# Teste 1: Health Check
echo "1️⃣  Testando Health Check (sem API Key)..."
curl -s "$API_URL/health" | jq '.' || echo "❌ Health Check falhou"
echo ""
echo ""

# Teste 2: Listar Clientes (sem API Key - deve falhar)
echo "2️⃣  Testando sem API Key (deve retornar 401)..."
curl -s -w "\nStatus: %{http_code}\n" "$API_URL/api/v1/clientes" || echo "❌ Teste falhou"
echo ""
echo ""

# Teste 3: Listar Clientes (com API Key)
echo "3️⃣  Testando listar clientes (com API Key)..."
curl -s -H "X-API-KEY: $API_KEY" "$API_URL/api/v1/clientes" | jq '.' || echo "❌ Teste falhou"
echo ""
echo ""

# Teste 4: Criar Cliente
echo "4️⃣  Testando criar cliente..."
curl -s -X POST "$API_URL/api/v1/clientes" \
  -H "Content-Type: application/json" \
  -H "X-API-KEY: $API_KEY" \
  -d '{
    "nome": "João Silva Teste",
    "cpf": "11122233344",
    "dataNascimento": "1990-05-20",
    "numeroCNH": "99988877766",
    "tipoCNH": "A"
  }' | jq '.' || echo "❌ Teste falhou"
echo ""
echo ""

# Teste 5: Estimar Risco (ML.NET)
echo "5️⃣  Testando estimar risco (ML.NET)..."
curl -s -X POST "$API_URL/api/v1/clientes/estimar-risco" \
  -H "Content-Type: application/json" \
  -H "X-API-KEY: $API_KEY" \
  -d '{
    "idade": 25,
    "tipoCNH": "A"
  }' | jq '.' || echo "❌ Teste falhou"
echo ""
echo ""

echo "✅ Testes concluídos!"
echo ""
echo "💡 Dica: Acesse http://localhost:5020/swagger para ver a documentação completa"


