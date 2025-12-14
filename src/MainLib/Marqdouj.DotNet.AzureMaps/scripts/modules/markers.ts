import * as atlas from "azure-maps-control";
import { Logger, LogLevel } from "./logger"
import { MapFactory } from "../core/factory"
import { Helpers } from "./helpers"
import { JsInteropDef, HtmlMarkerDef } from "./typings"

export class MarkerManager {
    static add(mapId: string, markers: HtmlMarkerDef[]) {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        markers.forEach(markerDef => {
            let options = { ...(markerDef as any).options };
            if (options.popup) {
                options.popup = new atlas.Popup(options.popup.options)
            }
            let marker = new atlas.HtmlMarker(options);
            const jsInterop: JsInteropDef = { id: markerDef.id, interopId: markerDef.interopId };
            (marker as any).jsInterop = jsInterop;

            map.markers.add(marker);

            if (markerDef.togglePopupOnClick) {
                map.events.add('click', marker, () => {
                    marker.togglePopup();
                });
            }
        });
    }

    static remove(mapId: string, markers: HtmlMarkerDef[]) {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        markers.forEach(markerDef => {
            let marker = this.#doGetMarker(map, mapId, markerDef.interopId);
            if (marker) {
                map.markers.remove(marker);
            }
        });
    }

    static getMarker(mapId: string, id: string): atlas.HtmlMarker {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        return this.#doGetMarker(map, mapId, id);
    }

    static #doGetMarker(map: atlas.Map, mapId: string, id: string): atlas.HtmlMarker {
        const markers = map.markers.getMarkers();
        const marker = markers.findLast(value => this.#isInteropHtmlMarker(value, id));

        if (!marker) {
            Logger.logMessage(mapId, LogLevel.Debug, `getMarker: marker not found where id = '${id}'`);
        }

        return marker;
    }

    static #isInteropHtmlMarker(obj: any, id?: string): obj is atlas.HtmlMarker {
        let ok = obj && obj.jsInterop != undefined && obj.jsInterop.interopId != undefined;
        if (ok && Helpers.isNotEmptyOrNull(id)) {
            ok = obj.jsInterop.id === id || obj.jsInterop.interopId === id;
        }
        return ok;
    }
}