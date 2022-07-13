using Microsoft.AspNetCore.Components;

namespace Drocha.Blazor.Components.Popover;

public class PopoverService
{
    public Dictionary<string, Popover> Fragments { get; } = new();

    public delegate Task FragmentsChangedHandler(object sender, Popover f);

    public event FragmentsChangedHandler? OnChanged;

    public void AddFragment(Popover popover)
    {
        Fragments[popover.Id] = popover;
        OnChanged?.Invoke(this, Fragments[popover.Id]);
    }

    public void RemoveFragment(string id)
    {
        Fragments.Remove(id);
    }

    private uint _visibilityCounter = 0;

    public uint VisibleElements => _visibilityCounter;

    public void UpdateVisibility(string id)
    {
        if (Fragments.ContainsKey(id))
        {
            var fragment = Fragments[id];
            if (fragment.Open)
            {
                _visibilityCounter += 1;
            }
            else
            {
                if (_visibilityCounter <= 0)
                {
                    // FIXME: this code should never be reached
                    throw new InvalidOperationException("unreachable");
                }
                _visibilityCounter -= 1;
            }
            OnChanged?.Invoke(this, fragment);
        }
    }
}