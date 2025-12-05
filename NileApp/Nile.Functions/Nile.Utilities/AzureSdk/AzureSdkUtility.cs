using Azure.Messaging.ServiceBus;
using Azure.Storage.Sas;
using Microsoft.Extensions.Logging;
using Nile.Common.Extensions;
using Nile.Common.InternalDTOs;
using Nile.Utilities.AzureSdk;
using Azure.Storage.Blobs;

namespace Nile.Utilities.AzureSdk;

internal partial class AzureSdkUtility : UtilityBase, IDisposable, IAsyncDisposable
{
    private readonly IDateUtility _dateUtility;
    private readonly IConfigUtility _configUtility;
    private ServiceBusClient _serviceBusClient;
    private BlobServiceClient _blobServiceClient;

    public AzureSdkUtility(ILogger<AzureSdkUtility> logger, IDateUtility dateUtility,
        IConfigUtility configurationUtility) : base(logger)
    {
        _dateUtility = dateUtility;
        _configUtility = configurationUtility;
        _serviceBusClient = new ServiceBusClient($"{_configUtility.ServiceBusUrl}", _configUtility.TokenCredential);
        _blobServiceClient = new BlobServiceClient(_configUtility.StorageAccountUri, _configUtility.TokenCredential);
    }

    public async void Dispose()
    {
        await _serviceBusClient.DisposeAsync();
    }

    public async ValueTask DisposeAsync()
   {
       await _serviceBusClient.DisposeAsync();
   }
}