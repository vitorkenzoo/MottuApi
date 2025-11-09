using Microsoft.ML;
using Microsoft.ML.Data;
using MottuApi.Core.Interfaces;
using System.Reflection;

namespace MottuApi.Services
{
    /// <summary>
    /// Modelo de dados para entrada do ML.NET
    /// </summary>
    public class ClienteInput
    {
        [LoadColumn(0)]
        public float Idade { get; set; }

        [LoadColumn(1)]
        public string TipoCNH { get; set; } = string.Empty;

        [LoadColumn(2)]
        public string Risco { get; set; } = string.Empty;
    }

    /// <summary>
    /// Modelo de dados para predição
    /// </summary>
    public class ClientePrediction
    {
        [ColumnName("PredictedLabel")]
        public string Risco { get; set; } = string.Empty;
    }

    /// <summary>
    /// Serviço de predição de risco de clientes usando ML.NET
    /// </summary>
    public class PredictionService : IPredictionService
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _model;
        private readonly PredictionEngine<ClienteInput, ClientePrediction> _predictionEngine;

        public PredictionService()
        {
            _mlContext = new MLContext(seed: 0);

            // Caminho para o modelo treinado
            var modelPath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "",
                "Model.zip"
            );

            // Se o modelo não existir, treina um novo
            if (!File.Exists(modelPath))
            {
                TreinarModelo(modelPath);
            }

            // Carrega o modelo
            _model = _mlContext.Model.Load(modelPath, out _);
            _predictionEngine = _mlContext.Model.CreatePredictionEngine<ClienteInput, ClientePrediction>(_model);
        }

        public string EstimarRisco(int idade, string tipoCNH)
        {
            var input = new ClienteInput
            {
                Idade = idade,
                TipoCNH = tipoCNH
            };

            var prediction = _predictionEngine.Predict(input);
            return prediction.Risco;
        }

        private void TreinarModelo(string modelPath)
        {
            // Caminho para o arquivo CSV de treino
            // Tenta primeiro o diretório de execução, depois o diretório raiz do projeto
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
            var csvPath = Path.Combine(basePath, "Data", "dados_treino.csv");

            // Se não encontrar, tenta o diretório raiz do projeto
            if (!File.Exists(csvPath))
            {
                var projectRoot = Directory.GetCurrentDirectory();
                csvPath = Path.Combine(projectRoot, "Data", "dados_treino.csv");
            }

            // Se ainda não encontrar, tenta caminho relativo ao bin
            if (!File.Exists(csvPath))
            {
                var binPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
                var projectPath = Path.GetFullPath(Path.Combine(binPath, "..", "..", "..", ".."));
                csvPath = Path.Combine(projectPath, "Data", "dados_treino.csv");
            }

            if (!File.Exists(csvPath))
            {
                throw new FileNotFoundException($"Arquivo de treino não encontrado. Tentou: {csvPath}");
            }

            // Carrega os dados do CSV
            var dataView = _mlContext.Data.LoadFromTextFile<ClienteInput>(
                csvPath,
                separatorChar: ',',
                hasHeader: true
            );

            // Divide os dados em treino e teste (80/20)
            var dataSplit = _mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);

            // Pipeline de transformação e treinamento
            var pipeline = _mlContext.Transforms.Conversion.MapValueToKey("Label", "Risco")
                .Append(_mlContext.Transforms.Categorical.OneHotEncoding("TipoCNHEncoded", "TipoCNH"))
                .Append(_mlContext.Transforms.Concatenate("Features", "Idade", "TipoCNHEncoded"))
                .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
                .Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy())
                .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            // Treina o modelo
            var trainedModel = pipeline.Fit(dataSplit.TrainSet);

            // Salva o modelo
            var directory = Path.GetDirectoryName(modelPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            _mlContext.Model.Save(trainedModel, dataSplit.TrainSet.Schema, modelPath);
        }
    }
}

