import * as atlas from "azure-maps-control"
import { IMapReference } from "./typings"
import { EventsMap } from "./events/EventsMap";

export class MapReference implements IMapReference {
    #dotNetRef: any;
    #eventsMap: EventsMap;
    #map: atlas.Map;
    #mapId: string;

    constructor(dotNetRef: any, mapId: string, azMap: atlas.Map) {
        this.#dotNetRef = dotNetRef;
        this.#mapId = mapId;
        this.#map = azMap;
        this.#eventsMap = new EventsMap(mapId);
    }

    get dotNetRef(): any { return this.#dotNetRef }
    get eventsMap(): EventsMap { return this.#eventsMap; }
    get mapId(): string { return this.#mapId }
    get map(): atlas.Map { return this.#map }

    clear() {
        this.#dotNetRef = null;
        this.#mapId = null;
        this.#map = null;
        this.#eventsMap.clear();
    }
}

