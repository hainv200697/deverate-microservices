using AuthenServices.Model;
using AuthenServices.Models;
using AuthenServices.Service;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenServices.RabbitMQ
{
    public class Consumer : BackgroundService
    {
        private IConnection connection;
        private IModel channel;
        private string queueName;
        private string exch;
        DeverateContext context;
        public Consumer(DeverateContext context, string exch)
        {
            this.exch = exch;
            this.context = context;
            InitRabbitMQ();

        }
        private void InitRabbitMQ()
        {
            var factory = new ConnectionFactory() { HostName = AppConstrain.hostname };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: this.exch, type: ExchangeType.Fanout);

            queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName,
                              exchange: this.exch,
                              routingKey: "");
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Receive {0}", message);
                Producer producer = new Producer();
                switch (this.exch)
                {
                    case "ResendPassword":
                        List<string> listSendPass = JsonConvert.DeserializeObject<List<string>>(message);
                        var listResendAccount = AccountDAO.Resend(listSendPass);
                        foreach (MessageAccountDTO msAccount in listResendAccount)
                        {
                            producer.PublishMessage(JsonConvert.SerializeObject(msAccount), "AccountToEmail");
                        }
                        break;
                }
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: false,
                                 consumer: consumer);
            return Task.CompletedTask;
        }
    }
}