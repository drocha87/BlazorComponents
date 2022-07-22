using Microsoft.AspNetCore.Components;

namespace DrBlazor;

public partial class DrTeleport : DrComponentBase, IDisposable
{
    [Inject] DrTeleportService TeleportService { get; set; } = null!;

    [Parameter] public string To { get; set; } = null!;
    [Parameter] public bool Disabled { get; set; } = false;

    [Parameter] public RenderFragment ChildContent { get; set; } = null!;

    private DrTeleportFragment? _fragment;

    protected override void OnInitialized()
    {
        _fragment = new DrTeleportFragment
        {
            Id = Guid.NewGuid().ToString(),
            To = To,
            Disabled = Disabled,
            Fragment = ChildContent,
        };
        TeleportService.AppendFragment(_fragment);
    }

    protected override void OnParametersSet()
    {
        if (_fragment is not null)
        {
            if (!_fragment.To.Equals(To) || !_fragment.Disabled.Equals(Disabled) || !_fragment.Fragment.Equals(ChildContent))
            {
                _fragment.To = To;
                _fragment.Disabled = Disabled;
                _fragment.Fragment = ChildContent;
                TeleportService.HasChanged();
            }
        }
    }

    public new void Dispose()
    {
        if (_fragment is not null)
        {
            TeleportService.RemoveFragment(_fragment.Id);
        }
        base.Dispose();
    }
}