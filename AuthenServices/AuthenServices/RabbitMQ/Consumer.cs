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
        DeverateContext context;
        public Consumer(DeverateContext context, string exch)
        {
            InitRabbitMQ(exch);
            this.context = context;
        }
        private void InitRabbitMQ(string exch)
        {
            var factory = new ConnectionFactory() { HostName = "35.240.253.45" };

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
                var messageAccount = JsonConvert.DeserializeObject<MessageAccount>(message);
                var result = AccountDAO.GenerateCompanyAccount(context, messageAccount).Split('_');
                Producer producer = new Producer();
                MessageAccountDTO messageDTO = new MessageAccountDTO(result[0], result[1], messageAccount.Email, messageAccount.Fullname);
                producer.PublishMessage(message: JsonConvert.SerializeObject(messageDTO), "AccountToEmail");
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: false,
                                 consumer: consumer);
            return Task.CompletedTask;
        }
    }
}