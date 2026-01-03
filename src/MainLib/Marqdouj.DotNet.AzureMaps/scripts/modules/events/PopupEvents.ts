import atlas from "azure-maps-control";
import { Helpers } from "../common/Helpers";
import { IMapReference, MapEventDef } from "../typings";
import { PopupManager } from "../PopupManager";
import { EventNotification } from "../EventManager";
import { EventsLogger } from "./EventsLogger";
import { EventsHelper } from "./EventsHelper";

export class PopupEvents {
    add(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#addPopupEvents(mapRef, this.#getEventDefs(events));
    }

    remove(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#removePopupEvents(mapRef, this.#getEventDefs(events));
    }

    #getEventDefs(events: MapEventDef[]) {
        return Object.values(events).filter(value => value.target === "popup" && Helpers.isValueInEnum(MapPopupEvent, value.type));
    }

    #addPopupEvents(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "addPopupEvents";

        events.forEach((value) => {
            const target = this.#getTarget(mapRef, value);
            let wasAdded: boolean = false;

            if (target) {
                const callback = this.#getCallback(mapRef, value, false);

                if (callback) {
                    if (value.once) {
                        mapRef.map.events.addOnce(value.type as MapPopupEvent, target, callback);
                    }
                    else {
                        mapRef.map.events.add(value.type as MapPopupEvent, target, callback);
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

    #removePopupEvents(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "removePopupEvents";

        events.forEach((value) => {
            const target = this.#getTarget(mapRef, value);
            let wasRemoved: boolean = false;

            if (target) {
                const callback = this.#getCallback(mapRef, value, true);

                if (callback) {
                    mapRef.map.events.remove(value.type as MapPopupEvent, target, callback);
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

        callback = (callback: atlas.TargetedEvent) => this.#NotifyMapPopupEvent(callback, mapRef, value);

        mapRef.eventsMap.addCallback(value, callback);
        return callback;
    }

    #getTarget(mapRef: IMapReference, event: MapEventDef) {
        return PopupManager.getPopup(mapRef, event.targetId);
    }

    #NotifyMapPopupEvent = (callback: atlas.TargetedEvent, mapRef: IMapReference, event: MapEventDef) => {
        let payload = EventsHelper.buildTargetedEventPayload(callback);
        let result = EventsHelper.buildEventResult(mapRef.mapId, event, payload);
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEventPopup, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEventPopup, event.type);
    };
}

enum MapPopupEvent {
    Drag = 'drag',
    DragEnd = 'dragend',
    DragStart = 'dragstart',
    Open = 'open',
    Close = 'close',
}