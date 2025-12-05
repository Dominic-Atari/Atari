using Nile.Common.InternalDTOs;
namespace Nile.Managers.Contract.Client
{
    public interface IMessageBusUtility
    {
        Task CreateQueueIfNotExists(string queueName);
        Task<HealthStatusType> Perform();
        Task SendMessageAsync<T>(string queueName, T message) where T : CLI.RequestBase;
    }
}