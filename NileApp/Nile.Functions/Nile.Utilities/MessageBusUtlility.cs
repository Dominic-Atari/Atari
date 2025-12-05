using Nile.Common.InternalDTOs;
using Nile.Utilities;
using Nile.Managers.Contract.Client;

namespace Nile.Utilities.AzureSdk;

internal partial class AzureSdkUtility : IMessageBusUtility
{
    public Task CreateQueueIfNotExists(string queueName)
    {
        throw new NotImplementedException();
    }

    public Task<HealthStatusType> Perform()
    {
        throw new NotImplementedException();
    }

    public Task SendMessageAsync<T>(string queueName, T message) where T : CLI.RequestBase
    {
        throw new NotImplementedException();
    }
}