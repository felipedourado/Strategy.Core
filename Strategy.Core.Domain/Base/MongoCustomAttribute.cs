namespace Strategy.Core.Domain.Base
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class MongoCustomAttribute : Attribute
    {
        public string CollectionName { get; }

        public MongoCustomAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }
    }
}
