using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace Kwetter.UserService.Messaging
{
    public class MessageProducer : IMessageProducer
    {

        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageProducer(IConfiguration configuration)
        {
            _configuration = configuration;

            var factory = new ConnectionFactory()
            {
                HostName = "whale-01.rmq.cloudamqp.com",
                UserName = "ttdabobe",
                Password = "2Fx5QundhYgVLYeKrEaIUhCeiXh_OE5J",
                VirtualHost = "ttdabobe"
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Direct);
                _channel.QueueDeclare("user", exclusive: false);
                //_connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("--> Connected to MessageBus");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the Message Bus: {ex.Message}");
            }
        }

        public void SendingMessage<T>(T message)
        {

            Console.WriteLine("--> RabbitMQ Connection Open, sending message...");
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            _channel.BasicPublish(exchange: "trigger", routingKey: "user", body: body);



        }


    }
}
