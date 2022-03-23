namespace Endpoint.Core.Options
{
    public class CreateMinimalApiProjectOptions
    {
        public int? Port { get; set; }
        public string Properties { get; set; }
        public string Name { get; set; }
        public string Resource { get; set; }
        public string DbContextName { get; set; }
        public bool? ShortIdPropertyName{ get; set; }
        public bool? NumericIdPropertyDataType{ get; set; }
        public string Directory { get; set; }
    }
}
