import * as atlas from "azure-maps-control";
import { MapFactory } from "../map-factory"
import { Logger, Extensions } from "../common"
import { PopupDef, JsInteropObject } from "../typings"

export class Popups {
    static add(mapId: string, popups: PopupDef[]) {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        popups.forEach(popupDef => {
            let popup = new atlas.Popup(popupDef.options);
            const jsInterop: JsInteropObject = { id: popupDef.id, interopId: popupDef.interopId };
            (popup as any).jsInterop = jsInterop;

            map.popups.add(popup);
        });
    }

    static remove(mapId: string, popups: PopupDef[]) {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        popups.forEach(popupDef => {
            let popup = this.#doGetPopup(map, mapId, popupDef.interopId);
            if (popup) {
                map.popups.remove(popup);
            }
        });
    }

    static getPopup(mapId: string, id: string): atlas.Popup {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        return this.#doGetPopup(map, mapId, id);
    }

    static #doGetPopup(map: atlas.Map, mapId: string, id: string): atlas.Popup {
        const popups = map.popups.getPopups();
        const popup = popups.findLast(value => this.#isInteropPopup(value, id));

        if (!popup) {
            Logger.logMessage(mapId, Logger.LogLevel.Debug, `getPopup: popup not found where id = '${id}'`);
        }

        return popup;
    }

    static #isInteropPopup(obj: any, id?: string): obj is atlas.Popup {
        let ok = obj instanceof atlas.Popup && (obj as any).jsInterop.interopId != undefined;
        if (ok && Extensions.isNotEmptyOrNull(id)) {
            ok = (obj as any).jsInterop.id === id || (obj as any).jsInterop.interopId === id;
        }
        return ok;
    }
}
