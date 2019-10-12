using AuthenServices.Model;
using MailingServices.Models;
using MailingServices.Service;
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

namespace MailingServices.RabbitMQ
{
    public class Consumer : BackgroundService
    {
        private IConnection connection;
        private IModel channel;
        private string queueName;
        private string exch;
        public Consumer(string exch)
        {
            this.exch = exch;
            InitRabbitMQ(exch);
        }
        private void InitRabbitMQ(string exch)
        {
            var factory = new ConnectionFactory() { HostName = "35.240.253.45" }; ;

            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: exch, type: ExchangeType.Fanout);

            queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName,
                              exchange: exch,
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
                switch (this.exch)
                {
                    case "AccountToEmail":
                        var messageAccountDTO = JsonConvert.DeserializeObject<MessageAccountDTO>(message);
                        EmailSender.SendMailAsync(messageAccountDTO.Email, "Welcome To DEVERATE System", "Username: " + messageAccountDTO.Username + ", Password: " + messageAccountDTO.Password);
                        break;
                    case "TestToEmail":
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
