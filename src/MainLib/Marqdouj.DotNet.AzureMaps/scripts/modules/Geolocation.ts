export class Geolocation {
    static async getLocation(options?: PositionOptions): Promise<GetLocationResult> {
        console.debug("Geolocation.getLocation:", options);

        var result: GetLocationResult = { position: null, error: null };
        var getCurrentPositionPromise = new Promise((resolve, reject) => {
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
                console.debug("Geolocation Error:", error);
                result.error = { code: error.code, message: error.message };
                console.debug("Geolocation Error Result:", result);
            }
        );
        return result;
    }

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