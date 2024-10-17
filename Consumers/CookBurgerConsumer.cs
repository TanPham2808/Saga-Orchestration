namespace Company.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;
    using System.Threading;
    using System;

    public class CookBurgerConsumer : IConsumer<CookBurger>
    {
        public Task Consume(ConsumeContext<CookBurger> context)
        {
            try
            {
                if (context.Message.CookTemp.Equals("Rare"))
                {
                    Thread.Sleep(2000);
                }
                else if (context.Message.CookTemp.Equals("Medium"))
                {
                    //throw new Exception("Logfile cannot be read-only");
                    Thread.Sleep(5000);
                }
                else if (context.Message.CookTemp.Equals("Burned"))
                {
                    Thread.Sleep(10000);
                }

                BurgerCookerFinishedCookingEvent burgerFinishCookingEvent = new BurgerCookerFinishedCookingEvent();
                burgerFinishCookingEvent.CorrelationId = context.Message.CorrelationId;

                // Bắn thực hiện Event Finish (kết thúc nấu ăn)
                context.Publish(burgerFinishCookingEvent); 
            } 
            catch (Exception ex)
            {
                context.Publish(new BurgerCookerFailCookingEvent
                {
                    CorrelationId = context.Message.CorrelationId,
                    ErrorDescreption = ex.Message
                });
            }
            return Task.CompletedTask;
        }
    }
}