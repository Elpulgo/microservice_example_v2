using System;

namespace Shared.Core.Models
{
    public class Entity
    {
        public readonly string TableName;

        public Entity(string tableName)
        {
            Id = Guid.NewGuid();
            TableName = tableName;
        }

        public Guid Id { get; private set; }
    }
}