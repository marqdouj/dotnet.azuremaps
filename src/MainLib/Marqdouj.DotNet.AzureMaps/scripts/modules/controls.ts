import * as atlas from "azure-maps-control";
import { Logger, LogLevel } from "./logger"
import { Helpers } from "./helpers"
import { MapFactory } from "../core/factory"
import { MapControlDef, JsInteropControl } from "./typings"

export class ControlManager {
    public static add(mapId: string, mapControls: MapControlDef[]): void {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

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

    static remove(mapId: string, items: MapControlDef[]) {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        const controls = map.controls.getControls();
        items.forEach(item => {
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

    public static getControls(mapId: string): JsInteropControl[] {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        const controls = map.controls.getControls();
        const result: JsInteropControl[] = [];

        controls.filter(control => this.#isInteropControl(control)).forEach(control => {
            const jsInterop = (control as any).jsInterop as JsInteropControl;
            if (jsInterop)
                result.push(jsInterop);
        });

        return result;
    }

    static #doGetControl(controls:atlas.Control[], mapId: string, id: string): atlas.Control {
        const control = controls.findLast(value => this.#isInteropControl(value, id));

        if (!control) {
            Logger.logMessage(mapId, LogLevel.Debug, `getControl: control not found where id = '${id}'`);
        }

        return control;
    }

    static #isInteropControl(obj: any, id?: string): obj is atlas.Control {
        let ok = obj && obj.jsInterop != undefined && obj.jsInterop.interopId != undefined;
        if (ok && Helpers.isNotEmptyOrNull(id)) {
            ok = obj.jsInterop.id === id || obj.jsInterop.interopId === id;
        }
        return ok;
    }
}
