import * as atlas from "azure-maps-control";
import { MapFactory } from "../map-factory"
import { MapControl, DataSourceDef, TMapFeature, MapLayerDef, MapOptions, SetCameraOptions } from "../typings"

export namespace MapInterop {
    export class Configuration {
        public static getMapOptions(mapId: string): any {
            const map = MapFactory.getMap(mapId);
            if (!map) {
                return;
            }

            const service = map.getServiceOptions();
            const camera = map.getCamera();
            const style = map.getStyle();
            const userInteraction = map.getUserInteraction();

            return {
                camera: camera,
                service: service,
                style: style,
                userInteraction: userInteraction
            };
        }

        /**
         * Sets the map options for the specified map.
         * @param mapId The ID of the map to set the options for.
         * @param options The map options to set, which can include camera, service, style, and userInteraction.
         */
        public static setMapOptions(mapId: string, options: MapOptions): void {
            const map = MapFactory.getMap(mapId);
            if (!map) {
                return;
            }

            if (options.camera) {
                map.setCamera(options.camera);
            }
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

        /**
         * Gets the camera options for the specified map.
         * @param mapId The ID of the map to get the camera options for.
         * @returns The camera options of the map, or undefined if the map does not exist.
         */
        public static getCamera(mapId: string) {
            const map = MapFactory.getMap(mapId);
            if (!map) {
                return;
            }
            return map.getCamera();
        }

        /**
         * Sets the camera options for the specified map.
         * @param mapId The ID of the map to set the camera options for.
         * @param options The camera options to set.
         */
        public static setCamera(
            mapId: string,
            options: SetCameraOptions): void {

            const map = MapFactory.getMap(mapId);
            if (!map) {
                return;
            }

            let cameraOptions: (atlas.CameraOptions | (atlas.CameraBoundsOptions & { pitch?: number, bearing?: number })) & atlas.AnimationOptions = {};

            if (options.camera) {
                cameraOptions = { ...options.camera };
            }

            if (options.cameraBounds) {
                cameraOptions = { ...cameraOptions, ...options.cameraBounds };
            }

            if (options.animation) {
                cameraOptions = { ...cameraOptions, ...options.animation };
            }

            map.setCamera(cameraOptions);
        }

        /**
         * Gets the user interaction options for the specified map.
         * @param mapId The ID of the map to get the user interaction options for.
         * @returns The user interaction options of the map, or undefined if the map does not exist.
         */
        public static getServiceOptions(mapId: string): atlas.ServiceOptions {
            const map = MapFactory.getMap(mapId);
            if (!map) {
                return;
            }
            return map.getServiceOptions();
        }

        /**
         * Sets the user interaction options for the specified map.
         * @param mapId The ID of the map to set the user interaction options for.
         * @param serviceOptions The user interaction options to set.
         */
        public static setServiceOptions(mapId: string, serviceOptions: atlas.ServiceOptions): void {
            const map = MapFactory.getMap(mapId);
            if (!map) {
                return;
            }
            map.setServiceOptions(serviceOptions);
        }

        /**
         * Gets the style options for the specified map.
         * @param mapId The ID of the map to get the style options for.
         * @returns The style options of the map, or undefined if the map does not exist.
         */
        public static getStyle(mapId: string): atlas.StyleOptions {
            const map = MapFactory.getMap(mapId);
            if (!map) {
                return;
            }
            return map.getStyle();
        }

        /**
         * Sets the style options for the specified map.
         * @param mapId The ID of the map to set the style options for.
         * @param style The style options to set.
         */
        public static setStyle(mapId: string, style: atlas.StyleOptions): void {
            const map = MapFactory.getMap(mapId);
            if (!map) {
                return;
            }
            map.setStyle(style);
        }

        /**
         * Gets the user interaction options for the specified map.
         * @param mapId The ID of the map to get the user interaction options for.
         * @returns The user interaction options of the map, or undefined if the map does not exist.
         */
        public static getUserInteraction(mapId: string): atlas.UserInteractionOptions {
            const map = MapFactory.getMap(mapId);
            if (!map) {
                return;
            }
            return map.getUserInteraction();
        }

        /**
         * Sets the user interaction options for the specified map.
         * @param mapId The ID of the map to set the user interaction options for.
         * @param userInteraction The user interaction options to set.
         */
        public static setUserInteraction(mapId: string, userInteraction: atlas.UserInteractionOptions): void {
            const map = MapFactory.getMap(mapId);
            if (!map) {
                return;
            }
            map.setUserInteraction(userInteraction);
        }
    }

