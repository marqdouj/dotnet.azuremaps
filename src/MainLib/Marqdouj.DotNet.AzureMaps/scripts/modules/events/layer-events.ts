import * as atlas from "azure-maps-control"
import { Helpers } from "../helpers"
import { EventNotifications, MapEventLogger } from "./common"
import { EventFactoryBase, MapEventMouse, MapEventTouch } from "./common"

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
            let callback = this.#getCallback(value);


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
            let callback = this.#getCallback(value);

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

    #getCallback(value: MapEventDef) {
        let callback: any;
        const isLayer = value.target === "layer";

        switch (value.type as MapLayerEvent) {
            case MapLayerEvent.LayerAdded:
                callback = isLayer ? this.#notifyMapEventLayer_LayerAdded : this.#notifyMapEvent_LayerAdded;
                break;
            case MapLayerEvent.LayerRemoved:
                callback = isLayer ? this.#notifyMapEventLayer_LayerRemoved : this.#notifyMapEvent_LayerRemoved;
                break;
            case MapLayerEvent.Click:
                callback = this.#notifyMapEventLayerMouse_Click;
                break;
            case MapLayerEvent.ContextMenu:
                callback = this.#notifyMapEventLayerMouse_ContextMenu;
                break;
            case MapLayerEvent.DblClick:
                callback = this.#notifyMapEventLayerMouse_DblClick;
                break;
            case MapLayerEvent.MouseDown:
                callback = this.#notifyMapEventLayerMouse_MouseDown;
                break;
            case MapLayerEvent.MouseEnter:
                callback = this.#notifyMapEventLayerMouse_MouseEnter;
                break;
            case MapLayerEvent.MouseLeave:
                callback = this.#notifyMapEventLayerMouse_MouseLeave;
                break;
            case MapLayerEvent.MouseMove:
                callback = this.#notifyMapEventLayerMouse_MouseMove;
                break;
            case MapLayerEvent.MouseOut:
                callback = this.#notifyMapEventLayerMouse_MouseOut;
                break;
            case MapLayerEvent.MouseOver:
                callback = this.#notifyMapEventLayerMouse_MouseOver;
                break;
            case MapLayerEvent.MouseUp:
                callback = this.#notifyMapEventLayerMouse_MouseUp;
                break;
            case MapLayerEvent.TouchCancel:
                callback = this.#notifyMapEventLayerTouch_TouchCancel;
                break;
            case MapLayerEvent.TouchEnd:
                callback = this.#notifyMapEventLayerTouch_TouchEnd;
                break;
            case MapLayerEvent.TouchMove:
                callback = this.#notifyMapEventLayerTouch_TouchMove;
                break;
            case MapLayerEvent.TouchStart:
                callback = this.#notifyMapEventLayerTouch_TouchStart;
                break;
            case MapLayerEvent.Wheel:
                callback = this.#notifyMapEventLayerWheel_Wheel;
                break;
            default:
        }

        return callback;
    }

    #notifyMapEventLayer = (layer: atlas.layer.Layer, event: MapEventLayer, hasTarget: boolean) => {
        let payload = { id: layer.getId(), jsInterop: Helpers.getJsInterop(layer) };
        let result = Helpers.buildEventResult(this.mapId, event, payload);
        let notify = hasTarget ? EventNotifications.NotifyMapEventLayer : EventNotifications.NotifyMapEvent;
        this.getDotNetRef().invokeMethodAsync(notify, result);
    };

    #notifyMapEventLayer_LayerAdded = (layer: atlas.layer.Layer) => this.#notifyMapEventLayer(layer, MapEventLayer.LayerAdded, true);
    #notifyMapEventLayer_LayerRemoved = (layer: atlas.layer.Layer) => this.#notifyMapEventLayer(layer, MapEventLayer.LayerRemoved, true);
    #notifyMapEvent_LayerAdded = (layer: atlas.layer.Layer) => this.#notifyMapEventLayer(layer, MapEventLayer.LayerAdded, false);
    #notifyMapEvent_LayerRemoved = (layer: atlas.layer.Layer) => this.#notifyMapEventLayer(layer, MapEventLayer.LayerRemoved, false);

    #NotifyMapEventLayerTouch = (callback: atlas.MapTouchEvent, type: MapEventTouch) => {
        let payload = { id: callback.layerId, touch: Helpers.buildTouchEventPayload(callback) };
        let result = Helpers.buildEventResult(this.mapId, type, payload);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventLayer, result);
    };

    #notifyMapEventLayerTouch_TouchCancel = (callback: atlas.MapTouchEvent) => this.#NotifyMapEventLayerTouch(callback, MapEventTouch.TouchCancel);
    #notifyMapEventLayerTouch_TouchEnd = (callback: atlas.MapTouchEvent) => this.#NotifyMapEventLayerTouch(callback, MapEventTouch.TouchEnd);
    #notifyMapEventLayerTouch_TouchMove = (callback: atlas.MapTouchEvent) => this.#NotifyMapEventLayerTouch(callback, MapEventTouch.TouchMove);
    #notifyMapEventLayerTouch_TouchStart = (callback: atlas.MapTouchEvent) => this.#NotifyMapEventLayerTouch(callback, MapEventTouch.TouchStart);

    #notifyMapEventLayerMouse = (callback: atlas.MapMouseEvent, event: MapEventMouse | MapLayerMouseEvent) => {
        let payload = { id: callback.layerId, mouse: Helpers.buildMouseEventPayload(callback) };
        let result = Helpers.buildEventResult(this.mapId, event, payload);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventLayer, result);
    };

    #notifyMapEventLayerMouse_Click = (callback: atlas.MapMouseEvent) => this.#notifyMapEventLayerMouse(callback, MapEventMouse.Click);
    #notifyMapEventLayerMouse_ContextMenu = (callback: atlas.MapMouseEvent) => this.#notifyMapEventLayerMouse(callback, MapEventMouse.ContextMenu);
    #notifyMapEventLayerMouse_DblClick = (callback: atlas.MapMouseEvent) => this.#notifyMapEventLayerMouse(callback, MapEventMouse.DblClick);
    #notifyMapEventLayerMouse_MouseDown = (callback: atlas.MapMouseEvent) => this.#notifyMapEventLayerMouse(callback, MapEventMouse.MouseDown);
    #notifyMapEventLayerMouse_MouseEnter = (callback: atlas.MapMouseEvent) => this.#notifyMapEventLayerMouse(callback, MapLayerMouseEvent.MouseEnter);
    #notifyMapEventLayerMouse_MouseLeave = (callback: atlas.MapMouseEvent) => this.#notifyMapEventLayerMouse(callback, MapLayerMouseEvent.MouseLeave);
    #notifyMapEventLayerMouse_MouseMove = (callback: atlas.MapMouseEvent) => this.#notifyMapEventLayerMouse(callback, MapEventMouse.MouseMove);
    #notifyMapEventLayerMouse_MouseOut = (callback: atlas.MapMouseEvent) => this.#notifyMapEventLayerMouse(callback, MapEventMouse.MouseOut);
    #notifyMapEventLayerMouse_MouseOver = (callback: atlas.MapMouseEvent) => this.#notifyMapEventLayerMouse(callback, MapEventMouse.MouseOver);
    #notifyMapEventLayerMouse_MouseUp = (callback: atlas.MapMouseEvent) => this.#notifyMapEventLayerMouse(callback, MapEventMouse.MouseUp);

    #notifyMapEventLayerWheel = (callback: atlas.MapMouseWheelEvent, type: string) => {
        let payload = { wheel: Helpers.buildWheelEventPayload(callback) };
        let result = Helpers.buildEventResult(this.mapId, type, payload);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventLayer, result);
    };

    #notifyMapEventLayerWheel_Wheel = (callback: atlas.MapMouseWheelEvent) => this.#notifyMapEventLayerWheel(callback, MapLayerEvent.Wheel);

    // #endregion
}

export enum MapEventLayer {
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

//Events not included in MapEventMouse
enum MapLayerMouseEvent {
    MouseEnter = 'mouseenter',
    MouseLeave = 'mouseleave',
}

