using Microsoft.AspNetCore.Components;

namespace DrBlazor;

public partial class DrGridItem : DrComponentBase
{
    [CascadingParameter] public DrGrid? Grid { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    // TODO: implements a way to observe breakpoints
    [Parameter] public uint Columns { get; set; } = 1;
    [Parameter] public bool Grow { get; set; } = false;
    [Parameter] public int Value { get; set; } = 0;

    private string _style = string.Empty;

    protected override void OnInitialized()
    {
        if (!Grow)
        {
            Grid?.ReserveSpace(Columns);
        }
    }

    protected override void OnParametersSet()
    {
        Console.WriteLine(Value);
        if (Columns > 12)
        {
            // in this grid layout is not allowed to have more than 12 columns
            Columns = 12;
        }

        _style = string.Empty;
        if (Style is not null)
        {
            _style = Style;
        }

        if (Grow)
        {
            _style = $"{_style}; grid-column: auto / span {Grid?.LastColumn ?? 12};";
        }
        else
        {
            _style = $"{_style}; grid-column: auto / span {Columns};";
        }
    }

    private Dictionary<string, object> Attributes =>
            new AttrBuilder()
                .AddClass("dr-grid-item")

                .AddClasses(Class)
                .Build();
}