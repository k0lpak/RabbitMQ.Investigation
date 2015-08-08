using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producer
{
    class Program
    {
        private const string Exchange = "logs";

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(Exchange, "fanout", false);
                    string message;
                    while (!string.IsNullOrEmpty(message = Console.ReadLine()))
                    {
                        channel.BasicPublish(Exchange, "", null, Encoding.UTF8.GetBytes(message));
                        Console.WriteLine("Message was sent: {0}", message);
                    }
                }
            }
        }
    }
}
