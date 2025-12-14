import * as atlas from "azure-maps-control";
import { Logger, LogLevel } from "./logger"
import { Helpers } from "./helpers"
import { MapFactory } from "../core/factory"
import { MarkerManager } from "./markers"
import { ControlManager } from "./controls"
import { PopupManager } from "./popups"

export class EventManager {
    static addEvents(dotNetRef: any, mapId: string, events: MapEvent[]): void {
        if (events.length == 0) return;

        const azmap = MapFactory.getMap(mapId);

        if (!azmap) {
            Logger.logMessage(mapId, LogLevel.Error, "Events:addEvents - map not found.");
            return;
        }

        events ??= [];

        this.#addMapEvents(dotNetRef, azmap, mapId, Object.values(events).filter(value => value.target === "map"));
        this.#addDataSourceEvents(dotNetRef, azmap, mapId, Object.values(events).filter(value => value.target === "datasource"));
        this.#addHtmlMarkerEvents(dotNetRef, azmap, mapId, Object.values(events).filter(value => value.target === "htmlmarker"));
        this.#addLayerEvents(dotNetRef, azmap, mapId, Object.values(events).filter(value => value.target === "layer"));
        this.#addShapeEvents(dotNetRef, azmap, mapId, Object.values(events).filter(value => value.target === "shape"));
        this.#addStyleControlEvents(dotNetRef, azmap, mapId, Object.values(events).filter(value => value.target === "stylecontrol"));
        this.#addPopupEvents(dotNetRef, azmap, mapId, Object.values(events).filter(value => value.target === "popup"));
    }

    static removeEvents(mapId: string, events: MapEvent[]): void {
        if (events.length == 0) return;

        const azmap = MapFactory.getMap(mapId);

        if (!azmap) {
            Logger.logMessage(mapId, LogLevel.Error, "Events:removeEvents - map not found.");
            return;
        }

        events ??= [];

        this.#removeMapEvents(azmap, mapId, Object.values(events).filter(value => value.target === "map"));
        this.#removeDataSourceEvents(azmap, mapId, Object.values(events).filter(value => value.target === "datasource"));
        this.#removeHtmlMarkerEvents(azmap, mapId, Object.values(events).filter(value => value.target === "htmlmarker"));
        this.#removeLayerEvents(azmap, mapId, Object.values(events).filter(value => value.target === "layer"));
        this.#removeShapeEvents(azmap, mapId, Object.values(events).filter(value => value.target === "shape"));
        this.#removeStyleControlEvents(azmap, mapId, Object.values(events).filter(value => value.target === "stylecontrol"));
        this.#removePopupEvents(azmap, mapId, Object.values(events).filter(value => value.target === "popup"));
    }

    // #region MapEvents

    static #addMapEvents(dotNetRef: any, azmap: atlas.Map, mapId: string, events: MapEvent[]): void {
        if (events.length == 0) return;

