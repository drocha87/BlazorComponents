using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace DrBlazor;

public partial class DrOverlay : DrComponentBase
{
    [Parameter] public EventCallback<MouseEventArgs>  OnClose { get; set; }
}