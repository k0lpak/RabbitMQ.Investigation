using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Consumer
{
    class Program
    {
        private const string Queue = "task_queue";
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(Queue, true, false, false, null);
                    channel.BasicQos(0, 1, false);
                    var eventHandler = new EventingBasicConsumer(channel);
                    eventHandler.Received += EventHandler_Received;
                    channel.BasicConsume(Queue, false, eventHandler);
                    Console.ReadLine();
                }
            }
        }

        private static void EventHandler_Received(object sender, BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body);
            Console.WriteLine("Start processing - {0}", message);
            var sleepSeconds = message.Count(x => x == '.');
            Thread.Sleep(sleepSeconds * 1000);
            (sender as IBasicConsumer).Model.BasicAck(e.DeliveryTag, false);
            Console.WriteLine("Completed - {0}", message);            
        }
    }
}
