using Nile.Common.InternalDTOs;
using Nile.Managers.Contract.Client.DataContract.V1.User;

namespace Nile.Managers.Admin;

public interface IUserManager
{
    Task<UsernameSuggestionsResponse> GenerateUsernameSuggestions(UsernameSuggestionsRequest request);

    Task<UserContextResponse> Login(LoginRequest request);

    Task<StoreUserResponseBase> Store(StoreUserRequestBase request);
}