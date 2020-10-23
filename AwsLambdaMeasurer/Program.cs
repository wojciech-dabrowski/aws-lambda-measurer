using System.Threading.Tasks;
using Amazon.Lambda;
using Amazon.Runtime.CredentialManagement;

namespace AwsLambdaMeasurer
{
    public static class Program
    {
        private static async Task Main(string[] args)
        {
            var config = new ConfigurationReader().ReadConfig();
            var profileSource = new SharedCredentialsFile();
            profileSource.TryGetProfile(config.ProfileName, out var credentialProfile);
            var credentials = credentialProfile.GetAWSCredentials(profileSource);
            var lambdaClient = new AmazonLambdaClient(credentials);
            var logger = new ConsoleLogger();
            var measurer = new MeasureService(config, lambdaClient, logger);
            var reportFactory = new ReportFactory();
            var reportWriter = new ReportWriter();

            var measurementsSummary = await measurer.PerformMeasurements();
            var csvReport = reportFactory.CreateCsvReport(measurementsSummary);
            await reportWriter.SaveReport(csvReport);
        }
    }
}