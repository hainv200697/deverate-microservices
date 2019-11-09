using System;
using System.Text;
using RabbitMQ.Client;
using ResourceServices.Model;

namespace ResourceServices.RabbitMQ
{
    public class Producer
    {
        public void PublishMessage(string message, string exch)
        {
            var factory = new ConnectionFactory() { HostName = AppConstrain.hostname };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: exch, type: ExchangeType.Fanout);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: exch,
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }
        }
    }
}
