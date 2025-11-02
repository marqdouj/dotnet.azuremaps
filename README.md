# DotNet.AzureMaps

> NOTE: This is a new repository and is published in preview.

## Summary
A Blazor implementation of the Azure Maps Web SDK. 
It provides a set of components and services to easily integrate Azure Maps into Blazor applications.
The component is configured to allow for multiple Azure maps on the same page.
## Prerequisites
- [Azure Maps account](https://learn.microsoft.com/en-us/azure/azure-maps/quick-demo-map-app#create-an-azure-maps-account).
If you don't have an azure account, you can create a [free account](https://azure.microsoft.com).

## Demo App (New)
The 'Sandbox' demo has been removed and replaced by 
[`DemoApp`](https://github.com/marqdouj/dotnet.azuremaps/tree/master/demo/DemoApp).
If you have downloaded the repository previously you should download it again.
Once the code is downloaded you can run the demo app locally.

## Customization (JS Interop)
If there is some functionality not yet supported or you need to customize existing support
you can use your own custom [Blazor JS Interop](https://learn.microsoft.com/en-us/aspnet/core/blazor/javascript-interoperability/).
An example of one of the methods to do this is in the demo.

## Setup
- See `Program.cs`, `MapSetup.cs`, and `App.Razor` in the demo app.

## Release Notes
### 10.0.0-rc-3.0
- `MapInterop`. 
  - Change `ValueTask` to `Task` for JSInterop.
  - Add `DataSourceDef` parameter to `CreateLayer`. 
    This allows for single JSInterop call to create a datasource and layer,
    i.e. `CreateLayer(layerDef, layerDef.GetDataSource())`

### 10.0.0-rc-2.4
- `DemoApp`. `Sandbox` demo has been replaced by `DemoApp`.

### 10.0.0-rc-2.3
- `Issues`
  - `StyleOptions.Style`. This was incorrectly formatted and the map style was not getting updated.
    This has been fixed to match the expected format.

- `MapInterop`. Add new method:
  - `RemoveLayer(MapLayerDef)`. Removes a layer and its associated datasource from the map.

### 10.0.0-rc-2.2
- `SymbolLayerOptions`. Initialize Image/Text options with a new instance.

### 10.0.0-rc-2.1
- `MapLayerDef`. Initialize MapLayerDef.Options with a new instance.

### 10.0.0-rc-2.0
- `MapLayerDef`. Initialize Id and SourceId with an internally generated valid Css Id.
  This negates the mandatory assignment of these Ids when creating a LayerDef.
- `AzureMap`. Initialize Id with an internally generated valid Css Id.
  This negates the mandatory assignment of the Id when adding the component to the page.

### 10.0.0-rc-1.3
- `MapInterop`. Added `Configuration.ZoomTo(Position center, double zoomLevel)`.

### 10.0.0-rc-1.0
- Initial release with basic map functionality.
