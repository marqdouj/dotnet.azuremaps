import * as atlas from "azure-maps-control";
import { Helpers } from "../common/Helpers";
import { MapEventDef, MapEventResult } from "../typings";

export class EventsHelper {
    static buildEventResult(mapId: string, event: MapEventDef, payload?: any): MapEventResult {
        return { mapId: mapId, type: event.type, target: event.target, payload: { targetId: event.targetId, ...payload } };
    }
    static buildShapeResults(shapes: Array<atlas.data.Feature<atlas.data.Geometry, any> | atlas.Shape>): object[] {
        const results: object[] = [];

        shapes.filter(feature => Helpers.isFeature(feature)).forEach(feature => {
            results.push(Helpers.getFeatureResult(feature));
        });
        shapes.filter(shape => Helpers.isShape(shape)).forEach(shape => {
            results.push(Helpers.getShapeResult(shape));
        });

        return results;
    }

    static buildDataSourcePayload(source: atlas.source.DataSource) {
        return { id: source.getId(), jsInterop: Helpers.getJsInterop(source) };
    }

    static buildLayerPayload(layer: atlas.layer.Layer) {
        return { id: layer.getId(), jsInterop: Helpers.getJsInterop(layer) };
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
}
