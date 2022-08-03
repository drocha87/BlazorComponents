using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DrBlazor;

// https://drafts.fxtf.org/geometry/#DOMRect
public struct DOMRect
{
    public readonly double Bottom { get; init; }
    public readonly double Height { get; init; }
    public readonly double Left { get; init; }
    public readonly double Right { get; init; }
    public readonly double Top { get; init; }
    public readonly double Width { get; init; }
    public readonly double X { get; init; }
    public readonly double Y { get; init; }
}

public static class ElementReferenceExtensions
{
    public static async Task<DOMRect> GetBoundingClientRectAsync(this ElementReference el, IJSRuntime js)
    {
        IJSObjectReference jsModule = await js.InvokeAsync<IJSObjectReference>("import", "./_content/DrBlazor/Extensions/extensions.js");
        DOMRect rect = await jsModule.InvokeAsync<DOMRect>("getBoundingClientRect", el);

        return rect;
    }
}