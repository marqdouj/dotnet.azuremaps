import * as atlas from "azure-maps-control";

export namespace Logger {
    export enum LogLevel {
        Trace = 0,
        Debug = 1,
        Information = 2,
        Warn = 3,
        Error = 4,
        Critical = 5,
        None = 6
    }

    export var currentLevel: LogLevel = LogLevel.Information;

    function GetMapHeader(mapId: string): string {
        return `Map with Id '${mapId}'`;
    }

    export function logMessage(mapId:string, level: LogLevel, message: string, ...optionalParams: any[]): void {
        if (level < currentLevel)
            return;

        const logOutput = `${GetMapHeader(mapId)} ${message}`;

        switch (level) {
            case LogLevel.Trace:
                console.trace(logOutput, ...optionalParams);
                break;
            case LogLevel.Debug:
                console.debug(logOutput, ...optionalParams);
                break;
            case LogLevel.Information:
                console.info(logOutput, ...optionalParams);
                break;
            case LogLevel.Warn:
                console.warn(logOutput, ...optionalParams);
                break;
            case LogLevel.Error:
                console.error(logOutput, ...optionalParams);
                break;
            case LogLevel.Critical:
                console.error(`CRITICAL: ${logOutput}`, ...optionalParams);
                break;

        }
    }
}

export enum GeoJSONType {
    Point = 'point',
    MultiPoint = 'multipoint',
    LineString = 'linestring',
    MultiLineString = 'multilinestring',
    Polygon = 'polygon',
    MultiPolygon = 'multipolygon'
}

export class Extensions {
    static isEmptyOrNull(str: string | null | undefined): boolean {
        return str?.trim() === "";
    }

    static isNotEmptyOrNull(str: string | null | undefined): boolean {
        return !this.isEmptyOrNull(str);
    }

    static isValueInEnum<T extends Record<string, string>>(enumObj: T, value: string): boolean {
        return Object.values(enumObj).includes(value as T[keyof T]);
    }

    static isFeature(obj: any): obj is atlas.data.Feature<atlas.data.Geometry, any> {
        return obj && obj.type === 'Feature';
    }

    static isShape(obj: any): obj is atlas.Shape {
        return obj && obj.getType != undefined;
    }

    static getFeatureResult(feature: atlas.data.Feature<atlas.data.Geometry, any>): object {

        const item: object = {
            id: feature.id?.toString(),
            type: feature.geometry.type,
            bbox: feature.bbox,
            source: "feature",
            properties: feature.properties
        };
        return item;
    }

    static getShapeResult(shape: atlas.Shape): object {
        const item: object = {
            id: shape.getId()?.toString(),
            type: shape.getType(),
            bbox: shape.getBounds(),
            source: "shape",
            properties: shape.getProperties()
        };
        return item;
    }

    static buildShapeResults(shapes: Array<atlas.data.Feature<atlas.data.Geometry, any> | atlas.Shape>): object[] {
        const results: object[] = [];

        shapes.filter(feature => this.isFeature(feature)).forEach(feature => {
            results.push(this.getFeatureResult(feature));
        });
        shapes.filter(shape => this.isShape(shape)).forEach(shape => {
            results.push(this.getShapeResult(shape));
        });

        return results;
    }
}
