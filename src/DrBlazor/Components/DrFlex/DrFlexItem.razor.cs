using Microsoft.AspNetCore.Components;

namespace DrBlazor;

public partial class DrFlexItem : DrComponentBase
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    ///     This property specifies how much of the remaining space in the flex container should
    ///     be assigned to the item (the flex grow factor).
    /// </summary>
    [Parameter] public float Grow { get; set; } = 1f;

    // TODO: implements others properties as self positioning

    private Dictionary<string,object>  _attributes =>
        new AttrBuilder()
            .AddStyle("flex-grow", Grow.ToString())
            .Build();
}