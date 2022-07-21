using Microsoft.AspNetCore.Components;

namespace DrBlazor;

public partial class DrFlexItem : DrComponentBase
{
    [CascadingParameter] public bool Column { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    ///     This property specifies how much of the remaining space in the flex container should
    ///     be assigned to the item (the flex grow factor).
    /// </summary>
    [Parameter] public float Grow { get; set; } = 1f;

    [Parameter] public string AlignSelf { get; set; } = "start";

    // TODO: implements others properties as self positioning

    private Dictionary<string, object> _attributes =>
            new AttrBuilder()
                .AddStyle("flex-grow", Grow.ToString())
                .AddStyle("align-self", AlignSelf)
                .AddStyle("width", "100%", Column)
                .Build();
}