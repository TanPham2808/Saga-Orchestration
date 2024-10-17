namespace Contracts
{
    using System;

    public record BurgerCookerBeginCookingEvent
    {
        public Guid CorrelationId { get; set; }
        public string CustomerName { get; init; }
        public string CookTemp { get; set; }
    }
}