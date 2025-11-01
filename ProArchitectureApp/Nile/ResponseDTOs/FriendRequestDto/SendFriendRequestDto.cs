namespace Nile.ResponseDTOs.FriendRequestDto
{
    public sealed class SendFriendRequestDto
    {
        public Guid RequesterUserId { get; set; }
        public Guid TargetUserId { get; set; }
    }
}
    