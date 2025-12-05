using System;

namespace Nile.MbUp;

public class PostUpdatedRequest : Nile.Managers.Contract.Client.DataContract.RequestBase
{
    public Guid PostId { get; set; }
}
