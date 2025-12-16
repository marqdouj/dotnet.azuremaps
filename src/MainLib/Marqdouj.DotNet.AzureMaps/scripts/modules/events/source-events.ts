import * as atlas from "azure-maps-control"
import { Helpers } from "../helpers"
import { EventNotifications, MapEventLogger } from "./common"
import { EventFactoryBase } from "./common"
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
                let callback = this.#getCallback(value);

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
                let callback = this.#getCallback(value);

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

    #getCallback(value: MapEventDef) {
        let callback: any;

        switch (value.type.toLowerCase()) {
            case MapEventDataSource.DataSourceUpdated:
                callback = this.#NotifyMapDataSourceEvent_Updated;
                break;
            case MapEventDataSource.DataAdded:
                callback = this.#notifyMapDataSourceEvent_DataAdded;
                break;
            case MapEventDataSource.DataRemoved:
                callback = this.#notifyMapDataSourceEvent_DataRemoved;
                break;
            default:
        }

        return callback;
    }

    #getTarget(azmap: atlas.Map, event: MapEventDef) {
        const target = SourceManager.getDataSource(azmap, this.mapId, event.targetId);
        return target;
    }

    #NotifyMapDataSourceEvent_Updated = (callback: atlas.source.DataSource, type: MapEventDataSource) => {
        let payload = { sourceId: callback.getId() };
        let result = Helpers.buildEventResult(this.mapId, type, payload);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventDataSource, result);
    };

    #notifyMapDataSourceEvent_DataSourceUpdated = (callback: atlas.source.DataSource) => this.#NotifyMapDataSourceEvent_Updated(callback, MapEventDataSource.DataSourceUpdated);

    #NotifyMapDataSourceEvent = (callback: atlas.Shape[], type: MapEventDataSource) => {
        let payload = { shapes: Helpers.buildShapeResults(callback) };
        let result = Helpers.buildEventResult(this.mapId, type, payload);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventDataSource, result);
    };

    #notifyMapDataSourceEvent_DataAdded = (callback: atlas.Shape[]) => this.#NotifyMapDataSourceEvent(callback, MapEventDataSource.DataAdded);
    #notifyMapDataSourceEvent_DataRemoved = (callback: atlas.Shape[]) => this.#NotifyMapDataSourceEvent(callback, MapEventDataSource.DataRemoved);
    // #endregion
}

enum MapEventDataSource {
    DataSourceUpdated = 'datasourceupdated',
    DataAdded = 'dataadded',
    DataRemoved = 'dataremoved',
}
