import * as atlas from "azure-maps-control"
import { Helpers } from "../helpers"
import { EventNotifications, MapEventLogger } from "./common"
import { MapEventMouse, MapEventTouch } from "./common"
import { EventFactoryBase } from "./EventFactoryBase";

export class LayerEventFactory extends EventFactoryBase {
    constructor(mapId: string) {
        super(mapId);
    }

    addEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#addLayerEvents(Object.values(events).filter(value => (value.target === "layer" || value.target === "map") && Helpers.isValueInEnum(MapLayerEvent, value.type)));
    }

    removeEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#removeLayerEvents(Object.values(events).filter(value => (value.target === "layer" || value.target === "map") && Helpers.isValueInEnum(MapLayerEvent, value.type)));
    }

    // #region Layer
    #addLayerEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "addLayerEvents";

        events.forEach((value) => {
            let wasAdded: boolean = false;
            const callback = this.#getCallback(value, false);

            if (callback) {
                const isLayer = value.target === "layer";

                if (isLayer) {
                    const target = this.#getTarget(azmap, value);
                    if (target) {
                        if (value.once) {
                            azmap.events.addOnce(value.type as MapEventLayer, target, callback);
                        }
                        else {
                            azmap.events.add(value.type as MapEventLayer, target, callback);
                        }
                        wasAdded = true;
                    }
                    else {
                        MapEventLogger.logInvalidTargetId(this.mapId, eventName, value);
                    }
                }
                else {
                    if (value.once) {
                        azmap.events.addOnce(value.type as any, callback);
                    }
                    else {
                        azmap.events.add(value.type as any, callback);
                    }
                    wasAdded = true;
                }
            }

            MapEventLogger.logEventAdd(this.mapId, eventName, wasAdded, value);
        });
    }

    #removeLayerEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "removeLayerEvents";

        events.forEach((value) => {
            let wasRemoved: boolean = false;
            const callback = this.#getCallback(value, true);

            if (callback) {
                const isLayer = value.target === "layer";

                if (isLayer) {
                    const target = this.#getTarget(azmap, value);
                    if (target) {
                        azmap.events.remove(value.type, target, callback);
                        wasRemoved = true;
                    } 
                    else {
                        MapEventLogger.logInvalidTargetId(this.mapId, eventName, value);
                    }
                }
                else {
                    azmap.events.remove(value.type, callback);
                    wasRemoved = true;
                }
            }

            MapEventLogger.logEventRemoved(this.mapId, eventName, wasRemoved, value);
        });
    }

    #getTarget(azmap: atlas.Map, event: MapEventDef) {
        if (event.target === "layer") {
            return azmap.layers.getLayerById(event.targetId);
        }
    }

    #getCallback(value: MapEventDef, removing: boolean) {
        const isLayer = value.target === "layer";
        let callback: any = this.getCallback(value, removing);
        
        if (callback) {
            return callback;
        }

        switch (value.type as MapLayerEvent) {
            case MapLayerEvent.LayerAdded:
            case MapLayerEvent.LayerRemoved:
                callback = (layer: atlas.layer.Layer) => this.#notifyMapEventLayer(layer, value, isLayer)
                break;
            case MapLayerEvent.Click:
            case MapLayerEvent.ContextMenu:
            case MapLayerEvent.DblClick:
            case MapLayerEvent.MouseDown:
            case MapLayerEvent.MouseEnter:
            case MapLayerEvent.MouseLeave:
            case MapLayerEvent.MouseMove:
            case MapLayerEvent.MouseOut:
            case MapLayerEvent.MouseOver:
            case MapLayerEvent.MouseUp:
                callback = (callback: atlas.MapMouseEvent) => this.#notifyMapEventLayerMouse(callback, value);
                break;
            case MapLayerEvent.TouchCancel:
            case MapLayerEvent.TouchEnd:
            case MapLayerEvent.TouchMove:
            case MapLayerEvent.TouchStart:
                callback = callback = (callback: atlas.MapTouchEvent) => this.#NotifyMapEventLayerTouch(callback, value);
                break;
            case MapLayerEvent.Wheel:
                callback = (callback: atlas.MapMouseWheelEvent) => this.#notifyMapEventLayerWheel(callback, value);
                break;
            default:
        }

        this.addCallback(value, callback);

        return callback;
    }

    #notifyMapEventLayer = (layer: atlas.layer.Layer, event: MapEventDef, hasTarget: boolean) => {
        let payload = { id: event.targetId, jsInterop: Helpers.getJsInterop(layer) };
        let result = Helpers.buildEventResult(this.mapId, event, payload);
        let notify = hasTarget ? EventNotifications.NotifyMapEventLayer : EventNotifications.NotifyMapEvent;
        this.getDotNetRef().invokeMethodAsync(notify, result);
    };

    #NotifyMapEventLayerTouch = (callback: atlas.MapTouchEvent, event: MapEventDef,) => {
        let payload = { id: event.targetId, touch: Helpers.buildTouchEventPayload(callback) };
        let result = Helpers.buildEventResult(this.mapId, event, payload);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventLayer, result);
    };

    #notifyMapEventLayerMouse = (callback: atlas.MapMouseEvent, event: MapEventDef) => {
        let payload = { id: event.targetId, mouse: Helpers.buildMouseEventPayload(callback) };
        let result = Helpers.buildEventResult(this.mapId, event, payload);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventLayer, result);
    };

    #notifyMapEventLayerWheel = (callback: atlas.MapMouseWheelEvent, event: MapEventDef) => {
        let payload = { id: event.targetId, wheel: Helpers.buildWheelEventPayload(callback) };
        let result = Helpers.buildEventResult(this.mapId, event, payload);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventLayer, result);
    };
    // #endregion
}

enum MapEventLayer {
    LayerAdded = 'layeradded',
    LayerRemoved = 'layerremoved'
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


