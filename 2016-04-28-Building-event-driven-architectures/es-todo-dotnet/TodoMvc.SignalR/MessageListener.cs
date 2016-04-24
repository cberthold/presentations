using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Todo.BoundedContext.Data;

namespace TodoMvc.SignalR
{
    public static class MessageListener
    {
        private static JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
        };
        private static IConnection _connection;
        private static IModel _channel;
        public static void Start()
        {
            var factory = new ConnectionFactory
            {
                HostName = "10.211.55.2",
                Port = 5672,
                UserName = "demo",
                Password = "demo",
                VirtualHost = "/",
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(15)
            }; _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(
                exchange: "signalr-exchange", 
                type: "fanout", 
                durable: true);
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(
                queue: queueName, 
                exchange: "signalr-exchange", 
                routingKey: "");
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += ConsumerOnReceived;
            _channel.BasicConsume(queue: queueName, noAck: true, consumer: consumer);
        }
        public static void Stop()
        {
            _channel.Close(200, "Goodbye");
            _connection.Close();
        }
        private static void ConsumerOnReceived(object sender, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body);
            var output = JsonConvert.DeserializeObject(message, settings);

            var list = (TodoListDTO)output;
            TodoMvcHub.SendBroadcast(list);
        }
    }
}