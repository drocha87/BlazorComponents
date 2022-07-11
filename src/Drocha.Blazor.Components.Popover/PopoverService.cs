using Microsoft.AspNetCore.Components;

namespace Drocha.Blazor.Components.Popover;

public class PopoverService
{
    public Dictionary<string, RenderFragment> Fragments { get; } = new();

    public delegate void FragmentsChangedHandler(object sender, RenderFragment f);

    public event FragmentsChangedHandler? OnChanged;

    public void AddFragment(string id, RenderFragment fragment)
    {
        Fragments[id] = fragment;
        OnChanged?.Invoke(this, fragment);
    }

    public void RemoveFragment(string id)
    {
        Fragments.Remove(id);
    }
}