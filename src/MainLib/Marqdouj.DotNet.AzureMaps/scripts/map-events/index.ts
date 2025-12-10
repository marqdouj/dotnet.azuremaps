import * as atlas from "azure-maps-control";
import { MapFactory } from "../map-factory"
import { Logger, Extensions } from "../common"

export class EventManager {
    static addEvents(dotNetRef: any, mapId: string, events: MapEvent[]): void {
        const azmap = MapFactory.getMap(mapId);

        if (!azmap) {
            Logger.logMessage(mapId, Logger.LogLevel.Error, "EventManager:addEvents - map not found.");
            return;
        }

        events ??= [];

        Logger.logMessage(mapId, Logger.LogLevel.Trace, "addEvents", events, Object.values(events).filter(value => value.target === "map"));

        this.#addMapEvents(dotNetRef, azmap, mapId, Object.values(events).filter(value => value.target === "map"));
        this.#addDataSourceEvents(dotNetRef, azmap, mapId, Object.values(events).filter(value => value.target === "datasource"));
    }

    static removeEvents(mapId: string, events: MapEvent[]): void {
        const azmap = MapFactory.getMap(mapId);

        if (!azmap) {
            Logger.logMessage(mapId, Logger.LogLevel.Error, "EventManager:removeEvents - map not found.");
            return;
        }

        events ??= [];

        this.#removeMapEvents(azmap, mapId, Object.values(events).filter(value => value.target === "map"));
        this.#removeDataSourceEvents(azmap, mapId, Object.values(events).filter(value => value.target === "datasource"));
    }

