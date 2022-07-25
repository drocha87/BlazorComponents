using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace DrBlazor;

public enum TypoStyle
{
    [Description("display")] Display,
    [Description("headline")] Headline,
    [Description("title")] Title,
    [Description("label")] Label,
    [Description("body")] Body,
}

public enum TypoSize
{
    [Description("large")] Large,
    [Description("medium")] Medium,
    [Description("small")] Small,
}

public partial class DrText : DrComponentBase
{
    [Parameter] public TypoStyle Typo { get; set; } = TypoStyle.Label;
    [Parameter] public TypoSize Size { get; set; } = TypoSize.Medium;

    [Parameter] public bool GutterBottom { get; set; } = false;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    private Dictionary<string, object> Attributes =>
        new AttrBuilder()
            .AddClass($"dr-text-{Typo.ToDescriptionString()}-{Size.ToDescriptionString()}")
            .AddStyle("margin-bottom", "24px", GutterBottom)

            .Build();
}