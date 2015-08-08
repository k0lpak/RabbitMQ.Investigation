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
                    chanel.ExchangeDeclare(Exchange, "topic");
                    Console.WriteLine("Enter comma separated match patterns (*.high,app.#)> ");
                    var patterns = Console.ReadLine().Split(',');
                    var queue = chanel.QueueDeclare().QueueName;
                    foreach (var pattern in patterns)
                    {
                        chanel.QueueBind(queue, Exchange, pattern);
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
            Console.WriteLine("Message: '{0}' Key: {1}", message, e.RoutingKey);
        }
    }
}
