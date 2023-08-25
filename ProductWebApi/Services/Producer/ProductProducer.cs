using System.Text;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;

namespace ProductWebApi.Services.Producer
{
    public class ProductProducer:IProductProducer
    {
        private readonly ProducerConfig _config;

        public ProductProducer(ProducerConfig config)
        {
            _config = config;
        }
        public async Task<bool> Produce(string topic,string message)
        {

            using (var producer = new ProducerBuilder<Null, string>(_config).Build())
            {
                await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
                producer.Flush(TimeSpan.FromSeconds(10));
                return true;
            }
        }
    }
}
