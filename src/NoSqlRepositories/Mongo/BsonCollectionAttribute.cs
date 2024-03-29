using System;

namespace NoSqlRepositories.Mongo
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BsonCollectionAttribute : Attribute
    {
        public BsonCollectionAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }

        public string CollectionName { get; }
    }
}