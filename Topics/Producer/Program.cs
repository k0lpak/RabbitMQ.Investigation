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
        private const string Exchange = "routing_exchange",
            AskMessageText = "Enter message > ",
            AskForPriorityText = "Enter app.priority (Example: myapp.high) > ";

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(Exchange, "topic");
                    string message = PromptForString(AskMessageText);
                    string priority = PromptForString(AskForPriorityText);

                    while (!string.IsNullOrEmpty(message) && !string.IsNullOrEmpty(priority))
                    {
                        channel.BasicPublish(Exchange, priority, null, Encoding.UTF8.GetBytes(message));
                        Console.WriteLine("Emit message '{0}' with key {1}", message, priority);

                        message = PromptForString(AskMessageText);
                        priority = PromptForString(AskForPriorityText);
                    }
                }
            }
        }

        private static string PromptForString(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }
    }
}
