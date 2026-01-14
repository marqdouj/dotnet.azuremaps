import * as atlas from "azure-maps-control"
import { MapReference } from "./MapReference";
import { Logger, LogLevel } from "./common/Logger"
import { MapEventDef, MapControlDef, IMapReference } from "./typings";
import { EventManager, EventNotification } from "./EventManager";
import { ControlManager } from "./ControlManager";
import { EventsHelper } from "./events/EventsHelper";
import { Helpers } from "./common/Helpers";
import { GeolocationManager } from "./GeolocationManager"

export class MapFactory {
    static #azmaps: Map<string, MapReference> = new Map<string, MapReference>();
    static getAuthTokenCallback: atlas.getAuthTokenCallback;

    static createMap(
        dotNetRef: any,
        mapId: string,
        settings: MapSettings,
        events?: MapEventDef[],
        controls?: MapControlDef[]): void {

        Logger.currentLevel = settings.logLevel ?? LogLevel.Information;

        if (this.#azmaps.has(mapId)) {
            Logger.logMessage(mapId, LogLevel.Warn, "Map already exists.");
            return;
        }

        const options = this.#buildMapOptions(mapId, settings.authOptions, settings.options);
        const azmap = new atlas.Map(mapId, options);
        const mapReference = new MapReference(dotNetRef, mapId, azmap);
        this.#azmaps.set(mapId, mapReference);
        Logger.logMessage(mapId, LogLevel.Debug, "was created.");

        ControlManager.add(mapId, controls);

        this.#addEvents(mapReference, events);
    }

    static getMap(mapId: string): atlas.Map | undefined {
        const mapReference = this.getMapReference(mapId);
        return mapReference?.map;
    }

    static getMapReference(mapId: string): IMapReference | undefined {
        const mapReference = this.#azmaps.get(mapId);

        if (!mapReference) {
            Logger.logMessage(mapId, LogLevel.Debug, "MapManager was not found.");
        }

        return mapReference as IMapReference;
    }

    static removeMap(mapId: string): void {
        const mapReference = this.#azmaps.get(mapId);

        if (mapReference) {
            GeolocationManager.clearWatch(mapId);

            if (this.#azmaps.delete(mapId)) {
                mapReference.clear();
                Logger.logMessage(mapId, LogLevel.Debug, "was removed");
            }
        }
    }

    static #buildMapOptions(mapId: string, authOptions: atlas.AuthenticationOptions, mapOptions?: MapOptions): TBuildMapOptions {
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
        const sasTokenUrl = (authOptions as any).sasTokenUrl;
        if (Helpers.isNotEmptyOrNull(sasTokenUrl)) {
            Logger.logMessage(mapId, LogLevel.Trace, "buildMapOptions with SasTokenUrl:", sasTokenUrl);
            options.authOptions.getToken = function (resolve, reject, map) {
                fetch(sasTokenUrl).then(r => r.text()).then(token => resolve(token));
            }
        }
        else {
            options.authOptions.getToken = this.getAuthTokenCallback;
        }

        return options;
    }

    static #addEvents(mapReference: MapReference, events: MapEventDef[]): void {
        const dotNetRef = mapReference.dotNetRef;
        const mapId = mapReference.mapId;
        const azmap = mapReference.map;

        if (!azmap) {
            Logger.logMessage(mapId, LogLevel.Error, "Cannot build events. Map not found.");
            return;
        }

        events ??= [];

        azmap.events.addOnce(MapEventAdd.Ready, event => {
            const errorDef: MapEventDef = { target: "map", type: MapEventAdd.Error };
            const readyDef: MapEventDef = { target: "map", type: MapEventAdd.Ready };

            //MapEventError - always subscribe.
            azmap.events.add(MapEventAdd.Error, event => {
                const payload = { message: event.error.message, name: event.error.name, stack: event.error.stack, cause: event.error.cause };
                let result = EventsHelper.buildEventResult(mapId, errorDef, payload);

                Logger.logMessage(mapId, LogLevel.Error, 'Map error', result);
                dotNetRef.invokeMethodAsync(EventNotification.MapError, result);
            });

            EventManager.add(mapId, events);

            let result = EventsHelper.buildEventResult(mapId, readyDef);
            dotNetRef.invokeMethodAsync(EventNotification.MapReady, result);
        });
    }
}

interface MapOptions {
    camera?: atlas.CameraOptions;
    cameraBounds?: atlas.CameraBoundsOptions;
    service?: atlas.ServiceOptions;
    style?: atlas.StyleOptions;
    userInteraction?: atlas.UserInteractionOptions;
}

interface MapSettings {
    authOptions: atlas.AuthenticationOptions;
    options?: MapOptions;
    logLevel?: LogLevel;
    inDevelopment?: boolean;
}

enum MapEventAdd {
    Error = 'error',
    Ready = 'ready',
}

type TBuildMapOptions = atlas.ServiceOptions & atlas.StyleOptions & atlas.UserInteractionOptions & (atlas.CameraOptions | atlas.CameraBoundsOptions);

