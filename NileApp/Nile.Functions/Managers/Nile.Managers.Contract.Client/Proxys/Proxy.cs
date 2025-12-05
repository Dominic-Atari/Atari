using Microsoft.Extensions.Logging;

namespace Nile.Managers.Proxys;

public class Proxy<TManager> : IProxy<TManager> where TManager : notnull
{
    private readonly TManager _manager;

    private readonly IContextFactoryUtility _contextFactoryUtility;

    private readonly ILogger<Proxy<TManager>> _logger;

    private readonly IAuthorizer _authorizer;

    private readonly IConverter _converter;

    private readonly IValidator _validator;
    
    public Proxy(TManager manager, IContextFactoryUtility contextFactoryUtility, ILogger<Proxy<TManager>> logger, IAuthorizer authorizer, IConverter converter, IValidator validator)
    {
        _manager = manager;
        _contextFactoryUtility = contextFactoryUtility;
        _logger = logger;
        _authorizer = authorizer;
        _converter = converter;
        _validator = validator;
    }

}
