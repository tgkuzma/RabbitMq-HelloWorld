using System;
using System.Text;
using RabbitMQ.Client;

namespace Send
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var message = "";
            var closeApp = false;
            Console.WriteLine("----------Sender----------");
            Console.WriteLine("Write a message to send...");
            Console.WriteLine("...or [Enter] to exit");

            do
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    if (!string.IsNullOrEmpty(message))
                    {
                        channel.QueueDeclare(queue: "hello",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "",
                            routingKey: "hello",
                            basicProperties: null,
                            body: body);
                        Console.WriteLine(" Sent... {0}", message);
                    }
                }

                message = Console.ReadLine();
                if (string.IsNullOrEmpty(message))
                {
                    closeApp = true;
                }

            } while (!closeApp);

            Environment.Exit(0);
        }
    }
}