        this.#addMapEventsConfig(dotNetRef, azmap, mapId, events);
        this.#addMapEventsData(dotNetRef, azmap, mapId, events);
        this.#addMapEventsGeneral(dotNetRef, azmap, mapId, events);
        this.#addMapEventsLayer(dotNetRef, azmap, mapId, events);
        this.#addMapEventsMouse(dotNetRef, azmap, mapId, events);
        this.#addMapEventsSource(dotNetRef, azmap, mapId, events);
        this.#addMapEventsStyle(dotNetRef, azmap, mapId, events);
        this.#addMapEventsTouch(dotNetRef, azmap, mapId, events);
        this.#addMapEventsWheel(dotNetRef, azmap, mapId, events);
    }

    static #addMapEventsConfig(dotNetRef: any, azmap: atlas.Map, mapId: string, events: MapEvent[]): void {
        if (events.length == 0) return;

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventConfig, value.type)).forEach((value) => {

            if (value.once) {
                //not supported.
                Logger.logMessage(mapId, LogLevel.Error, "event not added. 'once' is not supported:", value);
            }
            else {
                azmap.events.add(value.type as MapEventConfig, (config: atlas.MapConfiguration) => {
                    const result: MapEventInfo = { mapId: mapId, type: value.type, payload: config };
                    dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventConfig, result);
                });
            }

            Logger.logMessage(mapId, LogLevel.Trace, "event added:", value);
        });
    }

    static #addMapEventsData(dotNetRef: any, azmap: atlas.Map, mapId: string, events: MapEvent[]) {
        if (events.length == 0) return;

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventData, value.type)).forEach((value) => {
            let result: MapEventInfo = { mapId: mapId, type: value.type };

            if (value.once) {
                azmap.events.addOnce(value.type as MapEventData, (dataEvent: atlas.MapDataEvent) => {
                    result.payload = {
                        dataType: dataEvent.dataType,
                        isSourceLoaded: dataEvent.isSourceLoaded,
                        source: dataEvent.source?.getId(),
                        sourceDataType: dataEvent.sourceDataType,
                        tile: dataEvent.tile
                    };
                    dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventData, result);
                });
            }
            else {
                azmap.events.add(value.type as MapEventData, (dataEvent: atlas.MapDataEvent) => {
                    result.payload = {
                        dataType: dataEvent.dataType,
                        isSourceLoaded: dataEvent.isSourceLoaded,
                        source: dataEvent.source?.getId(),
                        sourceDataType: dataEvent.sourceDataType,
                        tile: dataEvent.tile
                    };
                    dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventData, result);
                });
            }

            Logger.logMessage(mapId, LogLevel.Trace, "event added:", value);
        });
    }

    static #addMapEventsGeneral(dotNetRef: any, azmap: atlas.Map, mapId: string, events: MapEvent[]): void {
        if (events.length == 0) return;

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventGeneral, value.type)).forEach((value) => {
            let result: MapEventInfo = { mapId: mapId, type: value.type };

            if (value.once) {
                azmap.events.addOnce(value.type as MapEventGeneral, () => {
                    dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEvent, result);
                });
            }
            else {
                azmap.events.add(value.type as MapEventGeneral, () => {
                    dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEvent, result);
                });
            }

            Logger.logMessage(mapId, LogLevel.Trace, "event added:", value);
        });
    }

    static #addMapEventsLayer(dotNetRef: any, azmap: atlas.Map, mapId: string, events: MapEvent[]) {
        if (events.length == 0) return;

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventLayer, value.type)).forEach((value) => {
            let result: MapEventInfo = { mapId: mapId, type: value.type };

            if (value.once) {
                azmap.events.addOnce(value.type as MapEventLayer, (layer: atlas.layer.Layer) => {
                    result.payload = { id: layer.getId() };
                    dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEvent, result);
                });
            }
            else {
                azmap.events.add(value.type as MapEventLayer, (layer: atlas.layer.Layer) => {
                    result.payload = { id: layer.getId() };
                    dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEvent, result);
                });
            }

            Logger.logMessage(mapId, LogLevel.Trace, "event added:", value);
        });
    }

    static #addMapEventsMouse(dotNetRef: any, azmap: atlas.Map, mapId: string, events: MapEvent[]) {
        if (events.length == 0) return;

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventMouse, value.type)).forEach((value) => {
            let result: MapEventInfo = { mapId: mapId, type: value.type };

            if (value.once) {
                azmap.events.addOnce(value.type as MapEventMouse, (mouseEvent: atlas.MapMouseEvent) => {
                    result.payload = this.#buildMouseEventPayload(mouseEvent);
                    dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventMouse, result);
                });
            }
            else {
                azmap.events.add(value.type as MapEventMouse, (mouseEvent: atlas.MapMouseEvent) => {
                    result.payload = this.#buildMouseEventPayload(mouseEvent);
                    dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventMouse, result);
                });
            }

            Logger.logMessage(mapId, LogLevel.Trace, "event added:", value);
        });
    }

    static #addMapEventsSource(dotNetRef: any, azmap: atlas.Map, mapId: string, events: MapEvent[]) {
        if (events.length == 0) return;

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventSource, value.type)).forEach((value) => {
            let result: MapEventInfo = { mapId: mapId, type: value.type };

            if (value.once) {
                azmap.events.addOnce(value.type as MapEventSource, (source: atlas.source.Source) => {
                    result.payload = { id: source.getId() };
                    dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventSource, result);
                });
            }
            else {
                azmap.events.add(value.type as MapEventSource, (source: atlas.source.Source) => {
                    result.payload = { id: source.getId() };
                    dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventSource, result);
                });
            }

            Logger.logMessage(mapId, LogLevel.Trace, "event added:", value);
        });
    }

    static #addMapEventsStyle(dotNetRef: any, azmap: atlas.Map, mapId: string, events: MapEvent[]) {
        if (events.length == 0) return;

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventStyle, value.type)).forEach((value) => {
            let result: MapEventInfo = { mapId: mapId, type: value.type };

            switch (value.type.toLowerCase() as MapEventStyle) {
                case MapEventStyle.StyleChanged:
                    if (value.once) {
                        azmap.events.addOnce(MapEventStyle.StyleChanged, (event: atlas.StyleChangedEvent) => {
                            result.payload = { style: event.style };
                            dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventStyle, result);
                        });
                    }
                    else {
                        azmap.events.addOnce(MapEventStyle.StyleChanged, (event: atlas.StyleChangedEvent) => {
                            result.payload = { style: event.style };
                            dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventStyle, result);
                        });
                    }
                    break;
                case MapEventStyle.StyleImageMissing:
                    if (value.once) {
                        azmap.events.addOnce(MapEventStyle.StyleImageMissing, (style: string) => {
                            result.payload = { style: style };
                            dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventStyle, result);
                        });
                    }
                    else {
                        azmap.events.addOnce(MapEventStyle.StyleImageMissing, (style: string) => {
                            result.payload = { style: style };
                            dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventStyle, result);
                        });
                    }
                    break;
                default:
            }

            Logger.logMessage(mapId, LogLevel.Trace, "event added:", value);
        });
    }

    static #addMapEventsTouch(dotNetRef: any, azmap: atlas.Map, mapId: string, events: MapEvent[]) {
        if (events.length == 0) return;

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventTouch, value.type)).forEach((value) => {
            let result: MapEventInfo = { mapId: mapId, type: value.type };

            if (value.once) {
                azmap.events.addOnce(value.type as MapEventTouch, (touchEvent: atlas.MapTouchEvent) => {
                    result.payload = this.#buildTouchEventPayload(touchEvent);
                    dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventTouch, result);
                });
            }
            else {
                azmap.events.add(value.type as MapEventTouch, (touchEvent: atlas.MapTouchEvent) => {
                    result.payload = this.#buildTouchEventPayload(touchEvent);
                    dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventTouch, result);
                });
            }

            Logger.logMessage(mapId, LogLevel.Trace, "event added:", value);
        });
    }

    static #addMapEventsWheel(dotNetRef: any, azmap: atlas.Map, mapId: string, events: MapEvent[]) {
        if (events.length == 0) return;

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventWheel, value.type)).forEach((value) => {
            let result: MapEventInfo = { mapId: mapId, type: value.type };

            if (value.once) {
                azmap.events.addOnce(value.type as MapEventWheel, (wheelEvent: atlas.MapMouseWheelEvent) => {
                    result.payload = this.#buildWheelEventPayload(wheelEvent);
                    dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventWheel, result);
                });
            }
            else {
                azmap.events.add(value.type as MapEventWheel, (wheelEvent: atlas.MapMouseWheelEvent) => {
                    result.payload = this.#buildWheelEventPayload(wheelEvent);
                    dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventWheel, result);
                });
            }

            Logger.logMessage(mapId, LogLevel.Trace, "event added:", value);
        });
    }

    static #removeMapEvents(azmap: atlas.Map, mapId: string, events: MapEvent[]): void {
        if (events.length == 0) return;

        events.forEach((value) => {
            azmap.events.remove(value.type, () => { });
            Logger.logMessage(mapId, LogLevel.Trace, "event removed:", value);
        });
    }

    // #endregion

    // #region DataSourceEvents
    static #addDataSourceEvents(dotNetRef: any, azmap: atlas.Map, mapId: string, events: MapEvent[]): void {
        if (events.length == 0) return;

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventDataSource, value.type)).forEach((value) => {
            const ds = azmap.sources.getById(value.targetId) as atlas.source.DataSource;

            if (ds) {
                let dsShapes: object[] = [];
                let payload: { sourceId: string, shapes: object[] };
                payload.sourceId = value.targetId;
                payload.shapes = dsShapes;

                let result: MapEventInfo = { mapId: mapId, type: value.type, payload: payload };
                let wasAdded: boolean = true;

                switch (value.type.toLowerCase()) {
                    case MapEventDataSource.DataSourceUpdated:
                        if (value.once) {
                            azmap.events.addOnce(MapEventDataSource.DataSourceUpdated, ds, () => {
                                dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventDataSource, result);
                            });
                        }
                        else {
                            azmap.events.add(MapEventDataSource.DataSourceUpdated, ds, () => {
                                dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventDataSource, result);
                            });
                        }
                        break;
                    case MapEventDataSource.DataAdded:
                    case MapEventDataSource.DataRemoved:
                        if (value.once) {
                            azmap.events.addOnce(value.type as any, ds, (shapes: atlas.Shape[]) => {
                                payload.shapes = this.#buildShapeResults(shapes);
                                dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventDataSource, result);
                            });
                        }
                        else {
                            azmap.events.add(value.type as any, ds, (shapes: atlas.Shape[]) => {
                                payload.shapes = this.#buildShapeResults(shapes);
                                dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventDataSource, result);
                            });
                        }
                        break;
                    default:
                        wasAdded = false;
                }

                if (wasAdded) {
                    Logger.logMessage(mapId, LogLevel.Trace, "event added:", value);
                }
                else {
                    Logger.logMessage(mapId, LogLevel.Trace, "event was not added - type not found:", value);
                }
            }
            else {
                Logger.logMessage(mapId, LogLevel.Error, "Events:addDataSourceEvents - invalid TargetId.", value);
            }
        });
    }

    static #removeDataSourceEvents(azmap: atlas.Map, mapId: string, events: MapEvent[]): void {
        events.forEach((value) => {
            const ds = azmap.sources.getById(value.targetId) as atlas.source.DataSource;

            if (ds) {
                azmap.events.remove(value.type, ds, () => { });
                Logger.logMessage(mapId, LogLevel.Trace, "event removed:", value);
            }
            else {
                Logger.logMessage(mapId, LogLevel.Error, "Events:removeDataSourceEvents - invalid TargetId.", value);
            }
        });
    }
    // #endregion

    // #region HtmlMarkerEvents
    static #addHtmlMarkerEvents(dotNetRef: any, azmap: atlas.Map, mapId: string, events: MapEvent[]): void {
        if (events.length == 0) return;

        Object.values(events).filter(value => Helpers.isValueInEnum(MapHtmlMarkerEvent, value.type)).forEach((value) => {
            const target = MarkerManager.getMarker(mapId, value.targetId);

            if (target) {
                let result: MapEventInfo = { mapId: mapId, type: value.type };

                if (value.once) {
                    azmap.events.addOnce(value.type as MapHtmlMarkerEvent, target, (event: atlas.TargetedEvent) => {
                        result.payload = this.#buildTargetedEventPayload(event);
                        dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventHtmlMarker, result);
                    });
                }
                else {
                    azmap.events.add(value.type as MapHtmlMarkerEvent, target, (event: atlas.TargetedEvent) => {
                        result.payload = this.#buildTargetedEventPayload(event);
                        dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventHtmlMarker, result);
                    });
                }

                Logger.logMessage(mapId, LogLevel.Trace, "event added:", value);
            }
            else {
                Logger.logMessage(mapId, LogLevel.Error, "Events:addHtmlMarkerEvents - invalid TargetId.", value);
            }

            Logger.logMessage(mapId, LogLevel.Trace, "event added:", value);
        });
    }

    static #removeHtmlMarkerEvents(azmap: atlas.Map, mapId: string, events: MapEvent[]): void {
        if (events.length == 0) return;

        events.forEach((value) => {
            const target = MarkerManager.getMarker(mapId, value.targetId);

            if (target) {
                azmap.events.remove(value.type, target, () => { });
                Logger.logMessage(mapId, LogLevel.Trace, "event removed:", value);
            }
            else {
                Logger.logMessage(mapId, LogLevel.Error, "Events:removeHtmlMarkerEvents - invalid TargetId.", value);
            }
        });
    }
    // #endregion

    // #region LayerEvents
    static #addLayerEvents(dotNetRef: any, azmap: atlas.Map, mapId: string, events: MapEvent[]): void {
        if (events.length == 0) return;

        Logger.logMessage(mapId, LogLevel.Trace, "addLayerEvents:", events);

        Object.values(events).filter(value => Helpers.isValueInEnum(MapLayerEvent, value.type)).forEach((value) => {
            const target = azmap.layers.getLayerById(value.targetId);

            if (target) {
                let result: MapEventInfo = { mapId: mapId, type: value.type };
                let wasAdded: boolean = false;

                if (Helpers.isValueInEnum(MapEventTouch, value.type)) {
                    if (value.once) {
                        azmap.events.addOnce(value.type as MapEventTouch, target, (touchEvent: atlas.MapTouchEvent) => {
                            result.payload = { id: value.targetId, touch: this.#buildTouchEventPayload(touchEvent) };
                            dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventLayer, result);
                        });
                    }
                    else {
                        azmap.events.add(value.type as MapEventTouch, target, (touchEvent: atlas.MapTouchEvent) => {
                            result.payload = { id: value.targetId, touch: this.#buildTouchEventPayload(touchEvent) };
                            dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventLayer, result);
                        });
                    }

                    wasAdded = true;
                }

                if (Helpers.isValueInEnum(MapEventLayer, value.type)) {
                    if (value.once) {
                        azmap.events.addOnce(value.type as MapEventLayer, target, (layer: atlas.layer.Layer) => {
                            result.payload = { id: layer.getId() };
                            dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventLayer, result);
                        });
                    }
                    else {
                        azmap.events.add(value.type as MapEventLayer, target, (layer: atlas.layer.Layer) => {
                            result.payload = { id: layer.getId() };
                            dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventLayer, result);
                        });
                    }

                    wasAdded = true;
                }

                if (Helpers.isValueInEnum(MapEventMouse, value.type) || Helpers.isValueInEnum(MapLayerMouseEvent, value.type)) {
                    if (value.once) {
                        azmap.events.addOnce(value.type as MapEventMouse, target, (mouseEvent: atlas.MapMouseEvent) => {
                            result.payload = { id: value.targetId, mouse: this.#buildMouseEventPayload(mouseEvent) };
                            dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventLayer, result);
                        });
                    }
                    else {
                        azmap.events.add(value.type as MapEventMouse, target, (mouseEvent: atlas.MapMouseEvent) => {
                            result.payload = { id: value.targetId, mouse: this.#buildMouseEventPayload(mouseEvent) };
                            dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventLayer, result);
                        });
                    }

                    wasAdded = true;
                }

                if (Helpers.isValueInEnum(MapEventWheel, value.type)) {
                    if (value.once) {
                        azmap.events.addOnce(value.type as MapEventWheel, target, (wheelEvent: atlas.MapMouseWheelEvent) => {
                            result.payload = { id: value.targetId, wheel: this.#buildWheelEventPayload(wheelEvent) };
                            dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventLayer, result);
                        });
                    }
                    else {
                        azmap.events.add(value.type as MapEventWheel, target, (wheelEvent: atlas.MapMouseWheelEvent) => {
                            result.payload = { id: value.targetId, wheel: this.#buildWheelEventPayload(wheelEvent) };
                            dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventLayer, result);
                        });
                    }

                    wasAdded = true;
                }

                if (wasAdded) {
                    Logger.logMessage(mapId, LogLevel.Trace, "event added:", value);
                }
                else {
                    Logger.logMessage(mapId, LogLevel.Error, "event not added - type not implemented:", value);
                }
            }
            else {
                Logger.logMessage(mapId, LogLevel.Error, "Events:addLayerEvents - invalid TargetId.", value);
            }
        });
    }

    static #removeLayerEvents(azmap: atlas.Map, mapId: string, events: MapEvent[]): void {
        if (events.length == 0) return;

        events.forEach((value) => {
            const target = azmap.layers.getLayerById(value.targetId);

            if (target) {
                azmap.events.remove(value.type, target, () => { });
                Logger.logMessage(mapId, LogLevel.Trace, "event removed:", value);
            }
            else {
                Logger.logMessage(mapId, LogLevel.Error, "Events:removeLayerEvents - invalid TargetId.", value);
            }
        });
    }
    // #endregion

    // #region ShapeEvents
    static #addShapeEvents(dotNetRef: any, azmap: atlas.Map, mapId: string, events: MapEvent[]): void {
        if (events.length == 0) return;

        Object.values(events).filter(value => Helpers.isValueInEnum(MapShapeEvent, value.type)).forEach((value) => {
            const ds = azmap.sources.getById(value.targetSourceId) as atlas.source.DataSource;
            const target = ds.getShapeById(value.targetId);

            if (target) {
                let result: MapEventInfo = { mapId: mapId, type: value.type };

                if (value.once) {
                    azmap.events.addOnce(value.type as MapShapeEvent, target, (shape: atlas.Shape) => {
                        result.payload = { id: shape.getId() };
                        dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventShape, result);
                    });
                }
                else {
                    azmap.events.add(value.type as MapShapeEvent, target, (shape: atlas.Shape) => {
                        result.payload = { id: shape.getId() };
                        dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventShape, result);
                    });
                }

                Logger.logMessage(mapId, LogLevel.Trace, "event added:", value);
            }
            else {
                Logger.logMessage(mapId, LogLevel.Error, "Events:addShapeEvents - invalid TargetId or TargetSourceId.", value);
            }

            Logger.logMessage(mapId, LogLevel.Trace, "event added:", value);
        });
    }

    static #removeShapeEvents(azmap: atlas.Map, mapId: string, events: MapEvent[]): void {
        events.forEach((value) => {
            const ds = azmap.sources.getById(value.targetSourceId) as atlas.source.DataSource;
            const target = ds.getShapeById(value.targetId);

            if (target) {
                azmap.events.remove(value.type, target, () => { });
                Logger.logMessage(mapId, LogLevel.Trace, "event removed:", value);
            }
            else {
                Logger.logMessage(mapId, LogLevel.Error, "Events:removeShapeEvents - invalid TargetId or TargetSourceId.", value);
            }
        });
    }
    // #endregion

    // #region StyleControlEvents
    static #addStyleControlEvents(dotNetRef: any, azmap: atlas.Map, mapId: string, events: MapEvent[]): void {
        if (events.length == 0) return;

        Object.values(events).filter(value => Helpers.isValueInEnum(MapStyleControlEvent, value.type)).forEach((value) => {
            const target = ControlManager.getControl(mapId, value.targetId) as atlas.control.StyleControl;

            if (target) {
                let result: MapEventInfo = { mapId: mapId, type: value.type };

                if (value.once) {
                    azmap.events.addOnce(value.type as MapStyleControlEvent, target, (style: string) => {
                        result.payload = { style: style };
                        dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventStyle, result);
                    });
                }
                else {
                    azmap.events.add(value.type as MapStyleControlEvent, target, (style: string) => {
                        result.payload = { style: style };
                        dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventStyle, result);
                    });
                }

                Logger.logMessage(mapId, LogLevel.Trace, "event added:", value);
            }
            else {
                Logger.logMessage(mapId, LogLevel.Error, "Events:addShapeEvents - invalid TargetId.", value);
            }

            Logger.logMessage(mapId, LogLevel.Trace, "event added:", value);
        });
    }

    static #removeStyleControlEvents(azmap: atlas.Map, mapId: string, events: MapEvent[]): void {
        events.forEach((value) => {
            const target = ControlManager.getControl(mapId, value.targetId) as atlas.control.StyleControl;

            if (target) {
                azmap.events.remove(value.type, target, () => { });
                Logger.logMessage(mapId, LogLevel.Trace, "event removed:", value);
            }
            else {
                Logger.logMessage(mapId, LogLevel.Error, "Events:removeStyleControlEvents - invalid TargetId.", value);
            }
        });
    }
    // #endregion

    // #region PopupEvents
    static #addPopupEvents(dotNetRef: any, azmap: atlas.Map, mapId: string, events: MapEvent[]): void {
        if (events.length == 0) return;

        Object.values(events).filter(value => Helpers.isValueInEnum(MapPopupEvent, value.type)).forEach((value) => {
            const target = PopupManager.getPopup(mapId, value.targetId);

            if (target) {
                let result: MapEventInfo = { mapId: mapId, type: value.type };

                if (value.once) {
                    azmap.events.addOnce(value.type as MapPopupEvent, target, (event: atlas.TargetedEvent) => {
                        result.payload = this.#buildTargetedEventPayload(event);
                        dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventPopup, result);
                    });
                }
                else {
                    azmap.events.add(value.type as MapPopupEvent, target, (event: atlas.TargetedEvent) => {
                        result.payload = this.#buildTargetedEventPayload(event);
                        dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventPopup, result);
                    });
                }

                Logger.logMessage(mapId, LogLevel.Trace, "event added:", value);
            }
            else {
                Logger.logMessage(mapId, LogLevel.Error, "Events:addPopupEvents - invalid TargetId.", value);
            }

            Logger.logMessage(mapId, LogLevel.Trace, "event added:", value);
        });
    }

    static #removePopupEvents(azmap: atlas.Map, mapId: string, events: MapEvent[]): void {
        if (events.length == 0) return;

        events.forEach((value) => {
            const target = PopupManager.getPopup(mapId, value.targetId);

            if (target) {
                azmap.events.remove(value.type, target, () => { });
                Logger.logMessage(mapId, LogLevel.Trace, "event removed:", value);
            }
            else {
                Logger.logMessage(mapId, LogLevel.Error, "Events:removePopupEvents - invalid TargetId.", value);
            }
        });
    }
    // #endregion

    static #isFeature(obj: any): obj is atlas.data.Feature<atlas.data.Geometry, any> {
        return Helpers.isFeature(obj);
    }

    static #isShape(obj: any): obj is atlas.Shape {
        return Helpers.isShape(obj);
    }

    static #getFeatureResult(feature: atlas.data.Feature<atlas.data.Geometry, any>): object {
        return Helpers.getFeatureResult(feature);
    }

    static #getShapeResult(shape: atlas.Shape): object {
        return Helpers.getShapeResult(shape);
    }

    static #buildShapeResults(shapes: Array<atlas.data.Feature<atlas.data.Geometry, any> | atlas.Shape>): object[] {
        return Helpers.buildShapeResults(shapes);
    }

    static #buildMouseEventPayload(mouseEvent: atlas.MapMouseEvent) {
        return {
            layerId: mouseEvent.layerId,
            pixel: mouseEvent.pixel,
            position: mouseEvent.position,
            shapes: this.#buildShapeResults(mouseEvent.shapes)
        };
    }

    static #buildTargetedEventPayload(event: atlas.TargetedEvent) {
        return {
            interop: (event.target as any).jsInterop
        };
    }

    static #buildTouchEventPayload(touchEvent: atlas.MapTouchEvent) {
        return {
            pixel: touchEvent.pixel,
            pixels: touchEvent.pixels,
            position: touchEvent.position,
            positions: touchEvent.positions,
            layerId: touchEvent.layerId,
            shapes: this.#buildShapeResults(touchEvent.shapes)
        };
    }

    static #buildWheelEventPayload(wheelEvent: atlas.MapMouseWheelEvent) {
        return {
            type: wheelEvent.type,
        };
    }
}

