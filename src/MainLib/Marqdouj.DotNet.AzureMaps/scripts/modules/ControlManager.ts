import * as atlas from "azure-maps-control"
import { Logger, LogLevel } from "./common/Logger";
import { MapFactory } from "./MapFactory";
import { MapControlDef, JsInteropControl } from "./typings";
import { Helpers } from "./common/Helpers";

export class ControlManager {
    public static add(mapId: string, mapControls: MapControlDef[]): void {
        const map = MapFactory.getMap(mapId);
        if (!map) return;

        mapControls ??= [];

        mapControls.forEach(mapControl => {
            let control: atlas.Control = null;

            switch (mapControl.type.toLowerCase()) {
                case "compass":
                    control = new atlas.control.CompassControl(mapControl.options as atlas.CompassControlOptions);
                    break;
                case "fullscreen":
                    control = new atlas.control.FullscreenControl(mapControl.options as atlas.FullscreenControlOptions);
                    break;
                case "pitch":
                    control = new atlas.control.PitchControl(mapControl.options as atlas.PitchControlOptions);
                    break;
                case "scale":
                    control = new atlas.control.ScaleControl(mapControl.options as atlas.ScaleControlOptions);
                    break;
                case "style":
                    control = new atlas.control.StyleControl(mapControl.options as atlas.StyleControlOptions);
                    break;
                case "traffic":
                    control = new atlas.control.TrafficControl(mapControl.options as atlas.TrafficControlOptions);
                    break;
                case "zoom":
                    control = new atlas.control.ZoomControl(mapControl.options as atlas.ZoomControlOptions);
                    break;
                default:
                    Logger.logMessage(mapId, LogLevel.Warn, `addControls: control type '${mapControl.type}' is not supported.`);
                    break;
            }

            if (control) {
                const jsInterop: JsInteropControl = {
                    id: mapControl.id,
                    interopId: mapControl.interopId,
                    type: mapControl.type,
                };

                (control as any).jsInterop = jsInterop;

                map.controls.add(control, mapControl.controlOptions);
            }
        });
    }

    public static remove(mapId: string, mapControls: MapControlDef[]): void {
        const map = MapFactory.getMap(mapId);
        if (!map) return;

        const controls = map.controls.getControls();
        mapControls.forEach(item => {
            let control = this.#doGetControl(controls, mapId, item.interopId);
            if (control) {
                map.controls.remove(control);
            }
        });
    }

    static getControl(mapId: string, id: string): atlas.Control {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        const controls = map.controls.getControls();
        return this.#doGetControl(controls, mapId, id);
    }

    static #doGetControl(controls: atlas.Control[], mapId: string, id: string): atlas.Control {
        const control = controls.findLast(value => Helpers.isInteropControl(value, id));

        if (!control) {
            Logger.logMessage(mapId, LogLevel.Debug, `getControl: control not found where id = '${id}'`);
        }

        return control;
    }

    public static getControls(mapId: string): JsInteropControl[] {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        const controls = map.controls.getControls();
        const result: JsInteropControl[] = [];

        controls.filter(control => Helpers.isInteropControl(control)).forEach(control => {
            const jsInterop = (control as any).jsInterop;
            if (jsInterop)
                result.push(jsInterop);
        });

        return result;
    }
}
