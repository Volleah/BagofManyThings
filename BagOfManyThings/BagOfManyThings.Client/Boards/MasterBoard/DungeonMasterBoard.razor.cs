using Microsoft.AspNetCore.Components;
using Markdig;

namespace BagOfManyThings.Client.Boards.MasterBoard
{
    public class MarkdownEditorBase : ComponentBase
    {
        public string Body { get; set; } = string.Empty;
        public string Preview => Markdown.ToHtml(Body);
    }

}