export interface MapEvent {
    type: string;
    once?: boolean;
    target: 'map' | 'datasource' | 'htmlmarker' | 'layer' | 'shape' | 'stylecontrol' | 'popup';
    targetId?: string;
    targetSourceId?: string;
}

export interface MapEventInfo {
    mapId: string;
    type: string;
    payload?: object;
}

export enum EventNotifications {
    NotifyMapEvent = 'NotifyMapEvent',
    NotifyMapEventConfig = 'NotifyMapEventConfig',
    NotifyMapEventData = 'NotifyMapEventData',
    NotifyMapEventDataSource = 'NotifyMapEventDataSource',
    NotifyMapEventError = 'NotifyMapEventError',
    NotifyMapEventHtmlMarker = 'NotifyMapEventHtmlMarker',
    NotifyMapEventLayer = 'NotifyMapEventLayer',
    NotifyMapEventMouse = 'NotifyMapEventMouse',
    NotifyMapEventPopup = "NotifyMapEventPopup",
    NotifyMapEventReady = 'NotifyMapEventReady',
    NotifyMapEventShape = 'NotifyMapEventShape',
    NotifyMapEventSource = 'NotifyMapEventSource',
    NotifyMapEventStyle = 'NotifyMapEventStyle',
    NotifyMapEventTouch = 'NotifyMapEventTouch',
    NotifyMapEventWheel = 'NotifyMapEventWheel',

