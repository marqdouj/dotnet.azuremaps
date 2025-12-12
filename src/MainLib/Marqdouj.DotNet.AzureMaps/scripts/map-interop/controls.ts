import * as atlas from "azure-maps-control";
import { MapFactory } from "../map-factory"
import { Logger } from "../common"
import { MapControl } from "../typings"

export class Controls {
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
                const jsInterop: JsInteropControl = {
                    id: control.id,
                    interopId: control.interopId,
                    type: control.type,
                    options: control.options,
                    controlOptions: control.controlOptions,
                };

                (mapControl as any).jsInterop = jsInterop;

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
        const result: JsInteropControl[] = [];

        controls.filter(control => this.#isInteropControl(control)).forEach(control => {
            const jsInterop = (control as any).jsInterop as JsInteropControl;
            if (jsInterop)
                result.push(jsInterop);
        });

        return result;
    }

    public static removeControls(mapId: string, controls: MapControl[]) {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        const mapControls = map.controls.getControls();

        mapControls.filter(mapControl => this.#isInteropControl(mapControl)).forEach(mapControl => {
            const jsInterop = (mapControl as any).jsInterop as JsInteropControl;
            Logger.logMessage(mapId, Logger.LogLevel.Trace, "found control:", jsInterop);

            const control = controls.findLast(e => e.id == jsInterop.id && e.interopId == jsInterop.interopId);
            if (control) {
                map.controls.remove(mapControl);
                Logger.logMessage(mapId, Logger.LogLevel.Trace, "removing control:", control);
            }
        });
    }

    static #isInteropControl(obj: any): obj is atlas.Control {
        return obj && obj.jsInterop != undefined && obj.jsInterop.interopId != undefined;
    }
}

interface JsInteropControl {
    id: string,
    interopId: string,
    type: string,
    options: object;
    controlOptions: object;
}
