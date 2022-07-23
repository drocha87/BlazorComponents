# DrTeleport

This component is used to teleport render fragments to some pre-defined spots
anywhere in your project. The teleportation is composed of two components
`DrTeleportSpot` which is used to mark a place that will be able to receive
teleported fragments and `DrTeleport` that effectively teleports the fragment
into some spot. Every spot will have a name

```html
<DrTeleportSpot Name="MyModals" />
```

So you can use the `DrTeleport` to teleport content to this spot using

```html
<DrTeleport To="MyModals">
    Some content to be teleported
</DrTeleport>
```

`DrTeleport` accepts a parameter `Disabled` if `true` the content will not be
teleported, it's `false` by default.

The `DrTeleport` parameter `To` can be defined dynamically which means that you
can use logic to define the place where the content should be teleported.

## Implementation note

The initial idea was to implement a component like
[teleport](https://vuejs.org/guide/built-ins/teleport.html) in `vuejs` which
will enable you to teleport your render fragment to any `DOM` node using a query
selector as parameter. The problem will this approach is that it requires
JSInterop and a wrapper around the render fragment. Using this approach seems to
me not the right blazor way so the way I implemented it was using pre defined
`spots` with `DrTeleportSpot` and providing a name to it so you can teleport
your content only to these prefined and named spots as the example below will
show.

## Use Case

Imagine that you want to have modals, dialogs, popovers, tooltips, snackbars
etc... components that need the parent to be full width and full height so you
can position it according to the view coordinates.

Not only to have the code easy to read but also to have the component sharing
the same logic it makes sense to have these components inside the same files
and close to the components that it belongs to, but rendering it in the root
component like `#app` or `#page` for example.

## Example

In this example we use the `DrTeleportSpot` and `DrTeleport` to create a place
where all the modals will be inside, so we can easily control the z-index and
position it based on the viewport.

The first thing we should do is to define the spot that will receive the teleported
components.

Inside the `MainLayout.razor` you can define the spot like this.

```html
<div class="page">
    <div class="sidebar">
        ...
    </div>
    <main>
        ...
    </main>
    <!-- Define the spot with a high z-index -->
    <div style="z-index: 1000;">
        <DrTeleportSpot Name="DrModals" />
    </div>
</div>
```

> You can change the `DrModals` to any name you want, but if you're using
> `DrPopover` you should keep this name.

Now our popover implementation `DrPopover` will use this spot to teleport the
popover inside it when the popover is visible. Here is how `DrPopover` teleport
its content to `DrModals`.

```html
<DrTeleport To="DrModals" Disabled="@(!_open)">
    @if (Dismissible)
    {
        <DrOverlay OnClose="CloseAsync"></DrOverlay>
    }
    <div @ref="_ref" @attributes="Attributes">
        @ChildContent
    </div>
</DrTeleport>
```

 __The idea here is not to explain how
the popover works. For more information please look
[here](../DrPopover/README.md)__.

First creates a new component and put this code inside it. For example
`TestPopover.razor` and `TestPopover.razor.cs` (code behind).

Inside the `TestPopover.razor` you will have a code more or less like this one.

```html
...

<DrPopover @ref="_popover" Margin="10" FlipToFit Direction="Direction.Top">
    <DrContainer>
        <div style="max-width: 280px;">
            <h4>Basic dialog title</h4>
            <p>
                A dialog is a type of modal window that appears in front of app content
                to provide critical information or prompt for a decision to be made.
            </p>
        </div>
    </DrContainer>
</DrPopover>

<DrButtonText @ref="_button" Text="Open Popover" OnClick="TogglePopover">
</DrButtonText>

...
```

And in the code behind `TestPopover.razor.cs` you can show/close the popover after
the user click the `Open Popover` button.

```csharp
public DrButtonText? _button;
private DrPopover? _popover;

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
            // popover uses the ElementReference to properly place its content
            await _popover.ShowAsync(_button!.Ref);
        }
    }
}
```

When the user clicks in the button, the popover content will be teleported to
the `div` inside the `#page` so the popover should be visible at this point.

The `DOM` should looks like this:

```html
<div class="page" b-ytfawa61q1="">
    <div class="sidebar" b-ytfawa61q1="">
        ...
    </div>

    <main b-ytfawa61q1="">
        ...
    </main>

    <!-- the spot we defined as DrModals with the popover content -->
    <div style="z-index: 1000;" b-ytfawa61q1="">
        <!--!-->
        <div style="position: absolute; width: fit-content; top: 345.016px; left: 180.695px;" _bl_5=""
            data-dr-popover-placement="top">
            <!--!-->
            <div class="dr-container" style="padding: 24px;">
                <!--!-->
                <div style="max-width: 280px;">
                    <h4>Basic dialog title</h4>
                    <p>
                        A dialog is a type of modal window that appears in front of app content
                        to provide critical information or prompt for a decision to be made.
                    </p>
                </div>
            </div>
        </div>
    </div>
</div>
```
