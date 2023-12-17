namespace Stas.Monitor.Domains
{
    public record Configuration
    {
        public IList<ThermometerConfiguration> Thermometers { get; init; }

        public Configuration(List<ThermometerConfiguration> thermometers)
        {
            this.Thermometers = thermometers;
        }
    }
}
