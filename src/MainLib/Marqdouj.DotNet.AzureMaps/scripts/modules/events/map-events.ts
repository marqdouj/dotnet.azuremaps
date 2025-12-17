import * as atlas from "azure-maps-control"
import { Helpers } from "../helpers"
import { EventNotifications, MapEventLogger } from "./common"
import { MapEventMouse, MapEventTouch, MapEventWheel } from "./common"
import { EventFactoryBase } from "./EventFactoryBase";

export class MapEventFactory extends EventFactoryBase {
    constructor(mapId: string) {
        super(mapId);
    }
   
    addEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#addMapEvents(Object.values(events).filter(value => value.target === "map"));
    }

    removeEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#removeMapEvents(Object.values(events).filter(value => value.target === "map"));
    }

    #addMapEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#addMapEventConfig(events);
        this.#addMapEventData(events);
        this.#addMapEventGeneral(events);
        this.#addMapEventLayer(events);
        this.#addMapEventMouse(events);
        this.#addMapEventSource(events);
        this.#addMapEventStyle(events);
        this.#addMapEventTouch(events);
        this.#addMapEventWheel(events);
    }

    #removeMapEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#removeMapEventConfig(events);
        this.#removeMapEventData(events);
        this.#removeMapEventGeneral(events);
        this.#removeMapEventLayer(events);
        this.#removeMapEventMouse(events);
        this.#removeMapEventSource(events);
        this.#removeMapEventStyle(events);
        this.#removeMapEventTouch(events);
        this.#removeMapEventWheel(events);
    }

    // #region Config
    #addMapEventConfig(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "addMapEventConfig";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventConfig, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            const callback = this.#getCallbackConfig(value, false);

            if (callback) {
                if (value.once) {
                    MapEventLogger.logOnceNotSupported(this.mapId, eventName, value);
                }
                else {
                    azmap.events.add(value.type as MapEventConfig, callback);
                    wasAdded = true;
                }
            }
            MapEventLogger.logEventAdd(this.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventConfig(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "removeMapEventConfig";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventConfig, value.type)).forEach((value) => {
            let wasRemoved: boolean = false;
            const callback = this.#getCallbackConfig(value, true);

            if (callback) {
                azmap.events.remove(value.type, callback);
                wasRemoved = true;
            }
            MapEventLogger.logEventRemoved(this.mapId, eventName, wasRemoved, value);
        });
    }

    #getCallbackConfig(value: MapEventDef, removing: boolean) {
        let callback: any = this.getCallback(value, removing);

        if (callback) {
            return callback;
        }

        callback = (config: atlas.MapConfiguration) => this.#notifyNotifyMapEventConfig(config, value);

        this.addCallback(value, callback);

        return callback;
    }

    #notifyNotifyMapEventConfig = (config: atlas.MapConfiguration, event: MapEventDef) => {
        let result = Helpers.buildEventResult(this.mapId, event, config);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventConfig, result);
        MapEventLogger.logNotifyFired(this.mapId, EventNotifications.NotifyMapEventConfig, event.type);
    };

    // #endregion

    // #region Data
    #addMapEventData(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "addMapEventData";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventData, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            const callback = this.#getCallbackData(value, false);

            if (callback) {
                if (value.once) {
                    azmap.events.addOnce(value.type as MapEventData, callback);
                }
                else {
                    azmap.events.add(value.type as MapEventData, callback);
                }
                wasAdded = true;
            }

            MapEventLogger.logEventAdd(this.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventData(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "removeMapEventData";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventData, value.type)).forEach((value) => {
            let wasRemoved: boolean = false;
            const callback = this.#getCallbackData(value, true);

            if (callback) {
                azmap.events.remove(value.type, callback);
                wasRemoved = true;
            }

            MapEventLogger.logEventRemoved(this.mapId, eventName, wasRemoved, value);
        });
    }

    #getCallbackData(value: MapEventDef, removing: boolean) {
        let callback: any = this.getCallback(value, removing);

        if (callback) {
            return callback;
        }

        callback = (dataEvent: atlas.MapDataEvent) => this.#notifyMapEventData(dataEvent, value);

        this.addCallback(value, callback);

        return callback;
    }

    #notifyMapEventData = (callback: atlas.MapDataEvent, event: MapEventDef) => {
        let result = Helpers.buildEventResult(this.mapId, event, this.#buildMapDataEventPayload(callback));
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventData, result);
        MapEventLogger.logNotifyFired(this.mapId, EventNotifications.NotifyMapEventData, event.type);
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
    #addMapEventGeneral(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "addMapEventGeneral";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventGeneral, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            const callback = this.#getCallbackGeneral(value, false);

            if (callback) {
                if (value.once) {
                    azmap.events.addOnce(value.type as MapEventGeneral, callback);
                }
                else {
                    azmap.events.add(value.type as MapEventGeneral, callback);
                }
                wasAdded = true;
            }

            MapEventLogger.logEventAdd(this.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventGeneral(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "removeMapEventGeneral";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventGeneral, value.type)).forEach((value) => {
            let wasRemoved = false;
            const callback = this.#getCallbackGeneral(value, true);

            if (callback) {
                azmap.events.remove(value.type, callback);
                wasRemoved = true;
            }
            
            MapEventLogger.logEventRemoved(this.mapId, eventName, wasRemoved, value);
        });
    }

    #getCallbackGeneral(value: MapEventDef, removing: boolean) {
        let callback: any = this.getCallback(value, removing);

        if (callback) {
            return callback;
        }

        callback = () => { this.#notifyMapEventGeneral(value); };

        this.addCallback(value, callback);

        return callback;
    }

    #notifyMapEventGeneral = (event: MapEventDef) => {
        let result = Helpers.buildEventResult(this.mapId, event, null);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEvent, result);
        MapEventLogger.logNotifyFired(this.mapId, EventNotifications.NotifyMapEvent, event.type);
    };
    // #endregion

    // #region Layer
    #addMapEventLayer(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "addMapEventLayer";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventLayer, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            const callback = this.#getCallbackLayer(value, false);

            if (callback) {
                if (value.once) {
                    azmap.events.addOnce(value.type as MapEventLayer, callback);
                }
                else {
                    azmap.events.add(value.type as MapEventLayer, callback);
                }
                wasAdded = true;
            }

            MapEventLogger.logEventAdd(this.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventLayer(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "removeMapEventLayer";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventLayer, value.type)).forEach((value) => {
            let wasRemoved = false;
            const callback = this.#getCallbackLayer(value, true);

            if (callback) {
                azmap.events.remove(value.type, callback);
                wasRemoved = true;
            }

            MapEventLogger.logEventRemoved(this.mapId, eventName, wasRemoved, value);
        });
    }

    #getCallbackLayer(value: MapEventDef, removing: boolean) {
        let callback: any = this.getCallback(value, removing);

        if (callback) {
            return callback;
        }

        callback = () => { this.#notifyMapEventLayer(value); };

        this.addCallback(value, callback);

        return callback;
    }

    #notifyMapEventLayer = (event: MapEventDef) => {
        let result = Helpers.buildEventResult(this.mapId, event, null);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEvent, result);
        MapEventLogger.logNotifyFired(this.mapId, EventNotifications.NotifyMapEvent, event.type);
    };
    // #endregion

    // #region Mouse
    #addMapEventMouse(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "addMapEventMouse";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventMouse, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            let callback = this.#getCallBackMouse(value, false);

            if (callback) {
                if (value.once) {
                    azmap.events.addOnce(value.type as MapEventMouse, callback);
                }
                else {
                    azmap.events.add(value.type as MapEventMouse, callback);
                }
                wasAdded = true;
            }

            MapEventLogger.logEventAdd(this.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventMouse(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "removeMapEventMouse";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventMouse, value.type)).forEach((value) => {
            let wasRemoved: boolean = false;
            let callback = this.#getCallBackMouse(value, true);

            if (callback) {
                azmap.events.remove(value.type, callback);
                wasRemoved = true;
            }

            MapEventLogger.logEventRemoved(this.mapId, eventName, wasRemoved, value);
        });
    }

    #getCallBackMouse(value: MapEventDef, removing: boolean) {
        let callback: any = this.getCallback(value, removing);

        if (callback) {
            return callback;
        }

        callback = (callback: atlas.MapMouseEvent) => this.#notifyMapEventMouse(callback, value);

        this.addCallback(value, callback);
        return callback;
    }

    #notifyMapEventMouse = (callback: atlas.MapMouseEvent, event: MapEventDef) => {
        if (event.preventDefault)
            callback.preventDefault();
        let payload = Helpers.buildMouseEventPayload(callback);
        let result = Helpers.buildEventResult(this.mapId, event, payload);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventMouse, result);
        MapEventLogger.logNotifyFired(this.mapId, EventNotifications.NotifyMapEventMouse, event.type);
    };

    // #endregion

    // #region Source
    #addMapEventSource(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "addMapEventSource";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventSource, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            const callback = this.#getCallbackSource(value, false);

            if (callback) {
                if (value.once) {
                    azmap.events.addOnce(value.type as MapEventSource, callback);
                }
                else {
                    azmap.events.add(value.type as MapEventSource, callback);
                }
                wasAdded = true;
            }

            MapEventLogger.logEventAdd(this.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventSource(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "removeMapEventSource";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventSource, value.type)).forEach((value) => {
            let wasRemoved: boolean = false;
            const callback = this.#getCallbackSource(value, true);

            if (callback) {
                azmap.events.remove(value.type, callback);
                wasRemoved = true;
            }

            MapEventLogger.logEventRemoved(this.mapId, eventName, wasRemoved, value);
        });
    }

    #getCallbackSource(value: MapEventDef, removing: boolean) {
        let callback: any = this.getCallback(value, removing);

        if (callback) {
            return callback;
        }

        callback = (source: atlas.source.Source) => this.#notifyMapEventSource(source, value);

        this.addCallback(value, callback);

        return callback;
    }

    #notifyMapEventSource(source: atlas.source.Source, event: MapEventDef) {
        let payload = { id: source.getId(), jsInterop: Helpers.getJsInterop(source) };
        let result = Helpers.buildEventResult(this.mapId, event, payload);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventSource, result);
        MapEventLogger.logNotifyFired(this.mapId, EventNotifications.NotifyMapEventSource, event.type);
    }

    // #endregion

    // #region Style
    #addMapEventStyle(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "addMapEventStyle";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventStyle, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            const callback = this.#getCallbackStyle(value, false);

            if (callback) {
                azmap.events.addOnce(value.type as any, callback);
                wasAdded = true;
            }

            MapEventLogger.logEventAdd(this.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventStyle(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "removeMapEventStyle";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventStyle, value.type)).forEach((value) => {
            let wasRemoved: boolean = false;
            const callback = this.#getCallbackStyle(value, true);

            if (callback) {
                azmap.events.remove(value.type, callback);
                wasRemoved = true;
            }

            MapEventLogger.logEventRemoved(this.mapId, eventName, wasRemoved, value);
        });
    }

    #getCallbackStyle(value: MapEventDef, removing: boolean) {
        let callback: any = this.getCallback(value, removing);

        if (callback) {
            return callback;
        }

        switch (value.type.toLowerCase()) {
            case MapEventStyle.StyleChanged:
                callback = (source: atlas.StyleChangedEvent) => this.#notifyMapEventStyle(source.style, value);
                break;
            case MapEventStyle.StyleImageMissing:
                callback = (style: string) => this.#notifyMapEventStyle(style, value);
                break;
            default:
        }

        this.addCallback(value, callback);

        return callback;
    }

    #notifyMapEventStyle(style: string, event: MapEventDef) {
        let payload = { style: style };
        let result = Helpers.buildEventResult(this.mapId, event, payload);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventStyle, result);
        MapEventLogger.logNotifyFired(this.mapId, EventNotifications.NotifyMapEventStyle, event.type);
    }
    // #endregion

    // #region Touch
    #addMapEventTouch(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "addMapEventTouch";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventTouch, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            const callback = this.#getCallbackTouch(value, false);

            if (callback) {
                if (value.once) {
                    azmap.events.addOnce(value.type as MapEventTouch, callback);
                }
                else {
                    azmap.events.add(value.type as MapEventTouch, callback);
                }
                wasAdded = true;
            }

            MapEventLogger.logEventAdd(this.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventTouch(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "removeMapEventTouch";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventTouch, value.type)).forEach((value) => {
            let wasRemoved: boolean = false;
            const callback = this.#getCallbackTouch(value, true);

            if (callback) {
                azmap.events.remove(value.type, callback);
                wasRemoved = true;
            }

            MapEventLogger.logEventRemoved(this.mapId, eventName, wasRemoved, value);
        });
    }

    #getCallbackTouch(value: MapEventDef, removing: boolean) {
        let callback: any = this.getCallback(value, removing);

        if (callback) {
            return callback;
        }

        callback = (callback: atlas.MapTouchEvent) => this.#notifyMapEventTouch(callback, value);

        this.addCallback(value, callback);

        return callback;
    }

    #notifyMapEventTouch = (callback: atlas.MapTouchEvent, event: MapEventDef) => {
        if (event.preventDefault)
            callback.preventDefault();
        let payload = Helpers.buildTouchEventPayload(callback);
        let result = Helpers.buildEventResult(this.mapId, event, payload);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventTouch, result);
        MapEventLogger.logNotifyFired(this.mapId, EventNotifications.NotifyMapEventTouch, event.type);
    };

    // #endregion

    // #region Wheel
    #addMapEventWheel(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "addMapEventWheel";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventWheel, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            let callback = this.#getCallbackWheel(value, false);

            if (callback) {
                if (value.once) {
                    azmap.events.addOnce(value.type as MapEventWheel, callback);
                }
                else {
                    azmap.events.add(value.type as MapEventWheel, callback);
                }
                wasAdded = true;
            }

            MapEventLogger.logEventAdd(this.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventWheel(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "removeMapEventWheel";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventWheel, value.type)).forEach((value) => {
            let wasRemoved: boolean = false;
            let callback = this.#getCallbackWheel(value, true);

            if (callback) {
                azmap.events.remove(value.type, callback);
                wasRemoved = true;
            }

            MapEventLogger.logEventRemoved(this.mapId, eventName, wasRemoved, value);
        });
    }

    #getCallbackWheel(value: MapEventDef, removing: boolean) {
        let callback: any = this.getCallback(value, removing);

        if (callback) {
            return callback;
        }

        callback = (callback: atlas.MapMouseWheelEvent) => this.#notifyMapEventWheel(callback, value);

        this.addCallback(value, callback);

        return callback;
    }

    #notifyMapEventWheel = (callback: atlas.MapMouseWheelEvent, event: MapEventDef) => {
        if (event.preventDefault)
            callback.preventDefault();
        let payload = Helpers.buildWheelEventPayload(callback);
        let result = Helpers.buildEventResult(this.mapId, event, payload);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventWheel, result);
        MapEventLogger.logNotifyFired(this.mapId, EventNotifications.NotifyMapEventWheel, event.type);
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