    static #addMapEvents(dotNetRef: any, azmap: atlas.Map, mapId: string, events: MapEvent[]): void {
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
        Object.values(events).filter(value => Extensions.isValueInEnum(MapEventConfig, value.type)).forEach((value) => {

            if (value.once) {
                //not supported.
                Logger.logMessage(mapId, Logger.LogLevel.Error, "event not added. 'once' is not supported:", value);
            }
            else {
                azmap.events.add(value.type as MapEventConfig, (config: atlas.MapConfiguration) => {
                    const result: MapEventInfo = { mapId: mapId, type: value.type, payload: config };
                    dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventConfig, result);
                });
            }

            Logger.logMessage(mapId, Logger.LogLevel.Trace, "event added:", value);
        });
    }

    static #addMapEventsData(dotNetRef: any, azmap: atlas.Map, mapId: string, events: MapEvent[]) {
        Object.values(events).filter(value => Extensions.isValueInEnum(MapEventData, value.type)).forEach((value) => {
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

            Logger.logMessage(mapId, Logger.LogLevel.Trace, "event added:", value);
        });
    }

    static #addMapEventsGeneral(dotNetRef: any, azmap: atlas.Map, mapId: string, events: MapEvent[]): void {
        Object.values(events).filter(value => Extensions.isValueInEnum(MapEventGeneral, value.type)).forEach((value) => {
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

            Logger.logMessage(mapId, Logger.LogLevel.Trace, "event added:", value);
        });
    }

    static #addMapEventsLayer(dotNetRef: any, azmap: atlas.Map, mapId: string, events: MapEvent[]) {
        Object.values(events).filter(value => Extensions.isValueInEnum(MapEventLayer, value.type)).forEach((value) => {
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

            Logger.logMessage(mapId, Logger.LogLevel.Trace, "event added:", value);
        });
    }

    static #addMapEventsMouse(dotNetRef: any, azmap: atlas.Map, mapId: string, events: MapEvent[]) {
        Object.values(events).filter(value => Extensions.isValueInEnum(MapEventMouse, value.type)).forEach((value) => {
            let result: MapEventInfo = { mapId: mapId, type: value.type };

            if (value.once) {
                azmap.events.addOnce(value.type as MapEventMouse, (mouseEvent: atlas.MapMouseEvent) => {
                    result.payload = {
                        layerId: mouseEvent.layerId,
                        pixel: mouseEvent.pixel,
                        position: mouseEvent.position,
                        shapes: this.#buildShapeResults(mouseEvent.shapes)
                    };
                    dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventMouse, result);
                });
            }
            else {
                azmap.events.add(value.type as MapEventMouse, (mouseEvent: atlas.MapMouseEvent) => {
                    result.payload = {
                        layerId: mouseEvent.layerId,
                        pixel: mouseEvent.pixel,
                        position: mouseEvent.position,
                        shapes: this.#buildShapeResults(mouseEvent.shapes)
                    };
                    dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventMouse, result);
                });
            }

            Logger.logMessage(mapId, Logger.LogLevel.Trace, "event added:", value);
        });
    }

    static #addMapEventsSource(dotNetRef: any, azmap: atlas.Map, mapId: string, events: MapEvent[]) {
        Object.values(events).filter(value => Extensions.isValueInEnum(MapEventSource, value.type)).forEach((value) => {
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

            Logger.logMessage(mapId, Logger.LogLevel.Trace, "event added:", value);
        });
    }

    static #addMapEventsStyle(dotNetRef: any, azmap: atlas.Map, mapId: string, events: MapEvent[]) {
        Object.values(events).filter(value => Extensions.isValueInEnum(MapEventStyle, value.type)).forEach((value) => {
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

            Logger.logMessage(mapId, Logger.LogLevel.Trace, "event added:", value);
        });
    }

    static #addMapEventsTouch(dotNetRef: any, azmap: atlas.Map, mapId: string, events: MapEvent[]) {
        Object.values(events).filter(value => Extensions.isValueInEnum(MapEventTouch, value.type)).forEach((value) => {
            let result: MapEventInfo = { mapId: mapId, type: value.type };

            if (value.once) {
                azmap.events.addOnce(value.type as MapEventTouch, (touchEvent: atlas.MapTouchEvent) => {
                    result.payload = {
                        pixel: touchEvent.pixel,
                        pixels: touchEvent.pixels,
                        position: touchEvent.position,
                        positions: touchEvent.positions,
                        layerId: touchEvent.layerId,
                        shapes: this.#buildShapeResults(touchEvent.shapes)
                    };
                    dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventTouch, result);
                });
            }
            else {
                azmap.events.add(value.type as MapEventTouch, (touchEvent: atlas.MapTouchEvent) => {
                    result.payload = {
                        pixel: touchEvent.pixel,
                        pixels: touchEvent.pixels,
                        position: touchEvent.position,
                        positions: touchEvent.positions,
                        layerId: touchEvent.layerId,
                        shapes: this.#buildShapeResults(touchEvent.shapes)
                    };
                    dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventTouch, result);
                });
            }

            Logger.logMessage(mapId, Logger.LogLevel.Trace, "event added:", value);
        });
    }

    static #addMapEventsWheel(dotNetRef: any, azmap: atlas.Map, mapId: string, events: MapEvent[]) {
        Object.values(events).filter(value => Extensions.isValueInEnum(MapEventWheel, value.type)).forEach((value) => {
            let result: MapEventInfo = { mapId: mapId, type: value.type };

            if (value.once) {
                azmap.events.addOnce(value.type as MapEventWheel, (wheelEvent: atlas.MapMouseWheelEvent) => {
                    result.payload = { type: wheelEvent.type };
                    dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventWheel, result);
                });
            }
            else {
                azmap.events.add(value.type as MapEventWheel, (wheelEvent: atlas.MapMouseWheelEvent) => {
                    result.payload = { type: wheelEvent.type };
                    dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventWheel, result);
                });
            }

            Logger.logMessage(mapId, Logger.LogLevel.Trace, "event added:", value);
        });
    }

    static #removeMapEvents(azmap: atlas.Map, mapId: string, events: MapEvent[]): void {
        events.forEach((value) => {
            azmap.events.remove(value.type, () => { });
            Logger.logMessage(mapId, Logger.LogLevel.Trace, "event removed:", value);
        });
    }

    static #addDataSourceEvents(dotNetRef: any, azmap: atlas.Map, mapId: string, events: MapEvent[]): void {
        Object.values(events).filter(value => Extensions.isValueInEnum(MapEventDataSource, value.type)).forEach((value) => {
            const ds = azmap.sources.getById(value.targetId) as atlas.source.DataSource;

            if (ds) {
                let dsShapes: object[] = [];
                let payload: { sourceId: string, shapes: object[] };
                payload.sourceId = value.targetId;
                payload.shapes = dsShapes;

                let result: MapEventInfo = { mapId: mapId, type: value.type, payload: payload };

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
                }

                Logger.logMessage(mapId, Logger.LogLevel.Trace, "event added:", value);
            }
            else {
                Logger.logMessage(mapId, Logger.LogLevel.Error, "EventManager:addDataSourceEvents - invalid TargetId.", value);
            }
        });
    }

    static #removeDataSourceEvents(azmap: atlas.Map, mapId: string, events: MapEvent[]): void {
        events.forEach((value) => {
            const ds = azmap.sources.getById(value.targetId) as atlas.source.DataSource;

            if (ds) {
                azmap.events.remove(value.type, ds, () => { });
                Logger.logMessage(mapId, Logger.LogLevel.Trace, "event removed:", value);
            }
            else {
                Logger.logMessage(mapId, Logger.LogLevel.Error, "EventManager:removeDataSourceEvents - invalid TargetId.", value);
            }
        });
    }

    static #isFeature(obj: any): obj is atlas.data.Feature<atlas.data.Geometry, any> {
        return obj && obj.type === 'Feature';
    }

    static #isShape(obj: any): obj is atlas.Shape {
        return obj && obj.getType != undefined;
    }

    static #getFeatureResult(feature: atlas.data.Feature<atlas.data.Geometry, any>): object {

        const item: object = {
            id: feature.id?.toString(),
            type: feature.geometry.type,
            bbox: feature.bbox,
            source: "feature",
            properties: feature.properties
        };
        return item;
    }

    static #getShapeResult(shape: atlas.Shape): object {
        const item: object = {
            id: shape.getId()?.toString(),
            type: shape.getType(),
            bbox: shape.getBounds(),
            source: "shape",
            properties: shape.getProperties()
        };
        return item;
    }

    static #buildShapeResults(shapes: Array<atlas.data.Feature<atlas.data.Geometry, any> | atlas.Shape>): object[] {
        const results: object[] = [];

        shapes.filter(feature => this.#isFeature(feature)).forEach(feature => {
            results.push(this.#getFeatureResult(feature));
        });
        shapes.filter(shape => this.#isShape(shape)).forEach(shape => {
            results.push(this.#getShapeResult(shape));
        });

        return results;
    }
}

export interface MapEvent {
    type: string;
    once?: boolean;
    target: 'map' | 'datasource' | 'shape' | 'layer' | 'stylecontrol' | 'popup' | 'htmlmarker';
    targetId?: string;
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
    NotifyMapEventLayer = 'NotifyMapEventLayer',
    NotifyMapEventMouse = 'NotifyMapEventMouse',
    NotifyMapEventReady = 'NotifyMapEventReady',
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
    //StyleSelected = 'styleselected',
}

//enum MapEventStyleChanged {
//    StyleChanged = 'stylechanged',
//}

enum MapEventTouch {
    TouchCancel = 'touchcancel',
    TouchEnd = 'touchend',
    TouchMove = 'touchmove',
    TouchStart = 'touchstart'
}
