using MongoDB.Bson;

namespace Strategy.Core.Domain.Base
{
    public interface IMongoDocumentBase
    {
        ObjectId Id { get; set; }
        DateTime CreateDate { get; }
        DateTime UpdateDate { get; set; }
    }
}
