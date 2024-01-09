using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;

namespace MessageBus.RabittMQ
{
    public class RabitMQProducer : IRabitMQProducer
    {
        public void SendEmail<T>(T message)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            var connection = factory.CreateConnection();

            using
            var channel = connection.CreateModel();

            channel.ExchangeDeclare("EmailExchange", ExchangeType.Direct);
            channel.QueueDeclare(queue: "email", exclusive: false);
            channel.QueueBind("email", "EmailExchange", routingKey: "key-route", arguments: null);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "EmailExchange", routingKey: "key-route", basicProperties: null, body: body);

            channel.Close();
            connection.Close();
        }
    }
}