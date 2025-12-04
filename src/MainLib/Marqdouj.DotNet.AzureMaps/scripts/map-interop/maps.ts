import * as atlas from "azure-maps-control";
import { MapFactory } from "../map-factory"
import { Logger } from "../common"
import { MapControl, DataSourceDef } from "../typings"

export class Maps {
    public static addControls(mapId: string, controls: MapControl[]): void {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        controls.forEach(control => {
            let mapControl: atlas.Control = null;

            switch (control.type.toLowerCase()) {
                case "compass":
                    mapControl = new atlas.control.CompassControl(control.options as atlas.CompassControlOptions);
                    break;
                case "fullscreen":
                    mapControl = new atlas.control.FullscreenControl(control.options as atlas.FullscreenControlOptions);
                    break;
                case "pitch":
                    mapControl = new atlas.control.PitchControl(control.options as atlas.PitchControlOptions);
                    break;
                case "scale":
                    mapControl = new atlas.control.ScaleControl(control.options as atlas.ScaleControlOptions);
                    break;
                case "style":
                    mapControl = new atlas.control.StyleControl(control.options as atlas.StyleControlOptions);
                    break;
                case "zoom":
                    mapControl = new atlas.control.ZoomControl(control.options as atlas.ZoomControlOptions);
                    break;
                default:
                    Logger.logMessage(mapId, Logger.LogLevel.Warn, `- addControls: control type '${control.type}' is not supported.`);
                    break;
            }

            if (mapControl) {
                (mapControl as any).jsInterop = {
                    id: control.id,
                    interopId: control.interopId,
                    type: control.type,
                    options: control.controlOptions
                };

                map.controls.add(mapControl, control.controlOptions);
            }
        });
    }

    public static getControls(mapId: string): object[] {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        const controls = map.controls.getControls();
        const result: object[] = [];

        controls.filter(control => this.#isInteropControl(control)).forEach(control => {
            const custom = control as any;
            const item: object = {
                id: custom.jsInterop.id,
                interopId: custom.jsInterop.interopId,
                type: custom.jsInterop.type,
                options: custom.jsInterop.options
            };
            result.push(item);
        });

        return result;
    }

    public static removeControls(mapId: string, controls: MapControl[]) {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        const mapControls = map.controls.getControls();

        mapControls.filter(control => this.#isInteropControl(control)).forEach(control => {
            const custom = control as any;

            if (controls.find(e => e.id === custom.jsInterop.id || e.interopId === custom.jsInterop.interopId)) {
                map.controls.remove(control);
            }
        });
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

    static #isInteropControl(obj: any): obj is atlas.Control {
        return obj && obj.jsInterop != undefined;
    }
}