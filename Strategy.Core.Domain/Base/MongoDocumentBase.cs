using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Strategy.Core.Domain.Base
{
    public class MongoDocumentBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public ObjectId Id { get; set; }

        [BsonElement("createDate")]
        [BsonDateTimeOptions(Representation = BsonType.DateTime)]
        public DateTime CreateDate => Id.CreationTime.ToUniversalTime();

        [BsonElement("updateDate")]
        [BsonDateTimeOptions(Representation = BsonType.DateTime)]
        public DateTime UpdateDate { get; set; }

    }
}
