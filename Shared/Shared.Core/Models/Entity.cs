using System;

namespace Shared.Core.Models
{
    public class Entity
    {
        public readonly string TableName;

        public Entity(string tableName)
        {
            TableName = tableName;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
    }
}