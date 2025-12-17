import * as atlas from "azure-maps-control"
import { Helpers } from "../helpers"
import { EventNotifications, MapEventLogger } from "./common"
import { EventFactoryBase } from "./EventFactoryBase"
import { ControlManager } from "../../modules/controls"

export class StyleControlEventFactory extends EventFactoryBase {
    constructor(mapId: string) {
        super(mapId);
    }

    addEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#addStyleControlEvents(Object.values(events).filter(value => value.target === "stylecontrol"));
    }

    removeEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#removeStyleControlEvents(Object.values(events).filter(value => value.target === "stylecontrol"));
    }

    // #region StyleControl
    #addStyleControlEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "addStyleControlEvents";

        Object.values(events).filter(value => Helpers.isValueInEnum(StyleControlEvent, value.type)).forEach((value) => {
            const target = ControlManager.getControl(this.mapId, value.targetId) as atlas.control.StyleControl;

            if (target) {
                let wasAdded: boolean = false;
                const callback = this.#getCallback(value, false);

                if (callback) {
                    if (value.once) {
                        azmap.events.addOnce(value.type as StyleControlEvent, target, callback);
                    }
                    else {
                        azmap.events.add(value.type as StyleControlEvent, target, callback);
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

    #removeStyleControlEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "removeStyleControlEvents";

        Object.values(events).filter(value => Helpers.isValueInEnum(StyleControlEvent, value.type)).forEach((value) => {
            const target = ControlManager.getControl(this.mapId, value.targetId) as atlas.control.StyleControl;

            if (target) {
                let wasRemoved: boolean = false;
                const callback = this.#getCallback(value, true);

                if (callback) {
                    azmap.events.remove(value.type, target, callback);
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

        callback = (style: string) => this.#notifyStyleControlEvent(style, value);

        this.addCallback(value, callback);
        return callback;
    }

    #notifyStyleControlEvent = (style: string, event: MapEventDef) => {
        let result = Helpers.buildEventResult(this.mapId, event, { style: style });
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventStyle, result);
        MapEventLogger.logNotifyFired(this.mapId, EventNotifications.NotifyMapEventStyle, event.type);
    };

    // #endregion
}

enum StyleControlEvent {
    StyleSelected = 'styleselected',
}