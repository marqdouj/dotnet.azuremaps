import * as atlas from "azure-maps-control";
import { MapFactory } from "../map-factory"
import { Logger } from "../common"
import { TMapFeature, TProperties } from "../typings"
import { SourceManager } from "./source"
import { GeoJSONType } from "../common"

export class FeatureManager {
    public static addFeature(
        mapId: string,
        mapFeature: TMapFeature,
        datasourceId: string,
        replace?: boolean) {

        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        const ds = SourceManager.getDataSource(map, mapId, datasourceId); 
        if (!ds)
            return;

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
            Logger.logMessage(mapId, Logger.LogLevel.Error, `- adding feature error: geometry type '${mapFeature.geometry.type}' not supported`);
            return;
        }

        const properties: TProperties = { ...mapFeature.properties, "interopId": mapFeature.interopId };

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
        mapFeature: TMapFeature,
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
}
