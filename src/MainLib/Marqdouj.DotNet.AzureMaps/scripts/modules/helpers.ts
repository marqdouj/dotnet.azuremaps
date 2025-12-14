import * as atlas from "azure-maps-control";

export class Helpers {
    static isEmptyOrNull(str: string | null | undefined): boolean {
        return str === null || str === undefined || str.trim() === "";
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

    static isJsInteropDef(obj: any): boolean {
        return obj && obj.jsInterop != undefined && obj.jsInterop.interopId != undefined;
    }
}