using Nile.Managers.Contract.Client.DataContract;

namespace Nile.Managers.Proxys;

public interface IProxy<out TManager> where TManager : notnull
{
    Task<TResponse> RunWithoutRequestBody<TRequest, TResponse>(
        Func<TManager, Func<TRequest, Task<TResponse>>> grtFunc)
        where TRequest : RequestBase
        where TResponse : ResponseBase;
    
    Task<TResponse> RunWithRequestDto<TRequest, TResponse>(
        Func<TManager, Func<TRequest, Task<TResponse>>> getFunc,
        TRequest request)
        where TRequest : RequestBase
        where TResponse : ResponseBase;

    Task<TResponse> RunWithRequestStream<TRequest, TResponse>(
        Func<TManager, Func<TRequest, Task<TResponse>>> getFunc,
        System.IO.Stream requestBody)
        where TRequest : RequestBase
        where TResponse : ResponseBase;

    Task<TResponse> RunWithRouteParams<TRequest, TResponse>(
        Func<TManager, Func<TRequest, Task<TResponse>>> getFunc,
        Dictionary<string, object> routeParams)
        where TRequest : RequestBase
        where TResponse : ResponseBase;

    Task<TResponse> RunWithRequestStreamAndRouteParams<TRequest, TResponse>(
        Func<TManager, Func<TRequest, Task<TResponse>>> getFunc,
        System.IO.Stream requestBody,
        Dictionary<string, object> routeParams)
        where TRequest : RequestBase
        where TResponse : ResponseBase;
}