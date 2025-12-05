namespace Nile.Managers;

public class PageableResponseBase : CLI.ResponseBase
{
    public string? NextPageLink { get; set; }
}