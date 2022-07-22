using Microsoft.AspNetCore.Components;

namespace DrBlazor;

public partial class DrFlex : DrComponentBase
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public bool Column { get; set; } = false;
    [Parameter] public bool Reverse { get; set; } = false;
    [Parameter] public string Gap { get; set; } = "2px";
    [Parameter] public string AlignItems { get; set; } = "start";
    [Parameter] public string JustifyContent { get; set; } = "start";

    private Dictionary<string, object> _attributes =>
        new AttrBuilder()
            .AddStyle("display", "flex")
            .AddStyle("flex-direction",
                !Column ? (Reverse ? "row-reverse" : "row") : (Reverse ? "column-reverse" : "column"))
            .AddStyle("gap", Gap)
            .AddStyle("align-items", AlignItems)
            .AddStyle("justify-content", JustifyContent)
            .Build();
}