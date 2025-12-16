import * as atlas from "azure-maps-control"
import { Logger, LogLevel } from "../modules/logger"
import { MapOptions, MapSettings, MapControlDef } from "../modules/typings"
import { ControlManager } from "../modules/controls"
import { MapContainer } from "./mapContainer"
import { Helpers } from "../modules/helpers"
import { EventNotifications } from "../modules/events/common"
import { Maps } from "./maps"

export class MapFactory {
    static #azmaps: Map<string, MapContainer> = new Map<string, MapContainer>();
    static getAuthTokenCallback: atlas.getAuthTokenCallback;

    static createMap(dotNetRef: any,
        mapId: string,
        settings: MapSettings,
        events?: MapEventDef[],
        controls?: MapControlDef[]): void {

        Logger.currentLevel = settings.logLevel ?? LogLevel.Information;

        if (this.#azmaps.has(mapId)) {
            Logger.logMessage(mapId, LogLevel.Warn, "Map already exists.");
            return;
        }

        const options = this.#buildMapOptions(settings.authOptions, settings.options);
        const azmap = new atlas.Map(mapId, options);
        this.#azmaps.set(mapId, new MapContainer(dotNetRef, mapId, azmap));
        Logger.logMessage(mapId, LogLevel.Debug, "was created.");

        if (controls) {
            ControlManager.add(mapId, controls);
        }

        this.#addEvents(dotNetRef, mapId, events);
    }

    static getMap(mapId: string): atlas.Map | undefined {
        const mapContainer = this.getMapContainer(mapId);
        return mapContainer?.azMap;
    }

    static getMapContainer(mapId: string): MapContainer | undefined {
        const mapContainer = this.#azmaps.get(mapId);

        if (!mapContainer) {
            Logger.logMessage(mapId, LogLevel.Debug, "mapContainer was not found.");
        }

        return mapContainer;
    }

    static removeMap(mapId: string): void {
        const mapContainer = this.#azmaps.get(mapId);

        if (mapContainer) {
            if (this.#azmaps.delete(mapId)) {
                mapContainer.clear();
                Logger.logMessage(mapId, LogLevel.Debug, "was removed");
            }
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

    static #addEvents(dotNetRef: any, mapId: string, events: MapEventDef[]): void {
        const azmap = this.getMap(mapId);

        if (!azmap) {
            Logger.logMessage(mapId, LogLevel.Error, "Cannot build events. Map not found.");
            return;
        }

        events ??= [];

        azmap.events.addOnce(MapEventAdd.Ready, event => {
            //MapEventError - always subscribe.
            azmap.events.add(MapEventAdd.Error, event => {
                const payload = { message: event.error.message, name: event.error.name, stack: event.error.stack, cause: event.error.cause };
                let result = Helpers.buildEventResult(mapId, MapEventAdd.Ready, payload);

                Logger.logMessage(mapId, LogLevel.Error, 'Map error', result);
                dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventError, result);
            });

            Maps.addEvents(mapId, events);

            let result = Helpers.buildEventResult(mapId, MapEventAdd.Ready, null);
            dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapEventReady, result);
            dotNetRef.invokeMethodAsync(EventNotifications.NotifyMapReady, result);
        });
    }
}

enum MapEventAdd {
    Error = 'error',
    Ready = 'ready',
}

type TBuildMapOptions = atlas.ServiceOptions & atlas.StyleOptions & atlas.UserInteractionOptions & (atlas.CameraOptions | atlas.CameraBoundsOptions);
