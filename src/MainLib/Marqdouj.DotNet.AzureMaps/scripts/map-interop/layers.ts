import * as atlas from "azure-maps-control";
import { MapFactory } from "../map-factory"
import { Logger, Extensions } from "../common"
import { MapLayerDef, DataSourceDef, TMapFeature } from "../typings"

export class Layers {
    public static createLayer(mapId: string, def: MapLayerDef, dsDef?: DataSourceDef) {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        if (Extensions.isEmptyOrNull(def.type)) {
            Logger.logMessage(mapId, Logger.LogLevel.Error, `- CreateLayer: layer type is missing`);
            return;
        }

        const layerId = def.id;

        if (Extensions.isNotEmptyOrNull(layerId)) {
            const lyr = map.layers.getLayerById(layerId);
            if (lyr) {
                Logger.logMessage(mapId, Logger.LogLevel.Error, `- CreateLayer: layer already exists where ID=${layerId}`);
                return;
            }
        }

        let ds: atlas.source.DataSource;

        if (dsDef) {
            ds = new atlas.source.DataSource(dsDef.id, dsDef.options);
            if (!ds) {
                Logger.logMessage(mapId, Logger.LogLevel.Error, `- CreateLayer: Unable to create datasource.`, dsDef);
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
                Logger.logMessage(mapId, Logger.LogLevel.Error, `- CreateLayer: Unable to find datasource where ID=${def.sourceId}.`);
                return;
            }
        }

        const layerOptions = (def.options || {}) as any;
        layerOptions.jsInterop = {
            layerid: layerId,
        };

        switch (def.type) {
            case 'Bubble':
                map.layers.add(new atlas.layer.BubbleLayer(ds, layerId, layerOptions), def.before);
                break;
            case 'HeatMap':
                map.layers.add(new atlas.layer.HeatMapLayer(ds, layerId, layerOptions), def.before);
                break;
            case 'Image':
                map.layers.add(new atlas.layer.ImageLayer(layerOptions, layerId), def.before);
                break;
            case 'Line':
                map.layers.add(new atlas.layer.LineLayer(ds, layerId, layerOptions), def.before);
                break;
            case 'Polygon':
                map.layers.add(new atlas.layer.PolygonLayer(ds, layerId, layerOptions), def.before);
                break;
            case 'PolygonExtrusion':
                map.layers.add(new atlas.layer.PolygonExtrusionLayer(ds, layerId, layerOptions), def.before);
                break;
            case 'Symbol':
                map.layers.add(new atlas.layer.SymbolLayer(ds, layerId, layerOptions), def.before);
                break;
            case 'Tile':
                map.layers.add(new atlas.layer.TileLayer(layerOptions, layerId), def.before);
                break;
            default:
                Logger.logMessage(mapId, Logger.LogLevel.Warn, `- CreateLayer: layer type '${def.type}' is not supported.`);
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
            Logger.logMessage(mapId, Logger.LogLevel.Debug, `- CreateLayer:- layer with id '${id}' was removed.`);
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
            Logger.logMessage(mapId, Logger.LogLevel.Error, `- adding feature error: geometry type '${mapFeature.geometry.type}' not supported`);
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

enum GeoJSONType {
    Point = 'point',
    MultiPoint = 'multipoint',
    LineString = 'linestring',
    MultiLineString = 'multilinestring',
    Polygon = 'polygon',
    MultiPolygon = 'multipolygon'
}