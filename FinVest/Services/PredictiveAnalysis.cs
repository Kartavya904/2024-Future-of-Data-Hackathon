using Microsoft.ML;
using Microsoft.ML.Data;
using FinVest.Models;
using System.IO;
using System.Linq;

namespace FinVest.Services
{
    public class PredictiveAnalysisService
    {
        private readonly MLContext _mlContext;
        private ITransformer _model;

        public PredictiveAnalysisService()
        {
            _mlContext = new MLContext();
        }

        public void TrainModel(string dataPath)
        {
            // Preprocess the CSV file to remove $ symbols
            PreprocessData(dataPath);

            // Load the data from the CSV file
            IDataView dataView = _mlContext.Data.LoadFromTextFile<StockDataModel>(dataPath, hasHeader: true, separatorChar: ',');

            // Define the pipeline
            var pipeline = _mlContext.Transforms.Concatenate("Features", nameof(StockDataModel.Open), nameof(StockDataModel.High), nameof(StockDataModel.Low), nameof(StockDataModel.Volume))
                            .Append(_mlContext.Regression.Trainers.Sdca(labelColumnName: nameof(StockDataModel.Close), featureColumnName: "Features"));

            // Train the model
            _model = pipeline.Fit(dataView);
        }

        private void PreprocessData(string dataPath)
        {
            var lines = File.ReadAllLines(dataPath);
            var preprocessedLines = lines.Select((line, index) =>
            {
                if (index == 0) return line; // Keep header
                var columns = line.Split(',');
                for (int i = 1; i <= 5; i++) // Columns that need $ removal (Close, Volume, Open, High, Low)
                {
                    columns[i] = columns[i].Replace("$", "").Trim();
                }
                return string.Join(",", columns);
            });
            File.WriteAllLines(dataPath, preprocessedLines);
        }

        public float Predict(StockDataModel input)
        {
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<StockDataModel, StockPredictionModel>(_model);
            var prediction = predictionEngine.Predict(input);
            return prediction.PredictedClose;
        }
    }
}
