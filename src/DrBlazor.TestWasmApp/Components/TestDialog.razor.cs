using Microsoft.AspNetCore.Components;

namespace DrBlazor;

public partial class TestDialog : DrComponentBase
{
    private bool _open = false;
    private bool _openPopover = false;

    public ElementReference _ref;

}

