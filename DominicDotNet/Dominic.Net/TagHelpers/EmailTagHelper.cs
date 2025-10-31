using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dominic.Net.TagHelpers
{
    // Custom tag helper to create a mailto link for email addresses
    public class EmailTagHelper : TagHelper
    {
        public string? Address { get; set; }
        public string? Content { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";

            output.Attributes.SetAttribute("href", "mailto:" + Address);
            output.Content.SetContent(Content);
        }
    }
}
