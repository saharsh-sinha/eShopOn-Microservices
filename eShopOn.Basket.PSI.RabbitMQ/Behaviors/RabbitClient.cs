using eShopOn.Basket.Behaviors;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;
using System.Threading.Tasks;

namespace eShopOn.Basket.PSI.RabbitMQ.Behaviors
{
    public class RabbitClient : BackgroundService
    {
        private readonly IConnection _conn;
        private IBasketProvider _basketProvider;

        public RabbitClient(IBasketProvider basketProvider)
        {
            _basketProvider = basketProvider;
            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = "guest";
            factory.Password = "guest";
            factory.HostName = "localhost";
            _conn = factory.CreateConnection();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            IModel channel = _conn.CreateModel();
            try
            {
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (ch, ea) =>
                {
                    var basketId = ea.Body.ToString();
                    await _basketProvider.DeleteBasketByIdAsync(basketId);
                    channel.BasicAck(ea.DeliveryTag, false);
                };
            }
            finally
            {
                channel.Close();
                _conn.Close();
            }
        }
    }
}