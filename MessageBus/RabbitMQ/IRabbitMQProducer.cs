namespace MessageBus.RabitMQ
{
    interface IRabitMQProducer
    {
        void SendProductMessage<T>(T message);
    }
}