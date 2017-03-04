using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace 消息生产者publish
{
    /// <summary>
    /// 消息生产，并把消息发送到交换机中
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.HostName = "127.0.0.1";
            factory.UserName = "guest";
            factory.Password = "guest";
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var properties = channel.CreateBasicProperties();

            properties.Headers = new Dictionary<string,object>
                {
                    {"password","123"},
                    { "username","peter"}
                };

            for (int i = 0; i < 100; i++)
            {
                var msg = Encoding.UTF8.GetBytes($"这是第{i}个消息体");
                channel.BasicPublish("myHeadersExchange", routingKey: string.Empty, basicProperties: properties, body: msg);
                Console.WriteLine($"第{i}个消息已推送到交换机");
            }
            Console.Read();
        }
    }
}
