import { Logger, LogLevel } from "./common/Logger";

export class GeolocationManager {
    static #watchIDs: Map<string, number> = new Map<string, number>;

    //https://developer.mozilla.org/en-US/docs/Web/API/Geolocation/getCurrentPosition
    static async getLocation(options?: PositionOptions): Promise<GetLocationResult> {
        const result: GetLocationResult = { position: null, error: null };
        const getCurrentPositionPromise = new Promise((resolve, reject) => {
            if (!navigator.geolocation) {
                reject({ code: 0, message: 'This device does not support geolocation.' });
            } else {
                navigator.geolocation.getCurrentPosition(resolve, reject, this.#buildOptions(options));
            }
        });
        await getCurrentPositionPromise.then(
            (position: GeolocationPosition) => { result.position = position; }
        ).catch(
            (error: any) => {
                result.error = { code: error.code, message: error.message };
            }
        );
        return result;
    }

    //https://developer.mozilla.org/en-US/docs/Web/API/Geolocation/watchPosition
    static async watchPosition(dotNetRef: any, mapId: string, callbackMethod: string, options: PositionOptions): Promise<number> {
        if (!navigator.geolocation) return null;

        if (this.isWatched(mapId)) {
            Logger.logMessage(mapId, LogLevel.Warn, "Geolocation.watchPosition: watch has already been added.");
            return;
        }

        const id = navigator.geolocation.watchPosition(
            (position: GeolocationPosition) => {
                if (this.isWatched(mapId)) {
                    const result: GetLocationResult = { position: position, error: null };
                    dotNetRef.invokeMethodAsync(callbackMethod, result);
                }
            },
            (error: any) => {
                const result: GetLocationResult = { position: null, error: { code: error.code, message: error.message } };
                dotNetRef.invokeMethodAsync(callbackMethod, result);
            },
            this.#buildOptions(options));

        Logger.logMessage(mapId, LogLevel.Trace, "Geolocation.watchPosition:", id);
        this.#watchIDs.set(mapId, id);
        return id;
    }

    //https://developer.mozilla.org/en-US/docs/Web/API/Geolocation/clearWatch
    static clearWatch(mapId: string) {
        if (!navigator.geolocation) return;

        if (this.#watchIDs.has(mapId)) {
            const id = this.#watchIDs.get(mapId);
            Logger.logMessage(mapId, LogLevel.Trace, "Geolocation.clearWatch:", id);
            navigator.geolocation.clearWatch(id);
            this.#watchIDs.delete(mapId);
        }
    }

    static isWatched(mapId: string) {
        return this.#watchIDs.has(mapId);
    }

    //If options are passed by JsInterop and timeout is null, it usually fails.
    //To workaround this, create an object that has only the values that contain an actual value.
    static #buildOptions(options?: PositionOptions) {
        if (options) {
            const result: PositionOptions = {};

            if (options.enableHighAccuracy) {
                result.enableHighAccuracy = options.enableHighAccuracy;
            }
            if (options.maximumAge) {
                result.maximumAge = options.maximumAge;
            }
            if (options.timeout) {
                result.timeout = options.timeout;
            }

            return result;
        }

        return undefined;
    }
}

type GetLocationResult = {
    position: any;
    error: any;
}