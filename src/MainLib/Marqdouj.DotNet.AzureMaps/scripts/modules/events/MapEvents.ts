import * as atlas from "azure-maps-control"
import { Helpers } from "../common/Helpers";
import { IMapReference, MapEventDef } from "../typings";
import { EventsLogger } from "./EventsLogger";
import { EventsHelper } from "./EventsHelper";
import { EventNotification, MapEventMouse, MapEventTouch, MapEventWheel } from "../EventManager";

export class MapEvents {
    add(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#addMapEvents(mapRef, this.#getEventDefs(events));
    }

    remove(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#removeMapEvents(mapRef, this.#getEventDefs(events));
    }

    #getEventDefs(events: MapEventDef[]) {
        return Object.values(events).filter(value => value.target === "map" &&
            (
            Helpers.isValueInEnum(MapEventConfig, value.type) ||
            Helpers.isValueInEnum(MapEventData, value.type) ||
            Helpers.isValueInEnum(MapEventGeneral, value.type) ||
            Helpers.isValueInEnum(MapEventLayer, value.type) ||
            Helpers.isValueInEnum(MapEventSource, value.type) ||
            Helpers.isValueInEnum(MapEventStyle, value.type)
            ));
    }

    #addMapEvents(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#addMapEventConfig(mapRef, events);
        this.#addMapEventData(mapRef, events);
        this.#addMapEventGeneral(mapRef, events);
        this.#addMapEventLayer(mapRef, events);
        this.#addMapEventMouse(mapRef, events);
        this.#addMapEventSource(mapRef, events);
        this.#addMapEventStyle(mapRef, events);
        this.#addMapEventTouch(mapRef, events);
        this.#addMapEventWheel(mapRef, events);
    }

    #removeMapEvents(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#removeMapEventConfig(mapRef, events);
        this.#removeMapEventData(mapRef, events);
        this.#removeMapEventGeneral(mapRef, events);
        this.#removeMapEventLayer(mapRef, events);
        this.#removeMapEventMouse(mapRef, events);
        this.#removeMapEventSource(mapRef, events);
        this.#removeMapEventStyle(mapRef, events);
        this.#removeMapEventTouch(mapRef, events);
        this.#removeMapEventWheel(mapRef, events);
    }

    // #region Config
    #addMapEventConfig(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "addMapEventConfig";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventConfig, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            const callback = this.#getCallbackConfig(mapRef, value, false);

            if (callback) {
                if (value.once) {
                    EventsLogger.logOnceNotSupported(mapRef.mapId, eventName, value);
                }
                else {
                    mapRef.map.events.add(value.type as MapEventConfig, callback);
                    wasAdded = true;
                }
            }
            EventsLogger.logEventAdd(mapRef.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventConfig(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "removeMapEventConfig";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventConfig, value.type)).forEach((value) => {
            let wasRemoved: boolean = false;
            const callback = this.#getCallbackConfig(mapRef, value, true);

            if (callback) {
                mapRef.map.events.remove(value.type, callback);
                wasRemoved = true;
            }
            EventsLogger.logEventRemoved(mapRef.mapId, eventName, wasRemoved, value);
        });
    }

    #getCallbackConfig(mapRef: IMapReference, value: MapEventDef, removing: boolean) {
        let callback: any = mapRef.eventsMap.getCallback(value, removing);

        if (callback) {
            return callback;
        }

        callback = (config: atlas.MapConfiguration) => this.#notifyNotifyMapEventConfig(config, mapRef, value);

        mapRef.eventsMap.addCallback(value, callback);

        return callback;
    }

    #notifyNotifyMapEventConfig = (callback: atlas.MapConfiguration, mapRef: IMapReference, event: MapEventDef) => {
        let result = EventsHelper.buildEventResult(mapRef.mapId, event, callback);
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEventConfig, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEventConfig, event.type);
    };
    // #endregion

    // #region Data
    #addMapEventData(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "addMapEventData";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventData, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            const callback = this.#getCallbackData(mapRef, value, false);

            if (callback) {
                if (value.once) {
                    mapRef.map.events.addOnce(value.type as MapEventData, callback);
                }
                else {
                    mapRef.map.events.add(value.type as MapEventData, callback);
                }
                wasAdded = true;
            }

            EventsLogger.logEventAdd(mapRef.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventData(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "removeMapEventData";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventData, value.type)).forEach((value) => {
            let wasRemoved: boolean = false;
            const callback = this.#getCallbackData(mapRef, value, true);

            if (callback) {
                mapRef.map.events.remove(value.type, callback);
                wasRemoved = true;
            }

            EventsLogger.logEventRemoved(mapRef.mapId, eventName, wasRemoved, value);
        });
    }

    #getCallbackData(mapRef: IMapReference, value: MapEventDef, removing: boolean) {
        let callback: any = mapRef.eventsMap.getCallback(value, removing);

        if (callback) {
            return callback;
        }

        callback = (dataEvent: atlas.MapDataEvent) => this.#notifyMapEventData(dataEvent, mapRef, value);

        mapRef.eventsMap.addCallback(value, callback);

        return callback;
    }

    #notifyMapEventData = (callback: atlas.MapDataEvent, mapRef: IMapReference, event: MapEventDef) => {
        let result = EventsHelper.buildEventResult(mapRef.mapId, event, this.#buildMapDataEventPayload(callback));
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEventData, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEventData, event.type);
    };

    #buildMapDataEventPayload(dataEvent: atlas.MapDataEvent) {
        return {
            dataType: dataEvent.dataType,
            isSourceLoaded: dataEvent.isSourceLoaded,
            source: dataEvent.source?.getId(),
            sourceDataType: dataEvent.sourceDataType,
            tile: dataEvent.tile
        };
    }
    // #endregion

    // #region General
    #addMapEventGeneral(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "addMapEventGeneral";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventGeneral, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            const callback = this.#getCallbackGeneral(mapRef, value, false);

            if (callback) {
                if (value.once) {
                    mapRef.map.events.addOnce(value.type as MapEventGeneral, callback);
                }
                else {
                    mapRef.map.events.add(value.type as MapEventGeneral, callback);
                }
                wasAdded = true;
            }

            EventsLogger.logEventAdd(mapRef.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventGeneral(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "removeMapEventGeneral";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventGeneral, value.type)).forEach((value) => {
            let wasRemoved = false;
            const callback = this.#getCallbackGeneral(mapRef, value, true);

            if (callback) {
                mapRef.map.events.remove(value.type, callback);
                wasRemoved = true;
            }

            EventsLogger.logEventRemoved(mapRef.mapId, eventName, wasRemoved, value);
        });
    }

    #getCallbackGeneral(mapRef: IMapReference, value: MapEventDef, removing: boolean) {
        let callback: any = mapRef.eventsMap.getCallback(value, removing);

        if (callback) {
            return callback;
        }

        callback = (callback: atlas.MapEvent) => { this.#notifyMapEventGeneral(callback, mapRef, value); };

        mapRef.eventsMap.addCallback(value, callback);

        return callback;
    }

    #notifyMapEventGeneral = (callback: atlas.MapEvent, mapRef: IMapReference, event: MapEventDef) => {
        let result = EventsHelper.buildEventResult(mapRef.mapId, event);
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEvent, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEvent, event.type);
    };
    // #endregion

    // #region Layer
    #addMapEventLayer(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "addMapEventLayer";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventLayer, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            const callback = this.#getCallbackLayer(mapRef, value, false);

            if (callback) {
                if (value.once) {
                    mapRef.map.events.addOnce(value.type as MapEventLayer, callback);
                }
                else {
                    mapRef.map.events.add(value.type as MapEventLayer, callback);
                }
                wasAdded = true;
            }

            EventsLogger.logEventAdd(mapRef.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventLayer(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "removeMapEventLayer";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventLayer, value.type)).forEach((value) => {
            let wasRemoved = false;
            const callback = this.#getCallbackLayer(mapRef, value, true);

            if (callback) {
                mapRef.map.events.remove(value.type, callback);
                wasRemoved = true;
            }

            EventsLogger.logEventRemoved(mapRef.mapId, eventName, wasRemoved, value);
        });
    }

    #getCallbackLayer(mapRef: IMapReference, value: MapEventDef, removing: boolean) {
        let callback: any = mapRef.eventsMap.getCallback(value, removing);

        if (callback) {
            return callback;
        }

        callback = (callback: atlas.layer.Layer) => { this.#notifyMapEventLayer(callback, mapRef, value); };

        mapRef.eventsMap.addCallback(value, callback);

        return callback;
    }

    #notifyMapEventLayer = (callback: atlas.layer.Layer, mapRef: IMapReference, event: MapEventDef) => {
        let result = EventsHelper.buildEventResult(mapRef.mapId, event, EventsHelper.buildLayerPayload(callback));
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEventGeneral, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEventGeneral, event.type);
    };
    // #endregion

    // #region Mouse
    #addMapEventMouse(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "addMapEventMouse";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventMouse, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            let callback = this.#getCallBackMouse(mapRef, value, false);

            if (callback) {
                if (value.once) {
                    mapRef.map.events.addOnce(value.type as MapEventMouse, callback);
                }
                else {
                    mapRef.map.events.add(value.type as MapEventMouse, callback);
                }
                wasAdded = true;
            }

            EventsLogger.logEventAdd(mapRef.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventMouse(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "removeMapEventMouse";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventMouse, value.type)).forEach((value) => {
            let wasRemoved: boolean = false;
            let callback = this.#getCallBackMouse(mapRef, value, true);

            if (callback) {
                mapRef.map.events.remove(value.type, callback);
                wasRemoved = true;
            }

            EventsLogger.logEventRemoved(mapRef.mapId, eventName, wasRemoved, value);
        });
    }

    #getCallBackMouse(mapRef: IMapReference, value: MapEventDef, removing: boolean) {
        let callback: any = mapRef.eventsMap.getCallback(value, removing);

        if (callback) {
            return callback;
        }

        callback = (callback: atlas.MapMouseEvent) => this.#notifyMapEventMouse(callback, mapRef, value);

        mapRef.eventsMap.addCallback(value, callback);
        return callback;
    }

    #notifyMapEventMouse = (callback: atlas.MapMouseEvent, mapRef: IMapReference, event: MapEventDef) => {
        if (event.preventDefault)
            callback.preventDefault();
        let payload = EventsHelper.buildMouseEventPayload(callback);
        let result = EventsHelper.buildEventResult(mapRef.mapId, event, payload);
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEventMouse, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEventMouse, event.type);
    };

    // #endregion

    // #region Source
    #addMapEventSource(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "addMapEventSource";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventSource, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            const callback = this.#getCallbackSource(mapRef, value, false);

            if (callback) {
                if (value.once) {
                    mapRef.map.events.addOnce(value.type as MapEventSource, callback);
                }
                else {
                    mapRef.map.events.add(value.type as MapEventSource, callback);
                }
                wasAdded = true;
            }

            EventsLogger.logEventAdd(mapRef.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventSource(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "removeMapEventSource";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventSource, value.type)).forEach((value) => {
            let wasRemoved: boolean = false;
            const callback = this.#getCallbackSource(mapRef, value, true);

            if (callback) {
                mapRef.map.events.remove(value.type, callback);
                wasRemoved = true;
            }

            EventsLogger.logEventRemoved(mapRef.mapId, eventName, wasRemoved, value);
        });
    }

    #getCallbackSource(mapRef: IMapReference, value: MapEventDef, removing: boolean) {
        let callback: any = mapRef.eventsMap.getCallback(value, removing);

        if (callback) {
            return callback;
        }

        callback = (source: atlas.source.Source) => this.#notifyMapEventSource(source, mapRef, value);

        mapRef.eventsMap.addCallback(value, callback);

        return callback;
    }

    #notifyMapEventSource(source: atlas.source.Source, mapRef: IMapReference, event: MapEventDef) {
        let payload = { id: source.getId(), jsInterop: Helpers.getJsInterop(source) };
        let result = EventsHelper.buildEventResult(mapRef.mapId, event, payload);
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEventSource, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEventSource, event.type);
    }
    // #endregion

    // #region Style
    #addMapEventStyle(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "addMapEventStyle";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventStyle, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            const callback = this.#getCallbackStyle(mapRef, value, false);

            if (callback) {
                if (value.once) {
                    mapRef.map.events.addOnce(value.type as any, callback);
                }
                else {
                    mapRef.map.events.add(value.type as any, callback);
                }
                wasAdded = true;
            }

            EventsLogger.logEventAdd(mapRef.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventStyle(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "removeMapEventStyle";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventStyle, value.type)).forEach((value) => {
            let wasRemoved: boolean = false;
            const callback = this.#getCallbackStyle(mapRef, value, true);

            if (callback) {
                mapRef.map.events.remove(value.type, callback);
                wasRemoved = true;
            }

            EventsLogger.logEventRemoved(mapRef.mapId, eventName, wasRemoved, value);
        });
    }

    #getCallbackStyle(mapRef: IMapReference, value: MapEventDef, removing: boolean) {
        let callback: any = mapRef.eventsMap.getCallback(value, removing);

        if (callback) {
            return callback;
        }

        switch (value.type.toLowerCase()) {
            case MapEventStyle.StyleChanged:
                callback = (source: atlas.StyleChangedEvent) => this.#notifyMapEventStyle(source.style, mapRef, value);
                break;
            case MapEventStyle.StyleImageMissing:
                callback = (style: string) => this.#notifyMapEventStyle(style, mapRef, value);
                break;
            default:
        }

        mapRef.eventsMap.addCallback(value, callback);

        return callback;
    }

    #notifyMapEventStyle(style: string, mapRef: IMapReference, event: MapEventDef) {
        let payload = { style: style };
        let result = EventsHelper.buildEventResult(mapRef.mapId, event, payload);
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEventStyle, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEventStyle, event.type);
    }
    // #endregion

    // #region Touch
    #addMapEventTouch(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "addMapEventTouch";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventTouch, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            const callback = this.#getCallbackTouch(mapRef, value, false);

            if (callback) {
                if (value.once) {
                    mapRef.map.events.addOnce(value.type as MapEventTouch, callback);
                }
                else {
                    mapRef.map.events.add(value.type as MapEventTouch, callback);
                }
                wasAdded = true;
            }

            EventsLogger.logEventAdd(mapRef.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventTouch(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "removeMapEventTouch";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventTouch, value.type)).forEach((value) => {
            let wasRemoved: boolean = false;
            const callback = this.#getCallbackTouch(mapRef, value, true);

            if (callback) {
                mapRef.map.events.remove(value.type, callback);
                wasRemoved = true;
            }

            EventsLogger.logEventRemoved(mapRef.mapId, eventName, wasRemoved, value);
        });
    }

    #getCallbackTouch(mapRef: IMapReference, value: MapEventDef, removing: boolean) {
        let callback: any = mapRef.eventsMap.getCallback(value, removing);

        if (callback) {
            return callback;
        }

        callback = (callback: atlas.MapTouchEvent) => this.#notifyMapEventTouch(callback, mapRef, value);

        mapRef.eventsMap.addCallback(value, callback);

        return callback;
    }

    #notifyMapEventTouch = (callback: atlas.MapTouchEvent, mapRef: IMapReference, event: MapEventDef) => {
        if (event.preventDefault)
            callback.preventDefault();
        let payload = EventsHelper.buildTouchEventPayload(callback);
        let result = EventsHelper.buildEventResult(mapRef.mapId, event, payload);
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEventTouch, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEventTouch, event.type);
    };

    // #endregion

    // #region Wheel
    #addMapEventWheel(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "addMapEventWheel";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventWheel, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            let callback = this.#getCallbackWheel(mapRef, value, false);

            if (callback) {
                if (value.once) {
                    mapRef.map.events.addOnce(value.type as MapEventWheel, callback);
                }
                else {
                    mapRef.map.events.add(value.type as MapEventWheel, callback);
                }
                wasAdded = true;
            }

            EventsLogger.logEventAdd(mapRef.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventWheel(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "removeMapEventWheel";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventWheel, value.type)).forEach((value) => {
            let wasRemoved: boolean = false;
            let callback = this.#getCallbackWheel(mapRef, value, true);

            if (callback) {
                mapRef.map.events.remove(value.type, callback);
                wasRemoved = true;
            }

            EventsLogger.logEventRemoved(mapRef.mapId, eventName, wasRemoved, value);
        });
    }

    #getCallbackWheel(mapRef: IMapReference, value: MapEventDef, removing: boolean) {
        let callback: any = mapRef.eventsMap.getCallback(value, removing);

        if (callback) {
            return callback;
        }

        callback = (callback: atlas.MapMouseWheelEvent) => this.#notifyMapEventWheel(callback, mapRef, value);

        mapRef.eventsMap.addCallback(value, callback);

        return callback;
    }

    #notifyMapEventWheel = (callback: atlas.MapMouseWheelEvent, mapRef: IMapReference, event: MapEventDef) => {
        if (event.preventDefault)
            callback.preventDefault();
        let payload = EventsHelper.buildWheelEventPayload(callback);
        let result = EventsHelper.buildEventResult(mapRef.mapId, event, payload);
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEventWheel, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEventWheel, event.type);
    };

    // #endregion
}

enum MapEventConfig {
    MapConfigChanged = 'mapconfigurationchanged',
}

enum MapEventData {
    Data = 'data',
    SourceData = 'sourcedata',
    StyleData = 'styledata',
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

enum MapEventLayer {
    LayerAdded = 'layeradded',
    LayerRemoved = 'layerremoved'
}

enum MapEventSource {
    SourceAdded = 'sourceadded',
    SourceRemoved = 'sourceremoved'
}

enum MapEventStyle {
    StyleChanged = 'stylechanged',
    StyleImageMissing = 'styleimagemissing',
}
