import { MapControl, DataSourceDef } from "../typings"
import { EventManager, MapEvent } from "../map-events"
import { Controls } from "./controls"
import { SourceManager } from "./source"

export class Maps {
    public static addEvents(dotNetRef: any, mapId: string, events: MapEvent[]): void {
        EventManager.addEvents(dotNetRef, mapId, events);
    }

    public static removeEvents(mapId: string, events: MapEvent[]): void {
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
        SourceManager.clearDatasource(mapId, id);
    }

    public static createDatasource(mapId: string, source: DataSourceDef): void {
        SourceManager.createDatasource(mapId, source);
    }

    public static getDataSourceShapes(mapId: string, id: string) {
        return SourceManager.getDataSourceShapes(mapId, id);
    }

    public static removeDatasource(mapId: string, id: string): void {
        SourceManager.removeDatasource(mapId, id);
    }
}