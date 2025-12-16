import * as atlas from "azure-maps-control"
import { Helpers } from "../helpers"
import { EventNotifications, MapEventLogger } from "./common"
import { EventFactoryBase } from "./EventFactoryBase"
import { PopupManager } from "../popups"

export class PopupEventFactory extends EventFactoryBase {
    constructor(mapId: string) {
        super(mapId);
    }

    addEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#addPopupEvents(Object.values(events).filter(value => value.target === "popup"));
    }

    removeEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#removePopupEvents(Object.values(events).filter(value => value.target === "popup"));
    }

    // #region Popup
    #addPopupEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "addPopupEvents";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapPopupEvent, value.type)).forEach((value) => {
            const target = this.#getTarget(value);
            let wasAdded: boolean = false;

            if (target) {
                const callback = this.#getCallback(value, false);

                if (callback) {
                    if (value.once) {
                        azmap.events.addOnce(value.type as MapPopupEvent, target, callback);
                    }
                    else {
                        azmap.events.add(value.type as MapPopupEvent, target, callback);
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

    #removePopupEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "removePopupEvents";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapPopupEvent, value.type)).forEach((value) => {
            const target = this.#getTarget(value);
            let wasRemoved: boolean = false;

            if (target) {
                const callback = this.#getCallback(value, true);

                if (callback) {
                    azmap.events.remove(value.type as MapPopupEvent, target, callback);
                    wasRemoved = true;
                }

                MapEventLogger.logEventRemoved(this.mapId, eventName, wasRemoved, value);
            }
            else {
                MapEventLogger.logInvalidTargetId(this.mapId, eventName, value);
            }
        });
    }
    // #endregion

    #getCallback(value: MapEventDef, removing: boolean) {
        let callback: any = this.getCallback(value, removing);

        if (callback) {
            return callback;
        }

        callback = (callback: atlas.TargetedEvent) => this.#NotifyMapPopupEvent(callback, value);

        this.addCallback(value, callback);
        return callback;
    }

    #getTarget(event: MapEventDef) {
        return PopupManager.getPopup(this.mapId, event.targetId);
    }

    #NotifyMapPopupEvent = (callback: atlas.TargetedEvent, event: MapEventDef) => {
        let payload = Helpers.buildTargetedEventPayload(callback);
        let result = Helpers.buildEventResult(this.mapId, event, payload);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventPopup, result);
    };

}

enum MapPopupEvent {
    Drag = 'drag',
    DragEnd = 'dragend',
    DragStart = 'dragstart',
    Open = 'open',
    Close = 'close',
}
