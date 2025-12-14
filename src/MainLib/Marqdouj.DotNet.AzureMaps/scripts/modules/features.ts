import * as atlas from "azure-maps-control";
import { Logger, LogLevel } from "./logger"
import { MapFactory } from "../core/factory"
import { JsInteropDef } from "./typings"
import { SourceManager } from "./source"

export class FeatureManager {
    public static addFeature(
        mapId: string,
        mapFeature: MapFeatureDef,
        datasourceId: string,
        replace?: boolean) {

        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        const ds = SourceManager.getDataSource(map, mapId, datasourceId);
        if (!ds)
            return;

        this.#doAddFeature(map, mapId, mapFeature, ds, replace);
    }

    public static addFeatures(
        mapId: string,
        mapFeatures: MapFeatureDef[],
        datasourceId: string,
        replace?: boolean) {

        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        const ds = SourceManager.getDataSource(map, mapId, datasourceId);
        if (!ds)
            return;

        mapFeatures.forEach(mapFeature => {
            this.#doAddFeature(map, mapId, mapFeature, ds, replace);
        });
    }


    static #doAddFeature(
        map: atlas.Map,
        mapId: string,
        mapFeature: MapFeatureDef,
        ds: atlas.source.DataSource,
        replace?: boolean) {

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
            Logger.logMessage(mapId, LogLevel.Error, `adding feature error: geometry type '${mapFeature.geometry.type}' not supported`);
            return;
        }

        const jsInterop: JsInteropDef = { id: mapFeature.id, interopId: mapFeature.interopId };
        const properties: TProperties = { ...mapFeature.properties, jsInterop: jsInterop };

        let feature = new atlas.data.Feature(geom, properties, mapFeature.id);

        if (mapFeature.asShape) {
            let shape = new atlas.Shape(feature);
            ds.add(shape);
        }
        else {
            ds.add(feature);
        }
    }

    public static updateFeature(
        mapId: string,
        mapFeature: MapFeatureDef,
        datasourceId: string) {

        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        const ds = SourceManager.getDataSource(map, mapId, datasourceId);

        if (!ds)
            return;

        const shape = ds.getShapeById(mapFeature.id);
        if (shape) {
            shape.setCoordinates(mapFeature.geometry.coordinates);
        }
    }

    public static updateFeatures(
        mapId: string,
        mapFeatures: MapFeatureDef[],
        datasourceId: string) {

        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        const ds = SourceManager.getDataSource(map, mapId, datasourceId);

        if (!ds)
            return;

        mapFeatures.forEach(feature => {
            const shape = ds.getShapeById(feature.id);
            if (shape) {
                shape.setCoordinates(feature.geometry.coordinates);
            }
        });
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

type TProperties = { [key: string]: any };

export interface MapFeatureDef extends JsInteropDef {
    geometry: any;
    bbox?: atlas.data.BoundingBox;
    properties?: TProperties;
    asShape: boolean;
}
