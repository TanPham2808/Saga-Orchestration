namespace Contracts
{
    using System;

    public record BurgerCookerFinishedCookingEvent
    {
        public Guid CorrelationId { get; set; }
    }
}