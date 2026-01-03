import atlas from "azure-maps-control";
import { Helpers } from "../common/Helpers";
import { IMapReference, MapEventDef } from "../typings";
import { EventsHelper } from "./EventsHelper";
import { EventNotification } from "../EventManager";
import { MarkerManager } from "../MarkerManager";
import { EventsLogger } from "./EventsLogger";

export class MarkerEvents {
    add(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#addHtmlMarkerEvents(mapRef, this.#getEventDefs(events));
    }

    remove(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#removeHtmlMarkerEvents(mapRef, this.#getEventDefs(events));
    }

    #getEventDefs(events: MapEventDef[]) {
        return Object.values(events).filter(value => value.target === "htmlmarker" && Helpers.isValueInEnum(MapHtmlMarkerEvent, value.type));
    }

    #addHtmlMarkerEvents(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "addHtmlMarkerEvents";

        events.forEach((value) => {
            const target = this.#getTarget(mapRef, value);
            let wasAdded: boolean = false;

            if (target) {
                const callback = this.#getCallback(mapRef, value, false);

                if (callback) {
                    if (value.once) {
                        mapRef.map.events.addOnce(value.type as MapHtmlMarkerEvent, target, callback);
                    }
                    else {
                        mapRef.map.events.add(value.type as MapHtmlMarkerEvent, target, callback);
                    }
                    wasAdded = true;
                }

                EventsLogger.logEventAdd(mapRef.mapId, eventName, wasAdded, value);
            }
            else {
                EventsLogger.logInvalidTargetId(mapRef.mapId, eventName, value);
            }
        });
    }

    #removeHtmlMarkerEvents(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "removeHtmlMarkerEvents";

        events.forEach((value) => {
            const target = this.#getTarget(mapRef, value);
            let wasRemoved: boolean = false;

            if (target) {
                const callback = this.#getCallback(mapRef, value, true);

                if (callback) {
                    mapRef.map.events.remove(value.type as MapHtmlMarkerEvent, target, callback);
                    wasRemoved = true;
                }

                EventsLogger.logEventRemoved(mapRef.mapId, eventName, wasRemoved, value);
            }
            else {
                EventsLogger.logInvalidTargetId(mapRef.mapId, eventName, value);
            }
        });
    }

    #getCallback(mapRef: IMapReference, value: MapEventDef, removing: boolean) {
        let callback: any = mapRef.eventsMap.getCallback(value, removing);

        if (callback) {
            return callback;
        }

        callback = (callback: atlas.TargetedEvent) => this.#NotifyMapHtmlMarkerEvent(callback, mapRef, value);

        mapRef.eventsMap.addCallback(value, callback);
        return callback;
    }

    #getTarget(mapRef: IMapReference, event: MapEventDef) {
        return MarkerManager.getMarker(mapRef.mapId, event.targetId);
    }

    #NotifyMapHtmlMarkerEvent = (callback: atlas.TargetedEvent, mapRef: IMapReference, event: MapEventDef) => {
        let payload = EventsHelper.buildTargetedEventPayload(callback);
        let result = EventsHelper.buildEventResult(mapRef.mapId, event, payload);
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEventHtmlMarker, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEventHtmlMarker, event.type);
    };
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