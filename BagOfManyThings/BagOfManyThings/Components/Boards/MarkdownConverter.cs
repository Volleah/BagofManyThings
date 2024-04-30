using Microsoft.AspNetCore.Components;
using Markdig;

namespace BagOfManyThings.Components.Boards
{
    public class MarkdownEditorBase : ComponentBase
    {
        public string Body { get; set; } = string.Empty;
        public string Preview => Markdown.ToHtml(Body);
    }

}
