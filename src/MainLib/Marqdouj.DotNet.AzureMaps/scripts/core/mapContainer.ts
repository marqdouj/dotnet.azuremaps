import * as atlas from "azure-maps-control"
import { MapFactory } from "./factory"
import { EventFactory } from "../modules/events/eventFactory"

export class MapContainer {
    events: EventFactory;

    constructor(public dotNetRef: any, public mapId: string, public azMap: atlas.Map) {
        this.events = new EventFactory(mapId);
    }

    clear() {
        this.dotNetRef = null;
        this.mapId = null;
        this.azMap = null;
    }
}

export class MapContainerHelper {
    mapId: string;

    constructor(mapId: string) {
        this.mapId = mapId;
    }

    getContainer(): MapContainer {
        return MapFactory.getMapContainer(this.mapId);
    }

    getMap(): atlas.Map {
        return this.getContainer().azMap;
    }

    getDotNetRef(): any {
        return this.getContainer().dotNetRef;
    }
}
