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

public static class EnumExtension
{
    public static string ToDescriptionString<T>(this T @enum) where T : Enum
    {
        var fieldInfo = @enum.GetType().GetField(@enum.ToString());
        if (fieldInfo is not null)
        {
            DescriptionAttribute[] attrs = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attrs.Length > 0 ? attrs[0].Description : string.Empty;
        }
        return string.Empty;
    }
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