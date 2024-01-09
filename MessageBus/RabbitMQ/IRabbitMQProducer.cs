namespace MessageBus.RabittMQ
{
    public interface IRabitMQProducer
    {
        void SendEmail<T>(T message);
    }
}