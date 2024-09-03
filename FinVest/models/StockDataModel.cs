using Microsoft.ML.Data;

namespace FinVest.Models
{
    public class StockDataModel
    {
        [LoadColumn(3)] // Open corresponds to the 4th column
        public float Open { get; set; }

        [LoadColumn(4)] // High corresponds to the 5th column
        public float High { get; set; }

        [LoadColumn(5)] // Low corresponds to the 6th column
        public float Low { get; set; }

        [LoadColumn(1)] // Close corresponds to the 2nd column
        [ColumnName("Label")] // This is the value we want to predict
        public float Close { get; set; }

        [LoadColumn(2)] // Volume corresponds to the 3rd column
        public float Volume { get; set; }
    }
}
