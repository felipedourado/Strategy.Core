using MongoDB.Bson.Serialization.Attributes;
using Strategy.Core.Domain.Base;

namespace Strategy.Core.Domain.Entities
{
    [MongoCustomAttribute("digitalAccountCollection")]
    public class DigitalAccountEntity : MongoDocumentBase
    {
        [BsonElement("agency")]
        public int Agency { get; set; }
        [BsonElement("account")]
        public int Account { get; set; }
        [BsonElement("product")]
        public string? Product { get; set; }
        [BsonElement("name")]
        public string? Name { get; set; }
    }
}
