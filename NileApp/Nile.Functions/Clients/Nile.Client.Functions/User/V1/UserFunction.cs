using Microsoft.Extensions.Logging;
using Nile.Common.Extensions;
using Nile.Managers.Admin;
using Nile.Managers.Engagement;
using Nile.Managers.Proxys;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Nile.Client.Functions.Common;

namespace Nile.Client.Functions.User.V1;

/// <summary>
/// User-facing HTTP endpoints for authentication, profile management, social interactions, and search.
/// Each endpoint delegates to domain managers via IProxy, keeping transport concerns separate from business logic.
/// </summary>
public class UserFunction : FunctionBase
{
    /// <summary>
    /// Root route prefix for all v1 user endpoints.
    /// </summary>
    private const string RouteBase = "V1/users";

    /// <summary>
    /// Proxy to the user manager (authentication, profile CRUD, settings, etc.).
    /// </summary>
    private readonly IProxy<IUserManager> _userManagerProxy;

    /// <summary>
    /// Proxy to the social engagement manager (friend requests, interactions, social search).
    /// </summary>
    private readonly IProxy<ISocialEngagementManager> _socialEngagementManagerProxy;

    public UserFunction(
        ILogger<UserFunction> logger,
        IConfigUtility configUtility,
        IProxy<IUserManager> userManagerProxy,
        IProxy<ISocialEngagementManager> socialEngagementManagerProxy) : base(logger, configUtility)
    {
        _userManagerProxy = userManagerProxy;
        _socialEngagementManagerProxy = socialEngagementManagerProxy;
    }

