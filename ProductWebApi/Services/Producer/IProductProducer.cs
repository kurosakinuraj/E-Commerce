namespace ProductWebApi.Services.Producer
{
    public interface IProductProducer
    {
        Task<bool> Produce(string topic, string message);
    }
}
