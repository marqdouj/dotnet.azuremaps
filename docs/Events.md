## DotNet.AzureMaps Documentation - Events

### [<- Go Back](README.md)

## Summary
Events can be added to the map:
  - **When the map is Created**. Use the `Events`/`EventDefs` Parameters in the `AzureMap` component.
  - **After the map is Ready**. This can be done using the `IAzureMapsContainer` that is passed to the AzureMap.OnMapReady event.
	- i.e. MapContainer.Maps.AddEvents(IEnumerable of MapEventDef);

### Event Parameters
- `EventDefs`. This is an IEnumerable of `MapEventDef`
  - This has precedence over the `Events` parameter.
- `Events`. This is an IEnumerable of `MapEventType`

### Definitions
- `MapEventType`. An enum the represents a map event.
  - All events are added as continuous.

- `MapEventDef`. A definition of a map event and to how apply it.
  - `MapEventType`. See above description.
  - `MapEventTarget`. Map, DataSource, etc.
	- Currently, only Map and DataSource are supported.
	  In the future I will be adding support for other targets such as Layer, Shape, etc.
  - `TargetId`. Not required for 'Map', Required for all other targets (i.e. DataSource requies a `SourceId`).
  - `Once`. If true, the event is fired only once; otherwise as continuous. Default is `false`.
