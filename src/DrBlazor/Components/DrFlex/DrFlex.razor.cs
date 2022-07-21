using Microsoft.AspNetCore.Components;

namespace DrBlazor;

public partial class DrFlex : DrComponentBase
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public bool Column { get; set; } = false;
    [Parameter] public bool Reverse { get; set; } = false;
    [Parameter] public string Gap { get; set; } = "2px";

    private Dictionary<string, object> _attributes =>
        new AttrBuilder()
            .AddStyle("display", "flex")
            .AddStyle("flex-direction",
                !Column ? (Reverse ? "row-reverse" : "row") : (Reverse ? "column-reverse" : "column"))
            .AddStyle("gap", Gap)
            .Build();
}