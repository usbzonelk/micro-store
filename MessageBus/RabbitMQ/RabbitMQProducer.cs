using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;

namespace MessageBus.RabitMQ
{
    public class RabitMQProducer: IRabitMQProducer {
        public void SendProductMessage < T > (T message) {
            var factory = new ConnectionFactory {
            HostName = "localhost",
            };
            
            var connection = factory.CreateConnection();

            using
            var channel = connection.CreateModel();
            channel.QueueDeclare("email", exclusive: false);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: "newEmail", body: body);
        }
    }
}