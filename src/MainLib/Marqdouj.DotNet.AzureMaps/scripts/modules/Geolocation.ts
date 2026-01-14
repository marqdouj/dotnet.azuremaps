export class Geolocation {
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
    static async watchPosition(dotNetRef: any, callbackMethod: string, options: PositionOptions): Promise<number> {
        if (!navigator.geolocation) return null;

        return navigator.geolocation.watchPosition(
            (position: GeolocationPosition) => {
                const result: GetLocationResult = { position: position, error: null };
                dotNetRef.invokeMethodAsync(callbackMethod, result);
            },
            (error: any) => {
                const result: GetLocationResult = { position: null, error: { code: error.code, message: error.message } };
                dotNetRef.invokeMethodAsync(callbackMethod, result);
            },
            this.#buildOptions(options));
    }

    //https://developer.mozilla.org/en-US/docs/Web/API/Geolocation/clearWatch
    static clearWatch(id:number) {
        if (navigator.geolocation) {
            navigator.geolocation.clearWatch(id);
        }
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