import * as atlas from "azure-maps-control"
import { Helpers } from "../helpers"
import { EventNotifications, MapEventLogger } from "./common"
import { EventFactoryBase } from "./common"
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
                let callback = this.#getCallback(value);

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
                let callback = this.#getCallback(value);

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

    #getCallback(value: MapEventDef) {
        let callback: any;

        switch (value.type.toLowerCase()) {
            case MapPopupEvent.Close:
                callback = this.#notifyMapPopupEvent_Close;
                break;
            case MapPopupEvent.Drag:
                callback = this.#notifyMapPopupEvent_Drag;
                break;
            case MapPopupEvent.DragEnd:
                callback = this.#notifyMapPopupEvent_DragEnd;
                break;
            case MapPopupEvent.DragStart:
                callback = this.#notifyMapPopupEvent_DragStart;
                break;
            case MapPopupEvent.Open:
                callback = this.#notifyMapPopupEvent_Open;
                break;
            default:
        }

        return callback;
    }

    #getTarget(event: MapEventDef) {
        return PopupManager.getPopup(this.mapId, event.targetId);
    }

    #NotifyMapPopupEvent = (callback: atlas.TargetedEvent, type: MapPopupEvent) => {
        let payload = Helpers.buildTargetedEventPayload(callback);
        let result = Helpers.buildEventResult(this.mapId, type, payload);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventPopup, result);
    };

    #notifyMapPopupEvent_Close = (callback: atlas.TargetedEvent) => this.#NotifyMapPopupEvent(callback, MapPopupEvent.Close);
    #notifyMapPopupEvent_Drag = (callback: atlas.TargetedEvent) => this.#NotifyMapPopupEvent(callback, MapPopupEvent.Drag);
    #notifyMapPopupEvent_DragEnd = (callback: atlas.TargetedEvent) => this.#NotifyMapPopupEvent(callback, MapPopupEvent.DragEnd);
    #notifyMapPopupEvent_DragStart = (callback: atlas.TargetedEvent) => this.#NotifyMapPopupEvent(callback, MapPopupEvent.DragStart);
    #notifyMapPopupEvent_Open = (callback: atlas.TargetedEvent) => this.#NotifyMapPopupEvent(callback, MapPopupEvent.Open);
}

enum MapPopupEvent {
    Drag = 'drag',
    DragEnd = 'dragend',
    DragStart = 'dragstart',
    Open = 'open',
    Close = 'close',
}
