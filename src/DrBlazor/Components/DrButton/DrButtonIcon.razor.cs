using Microsoft.AspNetCore.Components;

namespace DrBlazor;

public partial class DrButtonIcon : DrComponentBase
{
    [Parameter] public string Icon { get; set; } = null!;

    [Parameter] public EventCallback OnClick { get; set; }
}