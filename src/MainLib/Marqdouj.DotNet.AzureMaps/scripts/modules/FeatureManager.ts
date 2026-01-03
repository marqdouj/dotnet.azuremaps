import * as atlas from "azure-maps-control"
import { MapFactory } from "./MapFactory"
import { SourceHelper } from "./SourceManager";
import { Logger, LogLevel } from "./common/Logger";
import { IMapReference, JsInteropDef } from "./typings";

export class FeatureManager {
    public static add(
        mapId: string,
        mapFeatures: MapFeatureDef[],
        sourceId: string,
        replace?: boolean) {

        const mapRef = MapFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        const ds = SourceHelper.getSource(mapRef, sourceId);
        if (!ds)
            return;

        mapFeatures ??= [];

        mapFeatures.forEach(mapFeature => {
            this.#doAddFeature(mapRef, mapFeature, ds, replace);
        });
    }

    static #doAddFeature(
        mapRef: IMapReference,
        mapFeature: MapFeatureDef,
        ds: atlas.source.Source,
        replace?: boolean) {

        if (ds instanceof atlas.source.DataSource === false) {
            Logger.logMessage(mapRef.mapId, LogLevel.Error, `adding feature: source with ID '${ds.getId()}' is not a DataSource`);
            return;
        }

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
            Logger.logMessage(mapRef.mapId, LogLevel.Error, `adding feature error: geometry type '${mapFeature.geometry.type}' not supported`);
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

    public static update(
        mapId: string,
        mapFeatures: MapFeatureDef[],
        datasourceId: string) {

        const mapRef = MapFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        const ds = SourceHelper.getSource(mapRef, datasourceId);

        if (ds instanceof atlas.source.DataSource === false) {
            Logger.logMessage(mapRef.mapId, LogLevel.Error, `updating features: source with ID '${ds.getId()}' is not a DataSource`);
            return;
        }

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
