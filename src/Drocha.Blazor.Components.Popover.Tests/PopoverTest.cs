using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Drocha.Blazor.Components.Popover.Tests;

public class PopoverTest
{
    [Fact]
    public void PopoverRenderCorrectly()
    {
        // using var ctx = new TestContext();

        // var getControlHandler = () => new ElementReference();

        // ctx.Services.AddSingleton(new PopoverService());
        // ctx.JSInterop.SetupModule("./_content/Drocha.Blazor.Components.Popover/popover.js");
        // ctx.JSInterop.SetupVoid("popover.initialize", getControlHandler(), )

        // var popover = ctx.RenderComponent<Popover>(parameters => parameters
        //     .Add(p => p.Dismissible, true)
        //     .Add(p => p.Open, true)
        //     .Add(p => p.GetControl, getControlHandler)
        //     // TODO: mock the get control
        //     .AddChildContent("<h1>Hello, World</h1>"));
    }
}