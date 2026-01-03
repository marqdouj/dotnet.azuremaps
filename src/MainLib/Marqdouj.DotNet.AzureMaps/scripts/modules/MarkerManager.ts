import * as atlas from "azure-maps-control"
import { Logger, LogLevel } from "./common/Logger";
import { MapFactory } from "./MapFactory";
import { IMapReference, JsInteropDef } from "./typings";
import { Helpers } from "./common/Helpers";

export class MarkerManager {
    public static add(mapId: string, markers: HtmlMarkerDef[]): void {
        const mapRef = MapFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        markers ??= [];

        markers.forEach(markerDef => {
            let options = { ...(markerDef as any).options };
            if (options.popup) {
                options.popup = new atlas.Popup(options.popup.options)
            }
            let marker = new atlas.HtmlMarker(options);
            const jsInterop: JsInteropDef = { id: markerDef.id, interopId: markerDef.interopId };
            (marker as any).jsInterop = jsInterop;

            mapRef.map.markers.add(marker);

            if (markerDef.togglePopupOnClick) {
                mapRef.map.events.add('click', marker, () => {
                    marker.togglePopup();
                });
            }
        });
    }

    public static remove(mapId: string, markers: HtmlMarkerDef[]): void {
        const mapRef = MapFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        markers.forEach(markerDef => {
            let marker = this.#doGetMarker(mapRef, markerDef.interopId);
            if (marker) {
                mapRef.map.markers.remove(marker);
            }
        });
    }

    static getMarker(mapId: string, id: string): atlas.HtmlMarker {
        const mapRef = MapFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        return this.#doGetMarker(mapRef, id);
    }

    static #doGetMarker(mapRef: IMapReference, id: string): atlas.HtmlMarker {
        const markers = mapRef.map.markers.getMarkers();
        const marker = markers.findLast(value => this.#isInteropHtmlMarker(value, id));

        if (!marker) {
            Logger.logMessage(mapRef.mapId, LogLevel.Debug, `getMarker: marker not found where id = '${id}'`);
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

interface HtmlMarkerDef extends JsInteropDef {
    options: atlas.HtmlMarkerOptions;
    togglePopupOnClick: boolean;
}
