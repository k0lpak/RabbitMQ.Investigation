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
        private const byte PersistentMode = 2;
        private const string Queue = "task_queue";
        
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(Queue, true, false, false, null);
                    string message;
                    while ((message = Console.ReadLine()) != string.Empty)
                    {
                        var body = Encoding.UTF8.GetBytes(message);
                        var properties = channel.CreateBasicProperties();
                        properties.DeliveryMode = PersistentMode;

                        channel.BasicPublish("", Queue, properties, body);
                    }
                }
            }
        }
    }
}
