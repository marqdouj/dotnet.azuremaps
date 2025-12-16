import * as atlas from "azure-maps-control"

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
            jsInterop: this.getJsInterop(feature),
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
            jsInterop: this.getJsInterop(shape),
            id: shape.getId()?.toString(),
            type: shape.getType(),
            bbox: shape.getBounds(),
            source: "shape",
            properties: shape.getProperties()
        };
        return item;
    }

    static buildEventResult(mapId: string, event: MapEventDef, payload: any): MapEventResult {
        return { mapId: mapId, type: event.type, payload: { targetId: event.targetId, ...payload } };
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

    static buildMouseEventPayload(mouseEvent: atlas.MapMouseEvent) {
        const mouse = {
            layerId: mouseEvent.layerId,
            pixel: mouseEvent.pixel,
            position: mouseEvent.position,
            shapes: this.buildShapeResults(mouseEvent.shapes)
        };

        return { mouse: mouse };
    }

    static buildTargetedEventPayload(event: atlas.TargetedEvent) {
        return {
            jsInterop: (event.target as any).jsInterop,
            type: event.type
        };
    }

    static buildTouchEventPayload(touchEvent: atlas.MapTouchEvent) {
        return {
            pixel: touchEvent.pixel,
            pixels: touchEvent.pixels,
            position: touchEvent.position,
            positions: touchEvent.positions,
            layerId: touchEvent.layerId,
            shapes: this.buildShapeResults(touchEvent.shapes)
        };
    }

    static buildWheelEventPayload(wheelEvent: atlas.MapMouseWheelEvent) {
        return {
            type: wheelEvent.type,
        };
    }

    static isJsInteropDef(obj: any): boolean {
        return obj && obj.jsInterop != undefined && obj.jsInterop.interopId != undefined;
    }

    static isInteropControl(obj: any, id?: string): obj is atlas.Control {
        let ok = this.isJsInteropDef(obj);
        if (ok && Helpers.isNotEmptyOrNull(id)) {
            ok = obj.jsInterop.id === id || obj.jsInterop.interopId === id;
        }
        return ok;
    }

    static isInteropLayer(obj: any, id?: string): obj is atlas.layer.Layer {
        let ok = this.isJsInteropDef(obj);
        if (ok && Helpers.isNotEmptyOrNull(id)) {
            ok = obj.jsInterop.id === id || obj.jsInterop.interopId === id;
        }
        return ok;
    }

    static getJsInterop(obj: any): any {
        if (this.isJsInteropDef(obj)) {
            return obj.jsInterop;
        }
    }
}