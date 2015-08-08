using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer
{
    class Program
    {
        private const string Exchange = "routing_exchange";
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var chanel = connection.CreateModel())
                {
                    chanel.ExchangeDeclare(Exchange, "direct");
                    Console.WriteLine("Enter comma separated priorities (low,medium,high)> ");
                    var priorities = Console.ReadLine().Split(',');
                    var queue = chanel.QueueDeclare().QueueName;
                    foreach (var priority in priorities)
                    {
                        chanel.QueueBind(queue, Exchange, priority);
                    }

                    var consumer = new EventingBasicConsumer(chanel);
                    consumer.Received += Consumer_Received;
                    chanel.BasicConsume(queue, true, consumer);
                    Console.ReadLine();
                }
            }
        }

        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body);
            Console.WriteLine("Message: '{0}' Priority: {1}", message, e.RoutingKey);
        }
    }
}
