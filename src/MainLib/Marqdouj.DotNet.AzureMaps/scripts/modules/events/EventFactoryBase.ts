import * as atlas from "azure-maps-control";
import { MapContainerHelper } from "../../core/mapContainer";
import { Logger, LogLevel } from "../../modules/logger"

export class EventFactoryBase {
    mapId: string;
    #helper: MapContainerHelper;
    #eventMap: Map<string, object> = new Map<string, object>();

    constructor(mapId: string) {
        this.mapId = mapId;
        this.#helper = new MapContainerHelper(mapId);
    }

    getMap(): atlas.Map {
        return this.#helper.getMap();
    }

    getDotNetRef(): any {
        return this.#helper.getDotNetRef();
    }

    addCallback(value: MapEventDef, callback: any) {
        const eventId = this.#getCallbackId(value);
        if (!this.#eventMap.has(eventId)) {
            this.#eventMap.set(eventId, callback);
            Logger.logMessage(this.mapId, LogLevel.Trace, "EventFactoryBase:adding event:", eventId);
        }
    }

    getCallback(value: MapEventDef, removing: boolean) {
        const eventId = this.#getCallbackId(value);
        if (this.#eventMap.has(eventId)) {
            const callback = this.#eventMap.get(eventId);
            if (removing) {
                const wasRemoved = this.#eventMap.delete(eventId);
                Logger.logMessage(this.mapId, LogLevel.Trace, "EventFactoryBase:removing event:", eventId, wasRemoved);
            }
            return callback;
        }
    }

    #getCallbackId(value: MapEventDef) {
        return `${value.target}.${value.type}.${value.targetId}`;
    }
}
