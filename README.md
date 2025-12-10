# DotNet.AzureMaps

## Summary
A Blazor implementation of the Azure Maps Web SDK. 
It provides a set of components and services to easily integrate Azure Maps into Blazor applications.
The component is configured to allow for multiple Azure maps on the same page.

## Prerequisites
- [Azure Maps account](https://learn.microsoft.com/en-us/azure/azure-maps/quick-demo-map-app#create-an-azure-maps-account).
If you don't have an azure account, you can create a [free account](https://azure.microsoft.com).

## Demo
A demo of this, and other of my `DotNet` packages, can be found [here](https://github.com/marqdouj/dotnet.demo).

## Customization (JS Interop)
If there is some functionality not yet supported or you need to customize existing support
you can use your own custom [Blazor JS Interop](https://learn.microsoft.com/en-us/aspnet/core/blazor/javascript-interoperability/).
An example of one of the methods to do this is in the demo.

## Documentation
- [Go to Docs](docs/ReadMe.md)

## Setup
- See the [Configuration](docs/Configuration.md) section in the docs.

## Release Notes
### 10.3.0 (Major Changes)
- Major changes have been made to update/streamline JSInterop with the Azure Maps SDK.
  - `MapInterop`. The class has been marked as `obsolete`. It's orginal design did not lend well to future updates to support more advanced mapping scenarios.
    Replaced by the `IAzureMapContainer`.
  - `IAzureMapContainer`. The map container reference is passed to the parent component via the  `OnMapReady` event.
  - `OnMapReady`. New event.
  - `MapEventDef`. New class the defines a map event. See [Event Docs](docs/Events.md)
- Documentation. Docs have been added, updated, and streamlined for navigation.
- See the [Demo](https://github.com/marqdouj/dotnet.demo) for examples using the new features.

### 10.2.0 (Breaking Changes)
- Major refactor to update/streamline JSInterop with the Azure Maps SDK.
- Some namespaces, classes, and methods have been renamed or moved.
  - Most changes are backward compatible, but some breaking changes exist.
  - The most significant change is the default height and width of the map component is now 100% of its container.
    - You may need to adjust your CSS or container elements to ensure the map displays correctly.

### 10.1.0
- `MapConfiguration`.
  - `AddMarqdoujAzureMaps()`. Added `addValidation` parameter. 
    - If true and map validation fails then startup will fail.
    - If false (default) then all validation will be in the component; 
      which will display a message instead of the map if validation fails.

### 10.0.0
- Initial release.
