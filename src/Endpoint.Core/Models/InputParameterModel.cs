namespace Endpoint.Core.Models
{
    public class InputParameterModel
    {
        public string Type { get; private set; }
        public string Name { get; private set; }

        public InputParameterModel(string type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}
