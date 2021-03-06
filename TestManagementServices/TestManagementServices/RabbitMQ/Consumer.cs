﻿
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestManagementServices.Model;
using TestManagementServices.Models;
using TestManagementServices.Service;

namespace TestManagementServices.RabbitMQ
{
    public class Consumer : BackgroundService
    {
        private IConnection connection;
        private IModel channel;
        private string queueName;
        private string exch;
        public Consumer(string exch)
        {
            InitRabbitMQ(exch);
        }
        private void InitRabbitMQ(string exch)
        {
            this.exch = exch;
            var factory = new ConnectionFactory() { HostName = AppConstrain.HOSTNAME };

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
                Console.WriteLine(" [x] Receive {0}", message);
                switch (this.exch)
                {
                    case AppConstrain.GEN_TEST_CONSUMER:
                        EmployeeTestDTO employeeTest = JsonConvert.DeserializeObject<EmployeeTestDTO>(message);
                        SystemDAO.GenerateTest(employeeTest.accountIds, employeeTest.configId, employeeTest.startDate, employeeTest.endDate, employeeTest.oneForAll);
                        break;
                    case AppConstrain.GEN_TEST_APPLICANT:
                        ApplicantTestDTO applicantTest = JsonConvert.DeserializeObject<ApplicantTestDTO>(message);
                        SystemDAO.GenerateTestForApplicants(applicantTest.configId, applicantTest.applicants, applicantTest.startDate, applicantTest.endDate);
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
