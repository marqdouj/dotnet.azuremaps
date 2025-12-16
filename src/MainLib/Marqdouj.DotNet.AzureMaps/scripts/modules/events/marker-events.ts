import * as atlas from "azure-maps-control"
import { Helpers } from "../helpers"
import { EventNotifications, MapEventLogger } from "./common"
import { EventFactoryBase } from "./common"
import { MarkerManager } from "../markers";

export class MarkerEventFactory extends EventFactoryBase {
    constructor(mapId: string) {
        super(mapId);
    }

    addEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#addHtmlMarkerEvents(Object.values(events).filter(value => value.target === "htmlmarker"));
    }

    removeEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#removeHtmlMarkerEvents(Object.values(events).filter(value => value.target === "htmlmarker"));
    }

    // #region HtmlMarker
    #addHtmlMarkerEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "addHtmlMarkerEvents";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapHtmlMarkerEvent, value.type)).forEach((value) => {
            const target = this.#getTarget(value);
            let wasAdded: boolean = false;

            if (target) {
                let callback = this.#getCallback(value);

                if (callback) {
                    if (value.once) {
                        azmap.events.addOnce(value.type as MapHtmlMarkerEvent, target, callback);
                    }
                    else {
                        azmap.events.add(value.type as MapHtmlMarkerEvent, target, callback);
                    }
                    wasAdded = true;
                }

                MapEventLogger.logEventAdd(this.mapId, eventName, wasAdded, value);
            }
            else {
                MapEventLogger.logInvalidTargetId(this.mapId, eventName, value);
            }
        });
    }

    #removeHtmlMarkerEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "removeHtmlMarkerEvents";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapHtmlMarkerEvent, value.type)).forEach((value) => {
            const target = this.#getTarget(value);
            let wasRemoved: boolean = false;

            if (target) {
                let callback = this.#getCallback(value);

                if (callback) {
                    azmap.events.remove(value.type as MapHtmlMarkerEvent, target, callback);
                    wasRemoved = true;
                }

                MapEventLogger.logEventRemoved(this.mapId, eventName, wasRemoved, value);
            }
            else {
                MapEventLogger.logInvalidTargetId(this.mapId, eventName, value);
            }
        });
    }

    #getCallback(value: MapEventDef) {
        let callback: any;

        switch (value.type.toLowerCase()) {
            case MapHtmlMarkerEvent.Click:
                callback = this.#notifyMapHtmlMarkerEvent_Click;
                break;
            case MapHtmlMarkerEvent.ContextMenu:
                callback = this.#notifyMapHtmlMarkerEvent_ContextMenu;
                break;
            case MapHtmlMarkerEvent.DblClick:
                callback = this.#notifyMapHtmlMarkerEvent_DblClick;
                break;
            case MapHtmlMarkerEvent.Drag:
                callback = this.#notifyMapHtmlMarkerEvent_Drag;
                break;
            case MapHtmlMarkerEvent.DragEnd:
                callback = this.#notifyMapHtmlMarkerEvent_DragEnd;
                break;
            case MapHtmlMarkerEvent.DragStart:
                callback = this.#notifyMapHtmlMarkerEvent_DragStart;
                break;
            case MapHtmlMarkerEvent.KeyDown:
                callback = this.#notifyMapHtmlMarkerEvent_KeyDown;
                break;
            case MapHtmlMarkerEvent.KeyPress:
                callback = this.#notifyMapHtmlMarkerEvent_KeyPress;
                break;
            case MapHtmlMarkerEvent.KeyUp:
                callback = this.#notifyMapHtmlMarkerEvent_KeyUp;
                break;
            case MapHtmlMarkerEvent.MouseDown:
                callback = this.#notifyMapHtmlMarkerEvent_MouseDown;
                break;
            case MapHtmlMarkerEvent.MouseEnter:
                callback = this.#notifyMapHtmlMarkerEvent_MouseEnter;
                break;
            case MapHtmlMarkerEvent.MouseLeave:
                callback = this.#notifyMapHtmlMarkerEvent_MouseLeave;
                break;
            case MapHtmlMarkerEvent.MouseMove:
                callback = this.#notifyMapHtmlMarkerEvent_MouseMove;
                break;
            case MapHtmlMarkerEvent.MouseOut:
                callback = this.#notifyMapHtmlMarkerEvent_MouseOut;
                break;
            case MapHtmlMarkerEvent.MouseOver:
                callback = this.#notifyMapHtmlMarkerEvent_MouseOver;
                break;
            case MapHtmlMarkerEvent.MouseUp:
                callback = this.#notifyMapHtmlMarkerEvent_MouseUp;
                break;
            default:
        }

        return callback;
    }

    #getTarget(event: MapEventDef) {
        return MarkerManager.getMarker(this.mapId, event.targetId);
    }

    #NotifyMapHtmlMarkerEvent = (callback: atlas.TargetedEvent, type: MapHtmlMarkerEvent) => {
        let payload = Helpers.buildTargetedEventPayload(callback);
        let result = Helpers.buildEventResult(this.mapId, type, payload);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventHtmlMarker, result);
    };

    #notifyMapHtmlMarkerEvent_Click = (callback: atlas.TargetedEvent) => this.#NotifyMapHtmlMarkerEvent(callback, MapHtmlMarkerEvent.Click);
    #notifyMapHtmlMarkerEvent_ContextMenu = (callback: atlas.TargetedEvent) => this.#NotifyMapHtmlMarkerEvent(callback, MapHtmlMarkerEvent.ContextMenu);
    #notifyMapHtmlMarkerEvent_DblClick = (callback: atlas.TargetedEvent) => this.#NotifyMapHtmlMarkerEvent(callback, MapHtmlMarkerEvent.DblClick);
    #notifyMapHtmlMarkerEvent_MouseDown = (callback: atlas.TargetedEvent) => this.#NotifyMapHtmlMarkerEvent(callback, MapHtmlMarkerEvent.MouseDown);
    #notifyMapHtmlMarkerEvent_MouseEnter = (callback: atlas.TargetedEvent) => this.#NotifyMapHtmlMarkerEvent(callback, MapHtmlMarkerEvent.MouseEnter);
    #notifyMapHtmlMarkerEvent_MouseLeave = (callback: atlas.TargetedEvent) => this.#NotifyMapHtmlMarkerEvent(callback, MapHtmlMarkerEvent.MouseLeave);
    #notifyMapHtmlMarkerEvent_MouseMove = (callback: atlas.TargetedEvent) => this.#NotifyMapHtmlMarkerEvent(callback, MapHtmlMarkerEvent.MouseMove);
    #notifyMapHtmlMarkerEvent_MouseOut = (callback: atlas.TargetedEvent) => this.#NotifyMapHtmlMarkerEvent(callback, MapHtmlMarkerEvent.MouseOut);
    #notifyMapHtmlMarkerEvent_MouseOver = (callback: atlas.TargetedEvent) => this.#NotifyMapHtmlMarkerEvent(callback, MapHtmlMarkerEvent.MouseOver);
    #notifyMapHtmlMarkerEvent_MouseUp = (callback: atlas.TargetedEvent) => this.#NotifyMapHtmlMarkerEvent(callback, MapHtmlMarkerEvent.MouseUp);


    #notifyMapHtmlMarkerEvent_Drag = (callback: atlas.TargetedEvent) => this.#NotifyMapHtmlMarkerEvent(callback, MapHtmlMarkerEvent.Drag);
    #notifyMapHtmlMarkerEvent_DragEnd = (callback: atlas.TargetedEvent) => this.#NotifyMapHtmlMarkerEvent(callback, MapHtmlMarkerEvent.DragEnd);
    #notifyMapHtmlMarkerEvent_DragStart = (callback: atlas.TargetedEvent) => this.#NotifyMapHtmlMarkerEvent(callback, MapHtmlMarkerEvent.DragStart);

    #notifyMapHtmlMarkerEvent_KeyDown = (callback: atlas.TargetedEvent) => this.#NotifyMapHtmlMarkerEvent(callback, MapHtmlMarkerEvent.KeyDown);
    #notifyMapHtmlMarkerEvent_KeyPress = (callback: atlas.TargetedEvent) => this.#NotifyMapHtmlMarkerEvent(callback, MapHtmlMarkerEvent.KeyPress);
    #notifyMapHtmlMarkerEvent_KeyUp = (callback: atlas.TargetedEvent) => this.#NotifyMapHtmlMarkerEvent(callback, MapHtmlMarkerEvent.KeyUp);

    // #endregion
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
