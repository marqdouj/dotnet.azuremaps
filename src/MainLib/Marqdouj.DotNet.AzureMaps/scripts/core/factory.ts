import * as atlas from "azure-maps-control";
import { Logger, LogLevel } from "../modules/logger"
import { MapOptions, MapSettings, MapControlDef } from "../modules/typings"
import { ControlManager } from "../modules/controls"
import { EventManager, EventNotifications, MapEvent, MapEventInfo, MapEventAdd } from "../modules/events"

export class MapFactory {
    static #azmaps: Map<string, atlas.Map> = new Map<string, atlas.Map>();
    static getAuthTokenCallback: atlas.getAuthTokenCallback;

    static createMap(dotNetRef: any,
        mapId: string,
        settings: MapSettings,
        events?: MapEvent[],
        controls?: MapControlDef[]): void {

        Logger.currentLevel = settings.logLevel ?? LogLevel.Information;

        if (this.#azmaps.has(mapId)) {
            Logger.logMessage(mapId, LogLevel.Warn, "Map already exists.");
            return;
        }

        const options = this.#buildMapOptions(settings.authOptions, settings.options);
        const azmap = new atlas.Map(mapId, options);
        this.#azmaps.set(mapId, azmap);
        Logger.logMessage(mapId, LogLevel.Debug, "was created.");

        if (controls) {
            ControlManager.add(mapId, controls);
        }

        this.#addEvents(dotNetRef, mapId, events);
    }

    static getMap(mapId: string): atlas.Map | undefined {
        const map = this.#azmaps.get(mapId);

        if (!map) {
            Logger.logMessage(mapId, LogLevel.Debug, "was not found.");
        }

        return map;
    }

    static removeMap(mapId: string): void {
        if (this.#azmaps.delete(mapId)) {
            Logger.logMessage(mapId, LogLevel.Debug, "was removed");
        }
    }

    static #buildMapOptions(authOptions: atlas.AuthenticationOptions, mapOptions?: MapOptions): TBuildMapOptions {
        let options: TBuildMapOptions = {};

        if (mapOptions) {
            //Camera and CameraBounds are mutually exclusive
            if (mapOptions.camera) {
                options = { ...options, ...mapOptions.camera };
            }
            else if (mapOptions.cameraBounds) {
                options = { ...options, ...mapOptions.cameraBounds };
            }

            if (mapOptions.service) {
                options = { ...options, ...mapOptions.service };
            }

            if (mapOptions.style) {
                options = { ...options, ...mapOptions.style };
            }
            if (mapOptions.userInteraction) {
                options = { ...options, ...mapOptions.userInteraction };
            }
        }

        options.authOptions = authOptions;
        options.authOptions.getToken = this.getAuthTokenCallback;

        return options;
    }

    static #addEvents(dotNetRef: any, mapId: string, events: MapEvent[]): void {
        const azmap = this.getMap(mapId);

        if (!azmap) {
            Logger.logMessage(mapId, LogLevel.Error, "Cannot build events. Map not found.");
            return;
        }

        events ??= [];

        azmap.events.addOnce(MapEventAdd.Ready, event => {
            //MapEventError - always subscribe.
            azmap.events.add(MapEventAdd.Error, event => {
                const errorInfo: MapEventInfo = {
                    mapId: mapId,
                    type: MapEventAdd.Ready,
                    payload: { message: event.error.message, name: event.error.name, stack: event.error.stack, cause: event.error.cause }
                };

                Logger.logMessage(mapId, LogLevel.Error, 'Map error', errorInfo);
                dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventError, errorInfo);
            });

            EventManager.addEvents(dotNetRef, mapId, events);

            const readyInfo: MapEventInfo = { mapId: mapId, type: MapEventAdd.Ready };
            dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventReady, readyInfo);
            dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapReady, readyInfo);
        });
    }
}

type TBuildMapOptions = atlas.ServiceOptions & atlas.StyleOptions & atlas.UserInteractionOptions & (atlas.CameraOptions | atlas.CameraBoundsOptions);
