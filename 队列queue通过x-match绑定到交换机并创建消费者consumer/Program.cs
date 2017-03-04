using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace 队列queue通过x_match绑定到交换机并创建消费者consumer
{

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
            channel.ExchangeDeclare("myHeadersExchange", ExchangeType.Headers, true, false, null);
            channel.QueueDeclare("myHeadersQueue", true, false, false, null);
            //将队列绑定到交换机上
            channel.QueueBind("myHeadersQueue", "myHeadersExchange", routingKey:string.Empty,arguments: new Dictionary<string, object>
            {
                { "x-match","all"},//同样还有可以设置为any
                {"password","123"},
                { "username","peter"}
            });

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var msg = Encoding.UTF8.GetString(e.Body);
                Console.WriteLine(msg);
            };
            //队列绑定消费者
            channel.BasicConsume("myHeadersQueue", true,consumer);

        }
    }
}
