using Microsoft.AspNetCore.Components;

namespace DrBlazor;

public partial class TestDialog : DrComponentBase
{
    public ElementReference _ref;

    public DrButtonText? _button;
    private DrPopover? _popover;
    private DrDialog? _dialog;

    private async Task TogglePopover()
    {
        if (_popover is not null)
        {
            if (_popover.IsVisible)
            {
                await _popover.CloseAsync();
            }
            else
            {
                await _popover.ShowAsync(_button!.Ref);
            }
        }
    }

    private async Task ToggleDialog()
    {
        if (_dialog is not null)
        {
            if (_dialog.IsVisible)
            {
                await _dialog.CloseAsync();
            }
            else
            {
                await _dialog.ShowAsync();
            }
        }
    }
}

