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
### 10.7.7
- `IAzureMapContainer.Maps.ImageSprite`. Added support for `ImageSprite`.
  - See an example in the `Pizza Delivery` page in the demo app.

### 10.7.6
- `IAzureMapContainer.Maps.Data.Mercator`. Added support for 
  [`MercatorPoint`](https://learn.microsoft.com/en-us/javascript/api/azure-maps-control/atlas.data.mercatorpoint?view=azure-maps-typescript-latest).

### 10.7.5
- `IAzureMapContainer.Maps`.
  - `GetGeolocation`. Added new method to get the current geolocation.
  - `WatchGeolocation`. Added new method to watch the current geolocation.
  - `ClearWatchGeolocation`. Added new method to clear the geolocation watch.
- `AzureMap`.
  - `OnGeolocationWatch`. Added new callback event parameter to support `WatchGeolocation`.

### 10.7.4
- Depreciated.

### 10.7.3
- `Layers`.
  - `GeolocationManager`. Added new class to manage geolocation positions (with optional accurracy circles) to the map.
    See the demo app for a working example.
  - `Options.Filter`. Added new property. See the `GeolocationManager` for an example on using this.
- `GeoJson.Position`.
  - `Accuracy`. Added new property; normally used for geolocations.

### 10.7.2
- `StyleControl`. Fixed issue converting Options.MapStyles to Json.

### 10.7.1
- `Authentication`. 
  - `SasTokenUrl`. When Mode = SasToken you can provide a url to get the token.
    If the url is set, you do not need to configure the GetSasToken callback in App.Razor.
- `Controls`.
  - `TrafficLegendControl`. Added support for this control.

### 10.7.0
- `Authentication`. Added support for `SasToken` authentication.

### 10.6.1
- `Missing Events`. During the internal refactor, some map events were missed. These events have been added back:
  - `MapMouse`
  - `MapTouch`
  - `MapWheel`

### 10.6.0
- `JSInterop`. Internal refactor of JSInterop scripts to improve performance and functionality.
- `AzureMap`.
  - `OnMapEventReady`. This event is obsolete and has been removed. Use the `OnMapReady` event.
  - `MapInterop`. This class is obsolete and has been removed.

### 10.5.0
- `IAzureMapContainer`.
  - `Layers`.
    - `LayerAdded Event`. When events are passed to the `CreateLayer` method, 
       they were added to the layer after it was added to the map; 
       which meant that this event would not fire.
       Events are now added to the layer before it is added to the map.
  
### 10.4.0
- `IAzureMapContainer`.
  - `Maps`. Add new methods:
    - `GetTraffic`. Gets the traffic settings for the map.
    - `SetTraffic`. Sets the traffic settings for the map.
- `MapControls`.
  - `TrafficControl`. Added new control to manage traffic settings for the map.

### 10.3.8
- `Map Events`.
  - `Layers`. Fixed issue with deserialization of Layer event payload.
  - `MapEventArgs`. 
    - Added layer event payload for Map events LayerAdded/LayerRemoved.
    - Override ToString() to return serialized JSON of the event.
- `IAzureMapContainer`.
  - `Maps.CreateLayer`. Added parameter for layer/datasource events.

### 10.3.7
- `Map Events`.
  - `PreventDefault`. Incorrectly added it to `MapEventArgs`.
    Moved it to `MapEventDef` where it belongs.

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
