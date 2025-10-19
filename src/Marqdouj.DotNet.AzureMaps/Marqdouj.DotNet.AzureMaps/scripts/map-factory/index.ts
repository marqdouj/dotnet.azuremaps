import * as atlas from "azure-maps-control";
import { MapEvents } from "./events"
import { MapOptions, TOptions, MapControl } from "../typings"
import { MapInterop } from "../map-interop";

export class MapFactory {
    static #azmaps: Map<string, atlas.Map> = new Map<string, atlas.Map>();
    static getAuthTokenCallback: atlas.getAuthTokenCallback;

    static addMap(
        dotNetRef: any,
        mapId: string,
        authOptions: atlas.AuthenticationOptions,
        mapOptions: MapOptions,
        events: string[],
        controls?: MapControl[]): void {

        const header = this.#logHeader(mapId);

        if (this.#azmaps.has(mapId)) {
            console.warn(`${header} already exists.`);
            return;
        }

        const options = this.#buildMapOptions(authOptions, mapOptions);
        const azmap = new atlas.Map(mapId, options);

        this.#azmaps.set(mapId, azmap);
        console.debug(`${header} was added.`);

        this.#buildEvents(dotNetRef, mapId, events, controls);
    }

    static getMap(mapId: string): atlas.Map | undefined {
        const map = this.#azmaps.get(mapId);
        const header = this.#logHeader(mapId);

        if (!map) {
            console.warn(`${header} was not found.`);
        }
        return map;
    }

    static removeMap(mapId: string): void {
        const removed = this.#azmaps.delete(mapId);
        const header = this.#logHeader(mapId);

        if (removed) {
            console.debug(`${header} was removed.`);
        }
    }

    static #buildMapOptions(authOptions: atlas.AuthenticationOptions, mapOptions?: MapOptions): TOptions {
        let options: TOptions = {};

