using System;

namespace Contracts
{
    public record CookBurger
    {
        public string CookTemp { get; set; }
        public Guid CorrelationId { get; set; }
    }
}