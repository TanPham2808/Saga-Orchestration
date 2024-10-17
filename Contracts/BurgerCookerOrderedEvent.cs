namespace Contracts
{
    using System;

    public record BurgerCookerOrderedEvent
    {
        public Guid CorrelationId { get; init; }
        public string CustomerName { get; init; }

        public string CookTemp { get; init; }
    }
}