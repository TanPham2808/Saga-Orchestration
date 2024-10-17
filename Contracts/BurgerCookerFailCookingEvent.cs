namespace Contracts
{
    using System;

    public record BurgerCookerFailCookingEvent
    {
        public Guid CorrelationId { get; set; }
        public string ErrorDescreption { get; set; }
    }
}