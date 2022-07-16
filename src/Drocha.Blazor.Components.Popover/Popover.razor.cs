using Microsoft.AspNetCore.Components;

namespace Drocha.Blazor.Components.Popover;

public enum Direction
{
    Top,
    Right,
    Left,
    Bottom,
}

public partial class Popover : ComponentBase
{
    [Inject] PopoverService PopoverSvc { get; set; } = null!;

    [Parameter] public Func<ElementReference>? GetControl { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; } = default!;

    [Parameter] public bool Open { get; set; } = false;
    [Parameter] public EventCallback<bool> OpenChanged { get; set; }

    [Parameter] public bool Dismissible { get; set; } = false;
    [Parameter] public bool DisableArrow { get; set; } = false;

    [Parameter] public bool FlipToFit { get; set; } = true;
    [Parameter] public Direction Direction { get; set; } = Direction.Top;
    [Parameter] public int Margin { get; set; } = 2;

    public ElementReference Ref { get; set; }

    private readonly string _id = Guid.NewGuid().ToString();
    public string Id => _id;

    private bool _open;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _open = Open;
        PopoverSvc.AddFragment(this);
    }

    protected override void OnParametersSet()
    {
        if (_open != Open)
        {
            _open = Open;
            PopoverSvc.UpdateVisibility(_id);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (GetControl is null)
        {
            throw new NullReferenceException(nameof(GetControl));
        }

        if (firstRender)
        {
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    public void Close()
    {
        if (_open)
        {
            Open = _open = false;
            PopoverSvc.UpdateVisibility(_id);
            OpenChanged.InvokeAsync(_open).ConfigureAwait(false);
        }
    }
}