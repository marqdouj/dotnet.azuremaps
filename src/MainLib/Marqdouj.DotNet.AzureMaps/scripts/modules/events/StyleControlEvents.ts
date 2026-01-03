import atlas from "azure-maps-control";
import { Helpers } from "../common/Helpers";
import { IMapReference, MapEventDef } from "../typings";
import { EventNotification } from "../EventManager";
import { EventsHelper } from "./EventsHelper";
import { EventsLogger } from "./EventsLogger"
import { ControlManager } from "../ControlManager"

export class StyleControlEvents {
    add(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#addEvents(mapRef, this.#getEventDefs(events));
    }

    remove(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#removeEvents(mapRef, this.#getEventDefs(events));
    }

    #getEventDefs(events: MapEventDef[]) {
        return Object.values(events).filter(value => value.target === "stylecontrol" && Helpers.isValueInEnum(StyleControlEvent, value.type));
    }

    #addEvents(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "addStyleControlEvents";

        events.forEach((value) => {
            const target = ControlManager.getControl(mapRef.mapId, value.targetId);

            if (target instanceof atlas.control.StyleControl) {
                let wasAdded: boolean = false;
                const callback = this.#getCallback(mapRef, value, false);

                if (callback) {
                    if (value.once) {
                        mapRef.map.events.addOnce(value.type as StyleControlEvent, target, callback);
                    }
                    else {
                        mapRef.map.events.add(value.type as StyleControlEvent, target, callback);
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

    #removeEvents(mapRef: IMapReference, events: MapEventDef[]) {
        if (events.length == 0) return;

        const eventName = "removeStyleControlEvents";

        events.forEach((value) => {
            const target = ControlManager.getControl(mapRef.mapId, value.targetId);

            if (target instanceof atlas.control.StyleControl) {
                let wasRemoved: boolean = false;
                const callback = this.#getCallback(mapRef, value, true);

                if (callback) {
                    mapRef.map.events.remove(value.type, target, callback);
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

        callback = (style: string) => this.#notifyStyleControlEvent(style, mapRef, value);

        mapRef.eventsMap.addCallback(value, callback);
        return callback;
    }

    #notifyStyleControlEvent = (style: string, mapRef: IMapReference, event: MapEventDef) => {
        let result = EventsHelper.buildEventResult(mapRef.mapId, event, { style: style });
        mapRef.dotNetRef.invokeMethodAsync(EventNotification.NotifyMapEventStyle, result);
        EventsLogger.logNotifyFired(mapRef.mapId, EventNotification.NotifyMapEventStyle, event.type);
    };
}

enum StyleControlEvent {
    StyleSelected = 'styleselected',
}

