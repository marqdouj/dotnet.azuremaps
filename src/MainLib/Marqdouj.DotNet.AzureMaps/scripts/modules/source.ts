import * as atlas from "azure-maps-control"
import { Logger, LogLevel } from "./logger"
import { Helpers } from "./helpers"
import { MapFactory } from "../core/factory"
import { DataSourceDef, JsInteropDef } from "./typings"

export class SourceManager {
    static getDataSourceShapes(mapId: string, id: string) {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        let shapes: object[] = [];

        const ds = this.getDataSource(map, mapId, id);
        if (ds) {
            shapes = Helpers.buildShapeResults(ds.getShapes());
        }

        return shapes;
    }

    static clearDatasource(mapId: string, id: string): void {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        const ds = this.getDataSource(map, mapId, id);
        if (ds) {
            ds.clear();
            Logger.logMessage(mapId, LogLevel.Debug, `- clearDatasource: datasource with ID '${id}' was cleared.`);
        }
    }

    static createDatasource(mapId: string, source: DataSourceDef, events?: MapEventDef[]): void {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        const ds = map.sources.getById(source.id);
        if (ds) {
            Logger.logMessage(mapId, LogLevel.Warn, `- createDatasource: datasource with ID '${source.id}' already exists.`);
            return;
        }

        const newDs = new atlas.source.DataSource(source.id, source.options);
        const jsInterop: JsInteropDef = { id: source.id, interopId: source.interopId };
        (newDs as any).jsInterop = jsInterop;

        if (events) {
            const mapContainer = MapFactory.getMapContainer(mapId);
            mapContainer.events.addDataSourceEvents(events, newDs);
        }

        map.sources.add(newDs);

        if (source.url) {
            newDs.importDataFromUrl(source.url);
        }
    }

    static removeDatasource(mapId: string, id: string): void {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        const ds = map.sources.getById(id) as atlas.source.DataSource;
        if (ds) {
            map.sources.remove(ds);
            Logger.logMessage(mapId, LogLevel.Debug, `- removeDatasource: datasource with ID '${id}' was removed.`);
        }
    }

    static getDataSource(map: atlas.Map, mapId: string, id: string): atlas.source.DataSource {
        const source = map.sources.getById(id);

        if (source instanceof atlas.source.DataSource) {
            return source as atlas.source.DataSource;
        } else {
            Logger.logMessage(mapId, LogLevel.Debug, `getDataSource: source with ID '${id}' is not a DataSource.`);
        }
    }
}
