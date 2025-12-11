import * as atlas from "azure-maps-control";
import { MapFactory } from "../map-factory"
import { Logger } from "../common"
import { MapControl, DataSourceDef } from "../typings"
import { EventManager, MapEvent } from "../map-events"
import { Controls } from "./controls"

export class Maps {
    static addEvents(dotNetRef: any, mapId: string, events: MapEvent[]): void {
        EventManager.addEvents(dotNetRef, mapId, events);
    }

    static removeEvents(mapId: string, events: MapEvent[]): void {
        EventManager.removeEvents(mapId, events);
    }

    public static addControls(mapId: string, controls: MapControl[]): void {
        Controls.addControls(mapId, controls);
    }

    public static getControls(mapId: string): object[] {
        return Controls.getControls(mapId);
    }

    public static removeControls(mapId: string, controls: MapControl[]): void {
        Controls.removeControls(mapId, controls);
    }

    public static clearDatasource(mapId: string, id: string): void {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        const ds = map.sources.getById(id) as atlas.source.DataSource;
        if (ds) {
            ds.clear();
            Logger.logMessage(mapId, Logger.LogLevel.Debug, `- clearDatasource: datasource with ID '${id}' was cleared.`);
        }
    }

    public static createDatasource(mapId: string, source: DataSourceDef): void {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        const ds = map.sources.getById(source.id);
        if (ds) {
            Logger.logMessage(mapId, Logger.LogLevel.Warn, `- createDatasource: datasource with ID '${source.id}' already exists.`);
            return;
        }

        const newDs = new atlas.source.DataSource(source.id, source.options);
        map.sources.add(newDs);

        if (source.url) {
            newDs.importDataFromUrl(source.url);
        }
    }

    public static removeDatasource(mapId: string, id: string): void {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        const ds = map.sources.getById(id) as atlas.source.DataSource;
        if (ds) {
            map.sources.remove(ds);
            Logger.logMessage(mapId, Logger.LogLevel.Debug, `- removeDatasource: datasource with ID '${id}' was removed.`);
        }
    }
}