    export class Layers {
        public static createLayer(mapId: string, def: MapLayerDef, dsDef?: DataSourceDef) {
            const map = MapFactory.getMap(mapId);
            if (!map) {
                return;
            }

            const idHeader = `createLayer - ${Extensions.logHeader(mapId)}`;

            if (Extensions.isEmptyOrNull(def.type)) {
                console.error(`${idHeader} error: layer type is missing`);
                return;
            }

            const layerId = def.id;

            if (Extensions.isNotEmptyOrNull(layerId)) {
                const lyr = map.layers.getLayerById(layerId);
                if (lyr) {
                    console.error(`${idHeader} error: layer already exists where ID=${layerId}`);
                    return;
                }
            }

            let ds: atlas.source.DataSource;

            if (dsDef) {
                ds = new atlas.source.DataSource(dsDef.id, dsDef.options);
                if (!ds) {
                    console.error(`${idHeader} error: Unable to create datasource.`, dsDef);
                    return;
                }
                map.sources.add(ds);

                if (Extensions.isNotEmptyOrNull(dsDef.url)) {
                    ds.importDataFromUrl(dsDef.url);
                }
            }
            else {
                ds = map.sources.getById(def.sourceId) as atlas.source.DataSource;
                if (!ds) {
                    console.error(`${idHeader} error: Unable to find datasource where ID=${def.sourceId}.`);
                    return;
                }
            }

            switch (def.type) {
                case 'Bubble':
                    map.layers.add(new atlas.layer.BubbleLayer(ds, layerId, def.options as atlas.BubbleLayerOptions), def.before);
                    break;
                case 'HeatMap':
                    map.layers.add(new atlas.layer.HeatMapLayer(ds, layerId, def.options as atlas.HeatMapLayerOptions), def.before);
                    break;
                case 'Image':
                    map.layers.add(new atlas.layer.ImageLayer(def.options as atlas.ImageLayerOptions, layerId), def.before);
                    break;
                case 'Line':
                    map.layers.add(new atlas.layer.LineLayer(ds, layerId, def.options as atlas.LineLayerOptions), def.before);
                    break;
                case 'Polygon':
                    map.layers.add(new atlas.layer.PolygonLayer(ds, layerId, def.options as atlas.PolygonLayerOptions), def.before);
                    break;
                case 'PolygonExtrusion':
                    map.layers.add(new atlas.layer.PolygonExtrusionLayer(ds, layerId, def.options as atlas.PolygonExtrusionLayerOptions), def.before);
                    break;
                case 'Symbol':
                    map.layers.add(new atlas.layer.SymbolLayer(ds, layerId, def.options as atlas.SymbolLayerOptions), def.before);
                    break;
                case 'Tile':
                    map.layers.add(new atlas.layer.TileLayer(def.options as atlas.TileLayerOptions, layerId), def.before);
                    break;
                default:
                    console.error(`${idHeader} error: layer type '${def.type}' is not supported`);
                    break;
            }
        }

        public static removeLayer(mapId: string, id: string) {
            const map = MapFactory.getMap(mapId);
            if (!map) {
                return;
            }

            const lyr = map.layers.getLayerById(id);
            if (lyr) {
                map.layers.remove(lyr);
                console.debug(`${Extensions.logHeader(mapId)} - layer with id '${id}' was removed.`);
            }
        }

        public static addMapFeature(
            mapId: string,
            mapFeature: TMapFeature,
            datasourceId: string,
            replace?: boolean) {

            const map = MapFactory.getMap(mapId);
            if (!map) {
                return;
            }

            const ds = map.sources.getById(datasourceId) as atlas.source.DataSource;

            if (replace && mapFeature.id) {
                const shape = ds.getShapeById(mapFeature.id);
                if (shape) {
                    ds.remove(shape);
                }
            }

            let geom: atlas.data.Geometry;
            const geomType = mapFeature.geometry.type as string;

            switch (geomType.toLowerCase()) {
                case GeoJSONType.Point:
                    geom = new atlas.data.Point(mapFeature.geometry.coordinates);
                    break;
                case GeoJSONType.MultiPoint:
                    geom = new atlas.data.MultiPoint(mapFeature.geometry.coordinates, mapFeature.bbox);
                    break;
                case GeoJSONType.LineString:
                    geom = new atlas.data.LineString(mapFeature.geometry.coordinates, mapFeature.bbox);
                    break;
                case GeoJSONType.Polygon:
                    geom = new atlas.data.Polygon(mapFeature.geometry.coordinates, mapFeature.bbox);
                    break;
            }

            if (!geom) {
                console.error(`Map with ID '${mapId}' - adding feature error: geometry type '${mapFeature.geometry.type}' not supported`);
                return;
            }

            const feature = new atlas.data.Feature(geom, mapFeature.properties, mapFeature.id)
            if (mapFeature.asShape) {
                const shape = new atlas.Shape(feature); 
                ds.add(shape);
            }
            else {
                ds.add(feature);
            }
        }
    }