    /// <summary>
    /// Authenticates the user and returns a context (tokens, user info) for the client session.
    /// Uses the user manager's Login action with an empty body (credentials come from ambient context or headers in the manager).
    /// </summary>
    [Function(nameof(UserFunction) + "_" + nameof(Login) + V1Suffix)]
    [ContextType(typeof(MobileUserContext))]
    [OpenApiOperation(nameof(Login))]
    [OpenApiResponseWithBody(System.Net.HttpStatusCode.OK, "application/json", typeof(CLI.V1.User.UserContextResponse))]
    public async Task<HttpResponseData> Login(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = RouteBase + "/:login")]
        HttpRequestData req)
    {
        var result =
            await _userManagerProxy.RunWithoutRequestBody<CLI.V1.User.LoginRequest, CLI.V1.User.UserContextResponse>(
                mgr => mgr.Login);

        return await CreateResponse(req, result);
    }

    /// <summary>
    /// Generates and returns username suggestions for a prospective user, based on input constraints.
    /// No request body is required; the manager derives suggestions from context or defaults.
    /// </summary>
    [Function(nameof(UserFunction) + "_" + nameof(GetUsernameSuggestions) + V1Suffix)]
    [ContextType(typeof(MobileUserContext))]
    [OpenApiOperation(nameof(GetUsernameSuggestions))]
    [OpenApiResponseWithBody(System.Net.HttpStatusCode.OK, "application/json", typeof(CLI.V1.User.UsernameSuggestionsResponse))]
    public async Task<HttpResponseData> GetUsernameSuggestions(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = RouteBase + "/:usernameSuggestions")]
        HttpRequestData req)
    {
        var result = await _userManagerProxy
            .RunWithoutRequestBody<CLI.V1.User.UsernameSuggestionsRequest, CLI.V1.User.UsernameSuggestionsResponse>(
                mgr => mgr.GenerateUsernameSuggestions);

        return await CreateResponse(req, result);
    }

    /// <summary>
    /// Creates a new user profile from the request body and returns the stored profile summary.
    /// </summary>
    [Function(nameof(UserFunction) + "_" + nameof(CreateProfile) + V1Suffix)]
    [ContextType(typeof(MobileUserContext))]
    [OpenApiOperation(nameof(CreateProfile))]
    [OpenApiResponseWithBody(System.Net.HttpStatusCode.OK, "application/json", typeof(CLI.V1.User.StoreUserResponseBase))]
    public async Task<HttpResponseData> CreateProfile(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = RouteBase + "/profile")]
        HttpRequestData req)
    {
        var result = await _userManagerProxy
            .RunWithRequestStream<CLI.V1.User.CreateUserProfileRequest, CLI.V1.User.StoreUserResponseBase>(
                mgr => mgr.Store,
                req.Body);

        return await CreateResponse(req, result);
    }

    /// <summary>
    /// Updates an existing user profile (patch/put semantics as defined by the manager) with data from the request body.
    /// </summary>
    [Function(nameof(UserFunction) + "_" + nameof(UpdateProfile) + V1Suffix)]
    [ContextType(typeof(MobileUserContext))]
    [OpenApiOperation(nameof(UpdateProfile))]
    [OpenApiResponseWithBody(System.Net.HttpStatusCode.OK, "application/json", typeof(CLI.V1.User.StoreUserResponseBase))]
    public async Task<HttpResponseData> UpdateProfile(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = RouteBase + "/profile")]
        HttpRequestData req)
    {
        var result = await _userManagerProxy
            .RunWithRequestStream<CLI.V1.User.StoreUserRequestBase, CLI.V1.User.StoreUserResponseBase>(
                mgr => mgr.Store,
                req.Body);

        return await CreateResponse(req, result);
    }

    /// <summary>
    /// Stores or replaces the user's profile image. The request body contains the required metadata/payload.
    /// </summary>
    [Function(nameof(UserFunction) + "_" + nameof(StoreProfileImage) + V1Suffix)]
    [ContextType(typeof(MobileUserContext))]
    [OpenApiOperation(nameof(StoreProfileImage))]
    [OpenApiResponseWithBody(System.Net.HttpStatusCode.OK, "application/json", typeof(CLI.V1.User.StoreUserResponseBase))]
    public async Task<HttpResponseData> StoreProfileImage(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = RouteBase + "/profile/image")]
        HttpRequestData req)
    {
        var result = await _userManagerProxy
            .RunWithRequestStream<CLI.V1.User.StoreUserProfileImageRequest, CLI.V1.User.StoreUserResponseBase>(
                mgr => mgr.Store,
                req.Body);

        return await CreateResponse(req, result);
    }

    /// <summary>
    /// Deletes the current user's profile image, if present.
    /// </summary>
    [Function(nameof(UserFunction) + "_" + nameof(DeleteProfileImage) + V1Suffix)]
    [ContextType(typeof(MobileUserContext))]
    [OpenApiOperation(nameof(DeleteProfileImage))]
    [OpenApiResponseWithBody(System.Net.HttpStatusCode.OK, "application/json", typeof(CLI.V1.User.StoreUserResponseBase))]
    public async Task<HttpResponseData> DeleteProfileImage(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = RouteBase + "/profile/image")]
        HttpRequestData req)
    {
        var result = await _userManagerProxy
            .RunWithoutRequestBody<CLI.V1.User.DeleteUserProfileImageRequest, CLI.V1.User.StoreUserResponseBase>(
                mgr => mgr.Store);

        return await CreateResponse(req, result);
    }

    /// <summary>
    /// Stores user settings/preferences. The request body contains the new values to persist.
    /// </summary>
    [Function(nameof(UserFunction) + "_" + nameof(StoreSettings) + V1Suffix)]
    [ContextType(typeof(MobileUserContext))]
    [OpenApiOperation(nameof(StoreSettings))]
    [OpenApiResponseWithBody(System.Net.HttpStatusCode.OK, "application/json", typeof(CLI.V1.User.StoreUserResponseBase))]
    public async Task<HttpResponseData> StoreSettings(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = RouteBase + "/settings")]
        HttpRequestData req)
    {
        var result = await _userManagerProxy
            .RunWithRequestStream<CLI.V1.User.StoreUserRequestBase, CLI.V1.User.StoreUserResponseBase>(
                mgr => mgr.Store,
                req.Body);

        return await CreateResponse(req, result);
    }

    /// <summary>
    /// Sends a friend request to another user. The request body identifies the target.
    /// </summary>
    [Function(nameof(UserFunction) + "_" + nameof(SendFriendRequest) + V1Suffix)]
    [ContextType(typeof(MobileUserContext))]
    [OpenApiOperation(nameof(SendFriendRequest))]
    [OpenApiResponseWithBody(System.Net.HttpStatusCode.OK, "application/json", typeof(CLI.ResponseBase))]
    public async Task<HttpResponseData> SendFriendRequest(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = RouteBase + "/:sendFriendRequest")]
        HttpRequestData req)
    {
        var result = await _socialEngagementManagerProxy
            .RunWithRequestStream<CLI.V1.User.FriendRequestRequest, CLI.ResponseBase>(
                mgr => mgr.Interact,
                req.Body);

        return await CreateResponse(req, result);
    }

    /// <summary>
    /// Updates a pending friend request (e.g., accept/decline). The request body holds the action and identifiers.
    /// </summary>
    [Function(nameof(UserFunction) + "_" + nameof(UpdateFriendRequest) + V1Suffix)]
    [ContextType(typeof(MobileUserContext))]
    [OpenApiOperation(nameof(UpdateFriendRequest))]
    [OpenApiResponseWithBody(System.Net.HttpStatusCode.OK, "application/json", typeof(CLI.ResponseBase))]
    public async Task<HttpResponseData> UpdateFriendRequest(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = RouteBase + "/:updateFriendRequest")]
        HttpRequestData req)
    {
        var result = await _socialEngagementManagerProxy
            .RunWithRequestStream<CLI.V1.User.UpdateFriendRequestRequest, CLI.ResponseBase>(
                mgr => mgr.Interact,
                req.Body);

        return await CreateResponse(req, result);
    }

    /// <summary>
    /// Retrieves friend requests the current user has sent to others.
    /// </summary>
    [Function(nameof(UserFunction) + "_" + nameof(GetSentFriendRequests) + V1Suffix)]
    [ContextType(typeof(MobileUserContext))]
    [OpenApiOperation(nameof(GetSentFriendRequests))]
    [OpenApiResponseWithBody(System.Net.HttpStatusCode.OK, "application/json", typeof(CLI.ResponseBase))]
    public async Task<HttpResponseData> GetSentFriendRequests(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = RouteBase + "/sentFriendRequests")]
        HttpRequestData req)
    {
        var result = await _socialEngagementManagerProxy
            .RunWithoutRequestBody<CLI.V1.User.SentFriendRequestsRequest, CLI.ResponseBase>(
                mgr => mgr.Get);

        return await CreateResponse(req, result);
    }

    /// <summary>
    /// Retrieves friend requests the current user has received from others.
    /// </summary>
    [Function(nameof(UserFunction) + "_" + nameof(GetReceivedFriendRequests) + V1Suffix)]
    [ContextType(typeof(MobileUserContext))]
    [OpenApiOperation(nameof(GetReceivedFriendRequests))]
    [OpenApiResponseWithBody(System.Net.HttpStatusCode.OK, "application/json", typeof(CLI.ResponseBase))]
    public async Task<HttpResponseData> GetReceivedFriendRequests(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = RouteBase + "/receivedFriendRequests")]
        HttpRequestData req)
    {
        var result = await _socialEngagementManagerProxy
            .RunWithoutRequestBody<CLI.V1.User.ReceivedFriendRequestsRequest, CLI.ResponseBase>(
                mgr => mgr.Get);

        return await CreateResponse(req, result);
    }

    /// <summary>
    /// Searches within the user's friend list using criteria in the request body.
    /// </summary>
    [Function(nameof(UserFunction) + "_" + nameof(SearchFriendList) + V1Suffix)]
    [ContextType(typeof(MobileUserContext))]
    [OpenApiOperation(nameof(SearchFriendList))]
    [OpenApiResponseWithBody(System.Net.HttpStatusCode.OK, "application/json", typeof(CLI.V1.User.UserSearchResponseBase))]
    public async Task<HttpResponseData> SearchFriendList(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = RouteBase + "/friends/:search")]
        HttpRequestData req)
    {
        var result = await _socialEngagementManagerProxy
            .RunWithRequestStream<CLI.V1.User.FriendListSearchRequest, CLI.V1.User.UserSearchResponseBase>(
                mgr => mgr.Search,
                req.Body);
        return await CreateResponse(req, result);
    }

    /// <summary>
    /// Searches for users across the platform using criteria provided in the request body.
    /// </summary>
    [Function(nameof(UserFunction) + "_" + nameof(SearchUsers) + V1Suffix)]
    [ContextType(typeof(MobileUserContext))]
    [OpenApiOperation(nameof(SearchUsers))]
    [OpenApiResponseWithBody(System.Net.HttpStatusCode.OK, "application/json", typeof(CLI.V1.User.UserSearchResponseBase))]
    public async Task<HttpResponseData> SearchUsers(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = RouteBase + "/:search")]
        HttpRequestData req)
    {
        var result = await _socialEngagementManagerProxy
            .RunWithRequestStream<CLI.V1.User.UserSearchRequestBase, CLI.V1.User.UserSearchResponseBase>(
                mgr => mgr.Search,
                req.Body);

        return await CreateResponse(req, result);
    }

    /// <summary>
    /// Removes a user from the current user's friend list. The route parameter supplies the target user id.
    /// </summary>
    [Function(nameof(UserFunction) + "_" + nameof(Unfriend) + V1Suffix)]
    [ContextType(typeof(MobileUserContext))]
    [OpenApiOperation(nameof(Unfriend))]
    [OpenApiResponseWithBody(System.Net.HttpStatusCode.OK, "application/json", typeof(CLI.ResponseBase))]
    public async Task<HttpResponseData> Unfriend(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = RouteBase + "/{id}/:unfriend")]
        HttpRequestData req, Guid id)
    {
        var result = await _socialEngagementManagerProxy
            .RunWithRequestDto<CLI.V1.User.UnfriendRequest, CLI.ResponseBase>(
                mgr => mgr.Interact,
                new CLI.V1.User.UnfriendRequest
                {
                    TargetUserId = id,
                });

        return await CreateResponse(req, result);
    }

    /// <summary>
    /// Retrieves profile details for the specified user id (public profile or as authorized).
    /// </summary>
    [Function(nameof(UserFunction) + "_" + nameof(GetUserProfile) + V1Suffix)]
    [ContextType(typeof(MobileUserContext))]
    [OpenApiOperation(nameof(GetUserProfile))]
    [OpenApiResponseWithBody(System.Net.HttpStatusCode.OK, "application/json", typeof(CLI.ResponseBase))]
    public async Task<HttpResponseData> GetUserProfile(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = RouteBase + "/{id}/profile")]
        HttpRequestData req, Guid id)
    {
        var result = await _socialEngagementManagerProxy
            .RunWithRequestDto<CLI.V1.User.UserProfileRequest, CLI.ResponseBase>(
                mgr => mgr.Get,
                new CLI.V1.User.UserProfileRequest
                {
                    TargetUserId = id,
                });

        return await CreateResponse(req, result);
    }
}