        if (mapOptions) {
            if (mapOptions.service) {
                options = { ...mapOptions.service };
            }
            if (mapOptions.camera) {
                options = { ...options, ...mapOptions.camera };
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

    static #buildEvents(dotNetRef: any, mapId: string, events: string[], controls?: MapControl[]): void {
        const azmap = this.getMap(mapId);
        const header = this.#logHeader(mapId);

        events ??= [];

        azmap.events.addOnce('ready', event => {
            //MapEventError
            if (events.includes('error')) {
                azmap.events.add('error', event => {
                    console.error(`${header} error:`, event.error);
                    dotNetRef.invokeMethodAsync(MapEvents.EventNotifications.NotifyMapEventError,
                        {
                            mapId: mapId,
                            type: 'error',
                            payload: { message: event.error.message, name: event.error.name, stack: event.error.stack, cause: event.error.cause }
                        });
                });
            }

            //MapEventGeneral
            Object.values(MapEvents.MapEventGeneral).filter(value => events.includes(value)).forEach((value) => {
                azmap.events.add(value as any, () => {
                    dotNetRef.invokeMethodAsync(MapEvents.EventNotifications.NotifyMapEvent, { mapId: mapId, type: value });
                });
            });

            //MapEventConfig
            Object.values(MapEvents.MapEventConfig).filter(value => events.includes(value)).forEach((value) => {
                azmap.events.add(value as any, (config: atlas.MapConfiguration) => {
                    let result = {
                        mapId: mapId,
                        type: value,
                        payload: config
                    };
                    dotNetRef.invokeMethodAsync(MapEvents.EventNotifications.NotifyMapEventConfig, result);
                });
            });

            //MapEventData
            Object.values(MapEvents.MapEventData).filter(value => events.includes(value)).forEach((value) => {
                azmap.events.add(value as any, (dataEvent: atlas.MapDataEvent) => {
                    let result = {
                        mapId: mapId,
                        type: value,
                        payload: {
                            dataType: dataEvent.dataType,
                            isSourceLoaded: dataEvent.isSourceLoaded,
                            source: dataEvent.source?.getId(),
                            sourceDataType: dataEvent.sourceDataType,
                            tile: dataEvent.tile
                        }
                    };

                    dotNetRef.invokeMethodAsync(MapEvents.EventNotifications.NotifyMapEventData, result);
                });
            });

            //MapEventLayer
            Object.values(MapEvents.MapEventLayer).filter(value => events.includes(value)).forEach((value) => {
                azmap.events.add(value as any, (layer: atlas.layer.Layer) => {
                    let result = {
                        mapId: mapId,
                        type: value,
                        payload: {
                            id: layer.getId(),
                        }
                    }
                    dotNetRef.invokeMethodAsync(MapEvents.EventNotifications.NotifyMapEventLayer, result);
                });
            });

            //MapEventMouse
            Object.values(MapEvents.MapEventMouse).filter(value => events.includes(value)).forEach((value) => {
                azmap.events.add(value as any, (mouseEvent: atlas.MapMouseEvent) => {
                    try {
                        const shapes: object[] = [];

                        mouseEvent.shapes.filter(feature => this.#isFeature(feature)).forEach(feature => {
                            shapes.push(this.#getFeatureResult(feature));
                        });

                        mouseEvent.shapes.filter(shape => this.#isShape(shape)).forEach(shape => {
                            shapes.push(this.#getShapeResult(shape));
                        });

                        if (mouseEvent.shapes.length != shapes.length) {
                            console.warn(`${header} MouseEvent. Event shapes count [${mouseEvent.shapes.length}] and processed shapes count [${shapes.length}] does not match.`);
                        }

                        let result = {
                            mapId: mapId,
                            type: value,
                            payload: {
                                layerId: mouseEvent.layerId,
                                pixel: mouseEvent.pixel,
                                position: mouseEvent.position,
                                shapes: shapes
                            }
                        };

                        dotNetRef.invokeMethodAsync(MapEvents.EventNotifications.NotifyMapEventMouse, result);
                    } catch (e) {
                        console.error(`Error processing mouse event for ${header}:`, e);
                    }
                });
            });

            //MapEventSource
            Object.values(MapEvents.MapEventSource).filter(value => events.includes(value)).forEach((value) => {
                azmap.events.add(value as any, (source: atlas.source.Source) => {
                    let result = {
                        mapId: mapId,
                        type: value,
                        payload: {
                            id: source.getId()
                        }
                    };

                    dotNetRef.invokeMethodAsync(MapEvents.EventNotifications.NotifyMapEventSource, result);
                });
            });

            //MapEventStyle
            Object.values(MapEvents.MapEventStyle).filter(value => events.includes(value)).forEach((value) => {
                azmap.events.add(value as any, (style: string) => {
                    let result = {
                        mapId: mapId,
                        type: value,
                        payload: {
                            style: style
                        }
                    };
                    dotNetRef.invokeMethodAsync(MapEvents.EventNotifications.NotifyMapEventStyle, result );
                });
            });

            //MapEventStyleChanged
            Object.values(MapEvents.MapEventStyleChanged).filter(value => events.includes(value)).forEach((value) => {
                azmap.events.add(value as any, (styleEvent: atlas.StyleChangedEvent) => {
                    let result = {
                        mapId: mapId,
                        type: value,
                        payload: {
                            style: styleEvent.style
                        }
                    };
                    dotNetRef.invokeMethodAsync(MapEvents.EventNotifications.NotifyMapEventStyle, result);
                });
            });

            //MapEventTouch
            Object.values(MapEvents.MapEventTouch).filter(value => events.includes(value)).forEach((value) => {
                azmap.events.add(value as any, (touchEvent: atlas.MapTouchEvent) => {
                    const shapes: object[] = [];

                    touchEvent.shapes.forEach(shape => {
                        shapes.push(this.#getShapeResult(shape));
                    });

                    let result = {
                        mapId: mapId,
                        type: value,
                        payload: {
                            pixel: touchEvent.pixel,
                            pixels: touchEvent.pixels,
                            position: touchEvent.position,
                            positions: touchEvent.positions,
                            shapes: shapes,
                            layerId: touchEvent.layerId
                        }
                    }
                    dotNetRef.invokeMethodAsync(MapEvents.EventNotifications.NotifyMapEventTouch, result);
                });
            });

            //MapEventWheel
            Object.values(MapEvents.MapEventWheel).filter(value => events.includes(value)).forEach((value) => {
                azmap.events.add(value as any, (wheelEvent: atlas.MapMouseWheelEvent) => {
                    let result = {
                        mapId: mapId,
                        type: value,
                        payload: {
                            type: wheelEvent.type
                        }
                    }
                    dotNetRef.invokeMethodAsync(MapEvents.EventNotifications.NotifyMapEvent, result);
                });
            });

            if (controls) {
                MapInterop.Map.addControls(mapId, controls);
            }

            dotNetRef.invokeMethodAsync(MapEvents.EventNotifications.NotifyMapEventReady, { mapId: mapId, type: 'ready' });
        });
    }

    static #logHeader(mapId: string): string {
        return `Map with ID '${mapId}'`;
    }

    static #isFeature(obj: any): obj is atlas.data.Feature<atlas.data.Geometry, any> {
        return obj && obj.type === 'Feature';
    }

    static #isShape(obj: any): obj is atlas.Shape {
        return obj && obj.getType != undefined;
    }

    static #getFeatureResult(feature: atlas.data.Feature<atlas.data.Geometry, any>): object {

        const item: object = {
            id: feature.id?.toString(),
            type: feature.geometry.type,
            bbox: feature.bbox,
            source: "feature",
            properties: feature.properties
        };
        return item;
    }

    static #getShapeResult(shape: atlas.Shape): object {
        const item: object = {
            id: shape.getId()?.toString(),
            type: shape.getType(),
            bbox: shape.getBounds(),
            source: "shape",
            properties: shape.getProperties()
        };
        return item;
    }
}

