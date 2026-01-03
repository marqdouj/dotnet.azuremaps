import { Logger, LogLevel } from "../common/Logger";
import { MapEventDef } from "../typings";

export class EventsLogger {
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

    static logNotifyFired(mapId: string, name: string, ...optionalParams: any[]) {
        Logger.logMessage(mapId, LogLevel.Trace, `${name}: event was fired.`, optionalParams);
    }

    static logMessage(mapId: string, level: LogLevel, message: string, ...optionalParams: any[]) {
        Logger.logMessage(mapId, level, message, optionalParams);
    }
}