    NotifyMapReady = 'NotifyMapReady',

}
export enum MapEventAdd {
    Error = "error",
    Ready = "ready"
}

enum MapEventGeneral {
    BoxZoomEnd = 'boxzoomend',
    BoxZoomStart = 'boxzoomstart',
    Drag = 'drag',
    DragEnd = 'dragend',
    DragStart = 'dragstart',
    Idle = 'idle',
    Load = 'load',
    Move = 'move',
    MoveEnd = 'moveend',
    MoveStart = 'movestart',
    Pitch = 'pitch',
    PitchEnd = 'pitchend',
    PitchStart = 'pitchstart',
    Render = 'render',
    Resize = 'resize',
    Rotate = 'rotate',
    RotateEnd = 'rotateend',
    RotateStart = 'rotatestart',
    TokenAcquired = 'tokenacquired',
    Zoom = 'zoom',
    ZoomEnd = 'zoomend',
    ZoomStart = 'zoomstart'
}

enum MapEventConfig {
    MapConfigChanged = 'mapconfigurationchanged',
}

enum MapEventData {
    Data = 'data',
    SourceData = 'sourcedata',
    StyleData = 'styledata',
}

enum MapEventDataSource {
    DataSourceUpdated = 'datasourceupdated',
    DataAdded = 'dataadded',
    DataRemoved = 'dataremoved',
}

