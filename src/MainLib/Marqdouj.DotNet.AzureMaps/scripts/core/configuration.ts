import * as atlas from "azure-maps-control"
import { MapFactory } from "./factory"

export class Configuration {
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

    public static getServiceOptions(mapId: string): atlas.ServiceOptions {
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

    public static getStyle(mapId: string): atlas.StyleOptions {
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

    public static getUserInteraction(mapId: string): atlas.UserInteractionOptions {
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
