import * as atlas from "azure-maps-control"
import { Logger, LogLevel } from "./common/Logger";
import { MapFactory } from "./MapFactory";
import { IMapReference, JsInteropDef } from "./typings";
import { Helpers } from "./common/Helpers";

export class PopupManager {
    public static add(mapId: string, popups: PopupDef[]): void {
        const mapRef = MapFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        popups ??= [];

        popups.forEach(popupDef => {
            let popup = new atlas.Popup(popupDef.options);
            const jsInterop: JsInteropDef = { id: popupDef.id, interopId: popupDef.interopId };
            (popup as any).jsInterop = jsInterop;

            mapRef.map.popups.add(popup);
        });
    }

    public static remove(mapId: string, popups: PopupDef[]): void {
        const mapRef = MapFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        popups.forEach(popupDef => {
            let popup = this.#doGetPopup(mapRef, popupDef.interopId);
            if (popup) {
                mapRef.map.popups.remove(popup);
            }
        });
    }

    static getPopup(mapRef: IMapReference, id: string): atlas.Popup {
        return this.#doGetPopup(mapRef, id);
    }

    static #doGetPopup(mapRef: IMapReference, id: string): atlas.Popup {
        const popups = mapRef.map.popups.getPopups();
        const popup = popups.findLast(value => this.#isInteropPopup(value, id));

        if (!popup) {
            Logger.logMessage(mapRef.mapId, LogLevel.Debug, `getPopup: popup not found where id = '${id}'`);
        }

        return popup;
    }

    static #isInteropPopup(obj: any, id?: string): obj is atlas.Popup {
        let ok = obj && obj.jsInterop != undefined && obj.jsInterop.interopId != undefined;
        if (ok && Helpers.isNotEmptyOrNull(id)) {
            ok = obj.jsInterop.id === id || obj.jsInterop.interopId === id;
        }
        return ok;
    }
}

interface PopupDef extends JsInteropDef {
    options: atlas.PopupOptions;
}