enum MapEventLayer {
    LayerAdded = 'layeradded',
    LayerRemoved = 'layerremoved'
}

enum MapEventMouse {
    Click = 'click',
    ContextMenu = 'contextmenu',
    DblClick = 'dblclick',
    MouseDown = 'mousedown',
    MouseMove = 'mousemove',
    MouseOut = 'mouseout',
    MouseOver = 'mouseover',
    MouseUp = 'mouseup',
}

enum MapEventWheel {
    Wheel = 'wheel',
}

enum MapEventSource {
    SourceAdded = 'sourceadded',
    SourceRemoved = 'sourceremoved'
}

enum MapEventStyle {
    StyleChanged = 'stylechanged',
    StyleImageMissing = 'styleimagemissing',
}

enum MapEventTouch {
    TouchCancel = 'touchcancel',
    TouchEnd = 'touchend',
    TouchMove = 'touchmove',
    TouchStart = 'touchstart'
}

enum MapHtmlMarkerEvent {
    Click = 'click',
    ContextMenu = 'contextmenu',
    DblClick = 'dblclick',
    MouseDown = 'mousedown',
    MouseEnter = 'mouseenter',
    MouseLeave = 'mouseleave',
    MouseMove = 'mousemove',
    MouseOut = 'mouseout',
    MouseOver = 'mouseover',
    MouseUp = 'mouseup',