    export class Map {
        public static createDatasource(mapId: string, source: DataSourceDef): void {
            const map = MapFactory.getMap(mapId);
            if (!map) {
                return;
            }

            const ds = map.sources.getById(source.id);
            if (ds) {
                console.warn(`Map with ID '${mapId}' - datasource with ID '${source.id}' already exists`);
                return;
            }

            const newDs = new atlas.source.DataSource(source.id, source.options);
            map.sources.add(newDs);

            if (source.url) {
                newDs.importDataFromUrl(source.url);
            }
        }

        public static removeDatasource(mapId: string, id: string): void {
            const map = MapFactory.getMap(mapId);
            if (!map) {
                return;
            }

            const ds = map.sources.getById(id) as atlas.source.DataSource;
            if (ds) {
                map.sources.remove(ds);
                console.debug(`${Extensions.logHeader(mapId)} - datasource with id '${id}' was removed.`);
            }
        }

        public static clearDatasource(mapId: string, id: string): void {
            const map = MapFactory.getMap(mapId);
            if (!map) {
                return;
            }

            const ds = map.sources.getById(id) as atlas.source.DataSource;
            if (ds) {
                ds.clear();
            }
        }

        public static addControls(mapId: string, controls: MapControl[]): void {
            const map = MapFactory.getMap(mapId);
            if (!map) {
                return;
            }

            controls.forEach(control => {
                let mapControl: atlas.Control = null;

                switch (control.type.toLowerCase()) {
                    case "compass":
                        mapControl = new atlas.control.CompassControl(control.options);
                        break;
                    case "fullscreen":
                        mapControl = new atlas.control.FullscreenControl(control.options);
                        break;
                    case "pitch":
                        mapControl = new atlas.control.PitchControl(control.options);
                        break;
                    case "scale":
                        mapControl = new atlas.control.ScaleControl(control.options);
                        break;
                    case "style":
                        mapControl = new atlas.control.StyleControl(control.options);
                        break;
                    case "zoom":
                        mapControl = new atlas.control.ZoomControl(control.options);
                        break;
                }

                if (mapControl) {
                    (mapControl as any).jsInterop = {
                        id: control.id,
                        interopId: control.interopId,
                        type: control.type,
                        options: control.controlOptions
                    };

                    map.controls.add(mapControl, control.controlOptions);
                }
            });
        }

        /**
        * Gets the control info for controls that were added by addControls.
        */
        public static getControls(mapId: string): object[] {
            const map = MapFactory.getMap(mapId);
            if (!map) {
                return;
            }

            const controls = map.controls.getControls();
            const result: object[] = [];

            controls.filter(control => this.#isInteropControl(control)).forEach(control => {
                const custom = control as any;
                const item: object = {
                    id: custom.jsInterop.id,
                    interopId: custom.jsInterop.interopId,
                    type: custom.jsInterop.type,
                    options: custom.jsInterop.options
                };
                result.push(item);
            });

            return result;
        }

        public static removeControls(mapId: string, controls: MapControl[]) {
            const map = MapFactory.getMap(mapId);
            if (!map) {
                return;
            }

            const mapControls = map.controls.getControls();

            mapControls.filter(control => this.#isInteropControl(control)).forEach(control => {
                const custom = control as any;

                if (controls.find(e => e.id === custom.jsInterop.id || e.interopId === custom.jsInterop.interopId)) {
                    map.controls.remove(control);
                }
            });
        }

        static #isInteropControl(obj: any): obj is atlas.Control {
            return obj && obj.jsInterop != undefined;
        }
    }

    class Extensions {
        static logHeader(mapId: string): string {
            return `Map with ID '${mapId}'`;
        }

        static isEmptyOrNull(str: string | null | undefined): boolean {
            return str?.trim() === "";
        }

        static isNotEmptyOrNull(str: string | null | undefined): boolean {
            return !this.isEmptyOrNull(str);
        }
    }

    enum GeoJSONType {
        Point = 'point',
        MultiPoint = 'multipoint',
        LineString = 'linestring',
        MultiLineString = 'multilinestring',
        Polygon = 'polygon',
        MultiPolygon = 'multipolygon'
    }
}

