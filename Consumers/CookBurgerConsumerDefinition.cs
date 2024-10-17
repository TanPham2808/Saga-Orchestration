namespace Company.Consumers
{
    using MassTransit;

    public class CookBurgerConsumerDefinition :
        ConsumerDefinition<CookBurgerConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CookBurgerConsumer> consumerConfigurator, IRegistrationContext context)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));

            endpointConfigurator.UseInMemoryOutbox(context);
        }
    }
}