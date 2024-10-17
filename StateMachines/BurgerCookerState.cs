namespace Company.StateMachines
{
    using System;
    using MassTransit;

    public class BurgerCookerState : SagaStateMachineInstance
    {
        // CorrelationId để liên kết các sự kiện với instance của Saga
        public Guid CorrelationId { get; set; }

        // Trạng thái hiện tại của Saga
        public int CurrentState { get; set; }

        // Các thuộc tính liên quan đến quy trình cụ thể
        public string CustomerName { get; set; }
        public string CookTemp { get; set; }
        public string Value { get; set; }

        // Các dữ liệu hoặc thuộc tính bổ sung khác
        public DateTime OrderDate { get; set; }
        public Guid OrderId { get; set; }
    }
}