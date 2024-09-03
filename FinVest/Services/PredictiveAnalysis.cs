using Microsoft.ML;
using Microsoft.ML.Data;
using FinVest.Models;

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
            // Load the data from the CSV file
            IDataView dataView = _mlContext.Data.LoadFromTextFile<StockDataModel>(dataPath, hasHeader: true, separatorChar: ',');

            // Define the pipeline
            var pipeline = _mlContext.Transforms.Concatenate("Features", nameof(StockDataModel.Open), nameof(StockDataModel.High), nameof(StockDataModel.Low), nameof(StockDataModel.Volume))
                            .Append(_mlContext.Regression.Trainers.Sdca(labelColumnName: nameof(StockDataModel.Close), featureColumnName: "Features"));

            // Train the model
            _model = pipeline.Fit(dataView);
        }

        public float Predict(StockDataModel input)
        {
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<StockDataModel, StockPredictionModel>(_model);
            var prediction = predictionEngine.Predict(input);
            return prediction.PredictedClose;
        }
    }
}
