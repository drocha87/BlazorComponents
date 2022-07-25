using Microsoft.AspNetCore.Components;

namespace DrBlazor;

public partial class DrGrid : DrComponentBase
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    private Dictionary<string, object> Attributes =>
         new AttrBuilder()
             .AddClass("dr-grid")

             .AddAttributes(UserAttributes)
             .Build();

    private uint remainingColumnsInRow = 12;
    public uint LastColumn => remainingColumnsInRow;

    public void ReserveSpace(uint value)
    {
        var remaining = remainingColumnsInRow - value;
        if (remaining <= 0)
        {
            remaining = 12 - remaining;
        }
        else
        {
            remainingColumnsInRow -= value;
        }
        remainingColumnsInRow = remaining;
    }
}