# DotNet.AzureMaps

> NOTE: This is a new repository and is published in preview.

## Summary
A Blazor implementation of the Azure Maps Web SDK. 
It provides a set of components and services to easily integrate Azure Maps into Blazor applications.
The component is configured to allow for multiple Azure maps on the same page.
## Prerequisites
- [Azure Maps account](https://learn.microsoft.com/en-us/azure/azure-maps/quick-demo-map-app#create-an-azure-maps-account).
If you don't have an azure account, you can create a [free account](https://azure.microsoft.com).

## Demo
Demo on how to use this library is found 
[here](https://github.com/marqdouj/dotnet.azuremaps/tree/master/demo/Sandbox)

## Customization (JS Interop)
If there is some functionality not yet supported or you need to customize existing support
you can use your own custom [Blazor JS Interop](https://learn.microsoft.com/en-us/aspnet/core/blazor/javascript-interoperability/).
An example of one of the methods to do this is in the demo.

## Setup
- See `Program.cs`, `MapSetup.cs`, and `App.Razor` in the demo app.

## Release Notes
### 10.0.0-rc-1.1
- `MapInterop`. Added `Configuration.ZoomTo(Position center, double zoomLevel)`.

### 10.0.0-rc-1.0
- Initial release with basic map functionality.
