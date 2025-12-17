import * as atlas from "azure-maps-control"
import { Helpers } from "../helpers"
import { EventNotifications, MapEventLogger } from "./common"
import { EventFactoryBase } from "./EventFactoryBase"
import { SourceManager } from "../source"

export class SourceEventFactory extends EventFactoryBase {
    constructor(mapId: string) {
        super(mapId);
    }

    addEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#addDataSourceEvents(Object.values(events).filter(value => value.target === "datasource"));
    }

    removeEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#removeDataSourceEvents(Object.values(events).filter(value => value.target === "datasource"));
    }

    // #region DataSource
    #addDataSourceEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "addDataSourceEvents";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventDataSource, value.type)).forEach((value) => {
            const target = this.#getTarget(azmap, value);
            let wasAdded: boolean = false;

            if (target) {
                const callback = this.#getCallback(value, false);

                if (callback) {
                    if (value.once) {
                        azmap.events.addOnce(value.type as any, target, callback);
                    }
                    else {
                        azmap.events.add(value.type as any, target, callback);
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

    #removeDataSourceEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "removeDataSourceEvents";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventDataSource, value.type)).forEach((value) => {
            const target = this.#getTarget(azmap, value);
            let wasRemoved: boolean = false;

            if (target) {
                const callback = this.#getCallback(value, true);

                if (callback) {
                    azmap.events.remove(value.type as any, target, callback);
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

        switch (value.type.toLowerCase()) {
            case MapEventDataSource.DataSourceUpdated:
                callback = (callback: atlas.source.DataSource) => this.#NotifyMapDataSourceEvent_Updated(callback, value);
                break;
            default:
                callback = (callback: atlas.Shape[]) => this.#NotifyMapDataSourceEvent(callback, value);
        }

        this.addCallback(value, callback);
        return callback;
    }

    #getTarget(azmap: atlas.Map, event: MapEventDef) {
        const target = SourceManager.getDataSource(azmap, this.mapId, event.targetId);
        return target;
    }

    #NotifyMapDataSourceEvent_Updated = (callback: atlas.source.DataSource, event: MapEventDef) => {
        let payload = { sourceId: callback.getId() };
        let result = Helpers.buildEventResult(this.mapId, event, payload);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventDataSource, result);
        MapEventLogger.logNotifyFired(this.mapId, EventNotifications.NotifyMapEventDataSource, event.type);
    };

    #NotifyMapDataSourceEvent = (callback: atlas.Shape[], event: MapEventDef) => {
        let payload = { shapes: Helpers.buildShapeResults(callback) };
        let result = Helpers.buildEventResult(this.mapId, event, payload);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventDataSource, result);
        MapEventLogger.logNotifyFired(this.mapId, EventNotifications.NotifyMapEventDataSource, event.type);
    };
    // #endregion
}

enum MapEventDataSource {
    DataSourceUpdated = 'datasourceupdated',
    DataAdded = 'dataadded',
    DataRemoved = 'dataremoved',
}
