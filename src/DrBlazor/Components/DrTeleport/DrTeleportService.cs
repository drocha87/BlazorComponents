using Microsoft.AspNetCore.Components;

namespace DrBlazor;

public class DrTeleportFragment
{
    public string To { get; set; } = default!;
    public string Id { get; set; } = default!;
    public bool Disabled { get; set; }
    public RenderFragment Fragment { get; set; } = null!;
}

public class DrTeleportService
{
    private readonly List<DrTeleportFragment> _fragments = new();

    public event Action? OnChanged;

    public void AppendFragment(DrTeleportFragment fragment)
    {
        _fragments.Add(fragment);
        HasChanged();
    }

    public void RemoveFragment(string id)
    {
        _fragments.RemoveAll(x => x.Id.Equals(id));
        HasChanged();
    }

    public void HasChanged()
    {
        OnChanged?.Invoke();
    }

    public IEnumerable<RenderFragment> GetRenderFragments(string spotName)
    {
        return _fragments
            .Where(x => x.To.Equals(spotName) && !x.Disabled)
            .Select(x => x.Fragment);
    }
}