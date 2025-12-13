import { MapControl, DataSourceDef, HtmlMarkerDef, PopupDef } from "../typings"
import { EventManager, MapEvent } from "../map-events"
import { Controls } from "./controls"
import { SourceManager } from "./source"
import { Markers } from "./markers";
import { Popups } from "./popups";

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

    public static addMarkers(mapId: string, markers: HtmlMarkerDef[]) {
        Markers.add(mapId, markers);
    }

    public static removeMarkers(mapId: string, markers: HtmlMarkerDef[]) {
        Markers.remove(mapId, markers);
    }

    public static addPopups(mapId: string, popups: PopupDef[]) {
        Popups.add(mapId, popups);
    }

    public static removePopups(mapId: string, popups: PopupDef[]) {
        Popups.remove(mapId, popups);
    }
}