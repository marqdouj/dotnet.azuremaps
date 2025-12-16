import * as atlas from "azure-maps-control"
import { Helpers } from "../helpers"
import { EventNotifications, MapEventLogger } from "./common"
import { EventFactoryBase } from "./common"

export class ShapeEventFactory extends EventFactoryBase {
    constructor(mapId: string) {
        super(mapId);
    }

    addEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#addShapeEvents(Object.values(events).filter(value => value.target === "shape"));
    }

    removeEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#removeShapeEvents(Object.values(events).filter(value => value.target === "shape"));
    }

    // #region Shape
    #addShapeEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "addShapeEvents";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapShapeEvent, value.type)).forEach((value) => {
            const target = this.#getTarget(azmap, value);
            let wasAdded: boolean = false;

            if (target) {
                let callback = this.#getCallback(value);

                if (callback) {
                    if (value.once) {
                        azmap.events.addOnce(value.type as MapShapeEvent, target, callback);
                    }
                    else {
                        azmap.events.add(value.type as MapShapeEvent, target, callback);
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

    #removeShapeEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "removeShapeEvents";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapShapeEvent, value.type)).forEach((value) => {
            const target = this.#getTarget(azmap, value);
            let wasRemoved: boolean = false;

            if (target) {
                let callback = this.#getCallback(value);

                if (callback) {
                    azmap.events.remove(value.type as MapShapeEvent, target, callback);
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
            case MapShapeEvent.ShapeChanged:
                callback = this.#notifyMapShapeEvent_ShapeChanged;
                break;
            default:
        }

        return callback;
    }

    #getTarget(azmap: atlas.Map, event: MapEventDef) {
        const ds = azmap.sources.getById(event.targetSourceId) as atlas.source.DataSource;
        const target = ds.getShapeById(event.targetId);
        return target;
    }

    #NotifyMapShapeEvent = (callback: atlas.Shape, type: MapShapeEvent) => {
        let payload = Helpers.getShapeResult(callback);
        let result = Helpers.buildEventResult(this.mapId, type, payload);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventShape, result);
    };

    #notifyMapShapeEvent_ShapeChanged = (callback: atlas.Shape) => this.#NotifyMapShapeEvent(callback, MapShapeEvent.ShapeChanged);
}

enum MapShapeEvent {
    ShapeChanged = 'shapechanged',
}
