using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.BoundedContext.Data;

namespace Todo.BoundedContext.Projections
{
    public static class MessageSender
    {
        private static JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
        };
        public static void Publish(TodoListDTO list)
        {
            var factory = new ConnectionFactory
            {
                HostName = "10.211.55.2",
                Port = 5672,
                UserName = "demo",
                Password = "demo",
                VirtualHost = "/"
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(
                    exchange: "signalr-exchange", 
                    type: "fanout", 
                    durable: true);
                
                var message = JsonConvert.SerializeObject(list, settings);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(
                    exchange: "signalr-exchange", 
                    routingKey: "", 
                    basicProperties: null, 
                    body: body);
            }

        }
    }
}
