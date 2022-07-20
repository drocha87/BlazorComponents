# Drocha Blazor Components

Drocha Blazor Components is a set of UI components and other useful extensions for [Blazor](https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-6.0) applications. All components are free to use. **This project is experimental.**

## About the project

The main goal of this project is to provide components to be used in my personal projects and to tests new ideas. You are free to fork it or to use it as it is.

## Components

These are some of the components I expect to implement in the next couple months

- [x] Popover (partial implemented)
- [x] Tooltip
- [x] Teleport (like the teleport in vuejs)
- [ ] Dialog
- [ ] Mention Textarea

## How to test it

I provided two test projects `DrBlazor.TestServerApp`(not implemented yet) and `DrBlazor.TestWasmApp`. You can `cd` into these folders and run the command:

```sh
cd DrBlazor.TestWasmApp
dotnet watch run
```

## How to use it

Link `DrBlazor` as a reference to your project, and add the following line to the `head` tag of `wwwroot/index.html` in case of a wasm project.

```html
<link href="_content/DrBlazor/style.css" rel="stylesheet" />
```

and add the `DrBlazor` configuration as a singleton or scoped dependency.

```csharp
builder.Services.AddSingleton<DrConfig>();
```
