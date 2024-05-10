using System;

namespace Knights.Challenge.Domain.Entities
{
    public abstract class IdentifierEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

    }
}
