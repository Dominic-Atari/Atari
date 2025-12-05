using Microsoft.Extensions.Logging;
using Nile.Common.Extensions;
using DTO = Nile.Common.InternalDTOs;
using Nile.Database.DataContracts;
using Nile.Utilities;
using Nile.Utilities.AzureSdk;
using EF = Nile.Database.Entities;

namespace Nile.Accessors.User
{
    internal class UserAccessor : AccessorBase, IUserAccessor
    {
        private readonly DatabaseContext _dbContext;
        private readonly IDateUtility _dateUtility;
        private readonly IConfigUtility _configurationUtility;
        private readonly IMapper _mapper;
        private readonly IBlobStorageUtility _blobStorageUtility;

        public UserAccessor(
            ILogger<UserAccessor> logger,
            DatabaseContext dbContext,
            IDateUtility dateUtility,
            IConfigUtility configurationUtility,
            IMapper mapper,
            IBlobStorageUtility blobStorageUtility) : base(logger)
        {
            _dbContext = dbContext;
            _dateUtility = dateUtility;
            _configurationUtility = configurationUtility;
            _mapper = mapper;
            _blobStorageUtility = blobStorageUtility;
        }

        Task<DTO.UserResponseBase> IUserAccessor.Store(DTO.UserRequestBase request)
        {
            return request switch
            {
                DTO.CreateUserRequest req => CreateUser(req),
                _ => throw new NotImplementedException($"{nameof(IUserAccessor.Store)} not implemented for '{request.GetType().Name}' (yet!).")
            };
        }

        private async Task<DTO.UserResponseBase> CreateUser(DTO.CreateUserRequest req)
        {
            var user = _mapper.Map<EF.User>(req);
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<DTO.StoreUserResponseBase>(user);
        }
    }
}