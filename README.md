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
### 10.3.6
- `Map Events - **Obsolete**`.
  - `OnMapEventReady`. Do not use this event. It will be removed in a future version.
    - `OnMapReady`. Use this event instead.
- `Map Events - **New**`.
  - `OnMapEventAny`. Allows you to subscribe to a single event to receive any
    map event, instead of subscribing to a specific map event. The receiving event
    will need to cast the MapEventArgs to the specific MapEventArgs type (if required).
    - Does not apply to `OnMapReady`.
- `Map Events - **Enums**`. Added enums that represent subsets of MapEventType that apply to a specific MapEventTarget.
  The enums are castable to/from MapEventType. `MapEventTarget` extension methods have been added to return 
  an IEnumerable of `MapEventType` or `MapEventDef` that is applicable for a specific target.
  - `MapEventTypeDataSource`.
  - `MapEventTypeLayer`.
  - `MapEventTypeHtmlMarker`.
  - `MapEventTypePopup`.
  - `MapEventTypeShape`.
  - `MapEventTypeStyleControl`.
- `MapEventDef`. Added `PreventDefault` property. If true, applied to Mouse, Touch, and Wheel events.
- `Map Events - **Issues**`.
  - `Click Event`. Fixed issue where map click event might fire twice.

### 10.3.5
- `IAzureMapContainer`.
  - Added new method `Maps.AddEvents(IEnumerable<MapEventType> mapEvents)`.
    Events will be added using the `MapEventDef` default values.
- `Map Events`. Fixed the issue where the events could be added but not removed.
   Events can now be added and removed.

### 10.3.4
- `HtmlMarker`. Added support for HtmlMarkers
- `Popup`. Added support for Popups

### 10.3.3
- `Map Events`. Implemented dynamic add/remove event support for:
  - `Layer`
  - `Shape`
  - `StyleControl`

### 10.3.2
- Fixed issues in *.ts for Maps add/remove/get controls.

### 10.3.1
- Fixed NuGet package issue; removed unnecessary content files.

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
