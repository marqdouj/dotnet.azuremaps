import * as atlas from "azure-maps-control"
import { MapFactory } from "./MapFactory";

export class MapManager {
    // #region Camera
    public static getCamera(mapId: string): any {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        return map.getCamera();
    }

    public static setCamera(mapId: string, options: CameraOptionsSet): void {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        let cameraOptions: TCameraOptionsSet = this.#buildCameraOptionsSet(options);
        map.setCamera(cameraOptions);
    }

    static #buildCameraOptionsSet(options: CameraOptionsSet): TCameraOptionsSet {
        let cameraOptions: TCameraOptionsSet = {};

        if (options.camera) {
            cameraOptions = { ...options.camera, ...options.animation };
        }
        else if (options.cameraBounds) {
            cameraOptions = { ...options.cameraBounds, ...options.animation };
        }

        return cameraOptions;
    }

    // #endregion

    // #region MapOptions
    public static getMapOptions(mapId: string): any {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        const mapCamera = map.getCamera();
        const service = map.getServiceOptions();
        const style = map.getStyle();
        const userInteraction = map.getUserInteraction();

        return {
            mapCamera: mapCamera,
            service: service,
            style: style,
            userInteraction: userInteraction
        };
    }

    public static setMapOptions(mapId: string, options: MapOptionsSet): void {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        let cameraOptions: TCameraOptionsSet = this.#buildCameraOptionsSet(options.camera);
        map.setCamera(cameraOptions);

        if (options.service) {
            map.setServiceOptions(options.service);
        }
        if (options.style) {
            map.setStyle(options.style);
        }
        if (options.userInteraction) {
            map.setUserInteraction(options.userInteraction);
        }
    }
    // #endregion

    // #region Service
    public static getServiceOptions(mapId: string): any {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        return map.getServiceOptions();
    }

    public static setServiceOptions(mapId: string, serviceOptions: atlas.ServiceOptions): void {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        map.setServiceOptions(serviceOptions);
    }
    // #endregion

    // #region Traffic
    public static getTraffic(mapId: string): atlas.TrafficOptions {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        return map.getTraffic();
    }

    public static setTraffic(mapId: string, options: atlas.TrafficOptions) {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        map.setTraffic(options);
    }
    // #endregion

    // #region Style
    public static getStyle(mapId: string): any {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        return map.getStyle();
    }

    public static setStyle(mapId: string, style: atlas.StyleOptions): void {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        map.setStyle(style);
    }
    // #endregion

    // #region User Interaction
    public static getUserInteraction(mapId: string): any {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        return map.getUserInteraction();
    }

    public static setUserInteraction(mapId: string, userInteraction: atlas.UserInteractionOptions): void {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }
        map.setUserInteraction(userInteraction);
    }
    // #endregion
}

type TCameraOptionsSet = (atlas.CameraOptions | (atlas.CameraBoundsOptions & { pitch?: number, bearing?: number })) & atlas.AnimationOptions;

interface CameraOptionsSet {
    camera?: atlas.CameraOptions;
    cameraBounds?: atlas.CameraBoundsOptions & { pitch?: number, bearing?: number };
    animation?: atlas.AnimationOptions;
}

interface MapOptionsSet {
    camera?: CameraOptionsSet;
    service?: atlas.ServiceOptions;
    style?: atlas.StyleOptions;
    userInteraction?: atlas.UserInteractionOptions;
}