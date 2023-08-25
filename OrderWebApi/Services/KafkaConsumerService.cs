using Confluent.Kafka;
using System.Diagnostics;
using System.Text.Json;

namespace OrderWebApi.Services
{
    namespace KafkaConsumer
    {
        public class KafkaConsumerService : IHostedService
        {
            private readonly string topic = "product-changes";
            private readonly string groupId = "product_group";
            private readonly string bootstrapServers = "localhost:9092";

            public Task StartAsync(CancellationToken cancellationToken)
            {
                var config = new ConsumerConfig
                {
                    GroupId = groupId,
                    BootstrapServers = bootstrapServers,
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };

                try
                {
                    using (var consumerBuilder = new ConsumerBuilder
                    <Ignore, string>(config).Build())
                    {
                        consumerBuilder.Subscribe(topic);
                        var cancelToken = new CancellationTokenSource();

                        try
                        {
                            while (true)
                            {
                                var consumer = consumerBuilder.Consume
                                   (cancelToken.Token);
                                Debug.WriteLine($"Processing Order :{ consumer.Message.Value}");
                            }
                        }
                        catch (OperationCanceledException)
                        {
                            consumerBuilder.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

                return Task.CompletedTask;
            }
            public Task StopAsync(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }
    }

}

