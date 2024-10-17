namespace Company.StateMachines
{
    using Contracts;
    using MassTransit;
    using System;

    public class BurgerCookerStateMachine : MassTransitStateMachine<BurgerCookerState>
    {
        // Trạng thái
        public State Ordered { get; private set; }
        public State BeginCooking { get; private set; }
        public State FinishCooking { get; private set; }
        public State Completed { get; private set; }
        public State Fail { get; private set; }


        // Khai báo các event sử dụng cho Saga
        public Event<BurgerCookerOrderedEvent> BurgerCookerOrderedEvent { get; private set; }
        public Event<BurgerCookerBeginCookingEvent> BurgerCookerBeginCookingEvent { get; private set; }
        public Event<BurgerCookerFinishedCookingEvent> BurgerCookerFinishedCookingEvent { get; private set; }
        public Event<BurgerCookerFailCookingEvent> BurgerCookerFailCookingEvent { get; private set; }

        public BurgerCookerStateMachine()
        {
            InstanceState(x => x.CurrentState, Ordered);

            // Khởi tạo event
            Event(() => BurgerCookerOrderedEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => BurgerCookerBeginCookingEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => BurgerCookerFailCookingEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => BurgerCookerFinishedCookingEvent, x => x.CorrelateById(context => context.Message.CorrelationId));

            Initially(
                // Quy trình nhận đơn đặt hàng
                When(BurgerCookerOrderedEvent) 
                    .Then(context =>
                    {
                        context.Saga.CustomerName = context.Message.CustomerName;
                        context.Saga.CookTemp = context.Message.CookTemp;

                        BurgerCookerBeginCookingEvent burgerBeginCookingEvent = new BurgerCookerBeginCookingEvent();
                        burgerBeginCookingEvent.CorrelationId = context.Saga.CorrelationId;
                        burgerBeginCookingEvent.CookTemp = context.Saga.CookTemp;

                        context.Publish(burgerBeginCookingEvent);
                    })
                    .TransitionTo(BeginCooking) // Trạng thái tiếp theo
            );

            // Quy trình bắt đầu nấu nấu ăn
            During(BeginCooking,  
                When(BurgerCookerBeginCookingEvent)
                    .Then(context =>
                    {
                        CookBurger cb = new CookBurger();
                        cb.CorrelationId = context.Saga.CorrelationId;
                        cb.CookTemp = context.Saga.CookTemp;  // Nhiệt độ trung bình (Medium)
                        context.Publish(cb);
                    })
                    .TransitionTo(FinishCooking) 
            );

            // Hoàn tất nấu ăn
            During(FinishCooking,  
                When(BurgerCookerFinishedCookingEvent)
                    .Then(context =>
                    {
                        System.Console.WriteLine("Order up for: " + context.Saga.CustomerName + " Id: " + context.Saga.CorrelationId + " Cook Temp: " + context.Saga.CookTemp);
                    })
                    .TransitionTo(Completed)
            );

            // Xử lý sự kiện lỗi trong FinishCooking
            During(FinishCooking,
                When(BurgerCookerFailCookingEvent)
                    .Then(context =>
                    {
                        // Xử lý lỗi hoặc rollback các thay đổi nếu cần
                        System.Console.WriteLine($"Failed to finish cooking. Id:{context.Saga.CorrelationId}. Error: {context.Message.ErrorDescreption} ");
                    })
                    .TransitionTo(Fail) // Chuyển sang trạng thái Failed nếu có lỗi
            );

            SetCompletedWhenFinalized();
        }


    }
}