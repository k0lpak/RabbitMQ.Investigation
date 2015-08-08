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
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("hello", false, false, false, null);
                    while (Console.ReadKey().KeyChar != 'q')
                    {
                        var message = string.Format("Date on producer is {0}", DateTime.Now);
                        channel.BasicPublish("", "hello", null, Encoding.UTF8.GetBytes(message));
                        Console.WriteLine("Message '{0}' was sent.", message);
                    }
                }
            }
        }
    }
}
