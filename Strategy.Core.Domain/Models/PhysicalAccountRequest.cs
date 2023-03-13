namespace Strategy.Core.Domain.Models
{
    public class PhysicalAccountRequest : AccountBase
    {
        public string Street { get; set; }
        public string Manager { get; set; }
    }
}
