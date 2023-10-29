namespace Stas.Monitor.Domains
{
    public class Configuration
    {
        public List<ThermometerConfiguration> Thermometers { get; set; } = new List<ThermometerConfiguration>();
        public string MeasurementFilePath {get; set; }
        public string AlertFilePath { get; set; }
        public Configuration(string csvFilePath, List<ThermometerConfiguration> thermometers)
        {
            this.MeasurementFilePath = csvFilePath;
            
            this.Thermometers = thermometers ?? new List<ThermometerConfiguration>();
        }
    }
}