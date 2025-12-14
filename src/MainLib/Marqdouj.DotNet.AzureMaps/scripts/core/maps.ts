import { ControlManager } from "../modules/controls"
import { SourceManager } from "../modules/source"
import { MarkerManager } from "../modules/markers"
import { PopupManager } from "../modules/popups"
import { EventManager, MapEvent } from "../modules/events"
import { DataSourceDef, MapControlDef, HtmlMarkerDef, PopupDef } from "../modules/typings"

export class Maps {
    public static addEvents(dotNetRef: any, mapId: string, events: MapEvent[]): void {
        EventManager.addEvents(dotNetRef, mapId, events);
    }

    public static removeEvents(mapId: string, events: MapEvent[]): void {
        EventManager.removeEvents(mapId, events);
    }

    public static addControls(mapId: string, controls: MapControlDef[]): void {
        ControlManager.add(mapId, controls);
    }

    public static getControls(mapId: string): object[] {
        return ControlManager.getControls(mapId);
    }

    public static removeControls(mapId: string, controls: MapControlDef[]): void {
        ControlManager.remove(mapId, controls);
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
        MarkerManager.add(mapId, markers);
    }

    public static removeMarkers(mapId: string, markers: HtmlMarkerDef[]) {
        MarkerManager.remove(mapId, markers);
    }

    public static addPopups(mapId: string, popups: PopupDef[]) {
        PopupManager.add(mapId, popups);
    }

    public static removePopups(mapId: string, popups: PopupDef[]) {
        PopupManager.remove(mapId, popups);
    }
}