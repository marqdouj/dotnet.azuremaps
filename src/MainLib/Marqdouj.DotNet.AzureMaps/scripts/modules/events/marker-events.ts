import * as atlas from "azure-maps-control"
import { Helpers } from "../helpers"
import { EventNotifications, MapEventLogger } from "./common"
import { EventFactoryBase } from "./EventFactoryBase";
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
                const callback = this.#getCallback(value, false);

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
                const callback = this.#getCallback(value, true);

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

    #getCallback(value: MapEventDef, removing: boolean) {
        let callback: any = this.getCallback(value, removing);

        if (callback) {
            return callback;
        }

        callback = (callback: atlas.TargetedEvent) => this.#NotifyMapHtmlMarkerEvent(callback, value);

        this.addCallback(value, callback);
        return callback;
    }

    #getTarget(event: MapEventDef) {
        return MarkerManager.getMarker(this.mapId, event.targetId);
    }

    #NotifyMapHtmlMarkerEvent = (callback: atlas.TargetedEvent, event: MapEventDef) => {
        let payload = Helpers.buildTargetedEventPayload(callback);
        let result = Helpers.buildEventResult(this.mapId, event, payload);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventHtmlMarker, result);
    };
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
