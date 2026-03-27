using System.Diagnostics.Metrics;

namespace ProGraTutorial.Models
{
    public class AppCustomMeter
    {
        public static string name = "CustomMeter";

        private readonly Counter<int> productCounter;
        
        public AppCustomMeter(IMeterFactory meterFactory)
        {
            var meter = meterFactory.Create(name);
            productCounter = meter.CreateCounter<int>("Product.Sold");
        }
        public void ProductSold(string productName, int quantity)
        {
            productCounter.Add(quantity, new KeyValuePair<string, object?>("Product.Name", productName));
        }
    }
}
