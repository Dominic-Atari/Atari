using Nile.Managers.DataContract.HealthCheck;
using Nile.Managers.HealthCheck;

namespace Nile.Managers.Admin;

public interface IHealthCheckManager
{
    Task<HealthCheckResponse> Test(HealthCheckRequest request);
}