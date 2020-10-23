using System.Threading.Tasks;

namespace AwsLambdaMeasurer
{
    public static class Program
    {
        private static async Task Main(string[] args)
        {
            var config = new ConfigReader().ReadConfig();
            var measurer = new MeasureService(config);
            var reportFactory = new ReportFactory();
            var reportWriter = new ReportWriter();

            var measurementsSummary = await measurer.PerformMeasurements();
            var csvReport = reportFactory.CreateCsvReport(measurementsSummary);
            await reportWriter.SaveReport(csvReport);
        }
    }
}