    Drag = 'drag',
    DragEnd = 'dragend',
    DragStart = 'dragstart',

    KeyDown = 'keydown',
    KeyPress = 'keypress',
    KeyUp = 'keyup',
}

enum MapLayerEvent {
    LayerAdded = 'layeradded',
    LayerRemoved = 'layerremoved',

    Click = 'click',
    ContextMenu = 'contextmenu',
    DblClick = 'dblclick',
    MouseDown = 'mousedown',
    MouseEnter = 'mouseenter',
    MouseLeave = 'mouseleave',
    MouseMove = 'mousemove',
    MouseOut = 'mouseout',
    MouseOver = 'mouseover',
    MouseUp = 'mouseup',

    TouchCancel = 'touchcancel',
    TouchEnd = 'touchend',
    TouchMove = 'touchmove',
    TouchStart = 'touchstart',

    Wheel = 'wheel',
}

//Events not included in MapEventMouse
enum MapLayerMouseEvent {
    MouseEnter = 'mouseenter',
    MouseLeave = 'mouseleave',
}

enum MapPopupEvent {
    Drag = 'drag',
    DragEnd = 'dragend',
    DragStart = 'dragstart',
    Open = 'open',
    Close = 'close',
}

enum MapShapeEvent {
    ShapeChanged = 'shapechanged',
}

enum MapStyleControlEvent {
    StyleSelected = 'styleselected',
}