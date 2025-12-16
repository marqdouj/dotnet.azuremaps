import * as atlas from "azure-maps-control"
import { Helpers } from "../helpers"
import { EventNotifications, MapEventLogger } from "./common"
import { EventFactoryBase } from "./common"
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
                let callback: any;

                switch (value.type.toLowerCase()) {
                    case StyleControlEvent.StyleSelected:
                        callback = this.#notifyStyleControlEvent_StyleSelected
                        break;
                    default:
                }

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
                let callback: any;

                switch (value.type.toLowerCase()) {
                    case StyleControlEvent.StyleSelected:
                        callback = this.#notifyStyleControlEvent_StyleSelected;
                        break;
                    default:
                }

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

    #notifyStyleControlEvent = (style: string, event: StyleControlEvent) => {
        let result = Helpers.buildEventResult(this.mapId, event, { style: style });
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventStyle, result);
    };

    #notifyStyleControlEvent_StyleSelected = (style: string) => this.#notifyStyleControlEvent(style, StyleControlEvent.StyleSelected);
    // #endregion
}

enum StyleControlEvent {
    StyleSelected = 'styleselected',
}