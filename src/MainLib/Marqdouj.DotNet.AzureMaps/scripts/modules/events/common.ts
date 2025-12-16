import * as atlas from "azure-maps-control"
import { Logger, LogLevel } from "../../modules/logger"
import { MapContainerHelper } from "../../core/mapContainer"

export class EventFactoryBase {
    mapId: string;
    #helper: MapContainerHelper;

    constructor(mapId: string) {
        this.mapId = mapId;
        this.#helper = new MapContainerHelper(mapId);
    }

    getMap(): atlas.Map {
        return this.#helper.getMap();
    }

    getDotNetRef(): any {
        return this.#helper.getDotNetRef();
    }
}

export class MapEventLogger {
    static logEventAdd(mapId: string, name: string, wasAdded: boolean, event: MapEventDef) {
        if (wasAdded) {
            Logger.logMessage(mapId, LogLevel.Trace, `${name}: event added:`, event);
        } else {
            Logger.logMessage(mapId, LogLevel.Error, `${name}: event not supported:`, event);
        }
    }

    static logEventRemoved(mapId: string, name: string, wasRemoved: boolean, event: MapEventDef) {
        if (wasRemoved) {
            Logger.logMessage(mapId, LogLevel.Trace, `${name}: event removed:`, event);
        } else {
            Logger.logMessage(mapId, LogLevel.Error, `${name}: event not supported:`, event);
        }
    }

    static logInvalidTargetId(mapId: string, name: string, event: MapEventDef) {
        Logger.logMessage(mapId, LogLevel.Error, `${name}: 'once' is not supported:`, event);
        Logger.logMessage(mapId, LogLevel.Error, `${name}: invalid TargetId.`, event);
    }

    static logOnceNotSupported(mapId: string, name: string, event: MapEventDef) {
        Logger.logMessage(mapId, LogLevel.Error, `${name}: 'once' is not supported:`, event);
    }
}

export enum EventNotifications {
    NotifyMapEvent = 'NotifyMapEvent',
    NotifyMapEventConfig = 'NotifyMapEventConfig',
    NotifyMapEventData = 'NotifyMapEventData',
    NotifyMapEventDataSource = 'NotifyMapEventDataSource',
    NotifyMapEventError = 'NotifyMapEventError',
    NotifyMapEventHtmlMarker = 'NotifyMapEventHtmlMarker',
    NotifyMapEventLayer = 'NotifyMapEventLayer',
    NotifyMapEventMouse = 'NotifyMapEventMouse',
    NotifyMapEventPopup = "NotifyMapEventPopup",
    NotifyMapEventReady = 'NotifyMapEventReady',
    NotifyMapEventShape = 'NotifyMapEventShape',
    NotifyMapEventSource = 'NotifyMapEventSource',
    NotifyMapEventStyle = 'NotifyMapEventStyle',
    NotifyMapEventTouch = 'NotifyMapEventTouch',
    NotifyMapEventWheel = 'NotifyMapEventWheel',

    NotifyMapReady = 'NotifyMapReady',

}

export enum MapEventMouse {
    Click = 'click',
    ContextMenu = 'contextmenu',
    DblClick = 'dblclick',
    MouseDown = 'mousedown',
    MouseMove = 'mousemove',
    MouseOut = 'mouseout',
    MouseOver = 'mouseover',
    MouseUp = 'mouseup',
}

export enum MapEventTouch {
    TouchCancel = 'touchcancel',
    TouchEnd = 'touchend',
    TouchMove = 'touchmove',
    TouchStart = 'touchstart'
}

export enum MapEventWheel {
    Wheel = 'wheel',
}