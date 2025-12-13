import * as atlas from "azure-maps-control";
import { MapFactory } from "../map-factory"
import { Logger, Extensions } from "../common"
import { HtmlMarkerDef, JsInteropObject } from "../typings"

export class Markers {
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
            const jsInterop: JsInteropObject = { id: markerDef.id, interopId: markerDef.interopId };
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

    static getMarker(mapId: string, id: string ): atlas.HtmlMarker {
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
            Logger.logMessage(mapId, Logger.LogLevel.Debug, `getMarker: marker not found where id = '${id}'`);
        }

        return marker;
    }

    static #isInteropHtmlMarker(obj: any, id?: string): obj is atlas.HtmlMarker {
        let ok = obj instanceof atlas.HtmlMarker && (obj as any).jsInterop.interopId != undefined;
        if (ok && Extensions.isNotEmptyOrNull(id)) {
            ok = (obj as any).jsInterop.id === id || (obj as any).jsInterop.interopId === id;
        }
        return ok;
    }
}
