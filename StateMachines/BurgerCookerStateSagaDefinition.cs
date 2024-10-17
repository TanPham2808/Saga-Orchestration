namespace Company.StateMachines
{
    using MassTransit;

    public class BurgerCookerStateSagaDefinition :
        SagaDefinition<BurgerCookerState>
    {
        protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<BurgerCookerState> sagaConfigurator, IRegistrationContext context)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));

            endpointConfigurator.UseInMemoryOutbox(context);
        }
    }
}