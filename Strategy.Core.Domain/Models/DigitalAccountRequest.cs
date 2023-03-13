namespace Strategy.Core.Domain.Models
{
    public class DigitalAccountRequest : AccountBase
    {
        public string Product { get; set; }
        public string Name { get; set; }

    }
}
