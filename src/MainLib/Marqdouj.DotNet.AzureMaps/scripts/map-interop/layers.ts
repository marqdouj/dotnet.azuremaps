import * as atlas from "azure-maps-control";
import { MapFactory } from "../map-factory"
import { Logger, Extensions } from "../common"
import { MapLayerDef, DataSourceDef, MapFeature } from "../typings"
import { Maps } from "./maps"
import { FeatureManager } from "./features"

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

    public static removeLayerAndDataSource(mapId: string, def: MapLayerDef, dsDef?: DataSourceDef) {

        this.removeLayer(mapId, def.id);
        Maps.removeDatasource(mapId, dsDef.id);
    }

    public static addMapFeature(
        mapId: string,
        mapFeature: MapFeature,
        datasourceId: string,
        replace?: boolean) {

        FeatureManager.addFeature(mapId, mapFeature, datasourceId, replace);
    }

    public static addMapFeatures(
        mapId: string,
        mapFeatures: MapFeature[],
        datasourceId: string,
        replace?: boolean) {

        FeatureManager.addFeatures(mapId, mapFeatures, datasourceId, replace);
    }

    public static updateMapFeature(
        mapId: string,
        mapFeature: MapFeature,
        datasourceId: string) {

        FeatureManager.updateFeature(mapId, mapFeature, datasourceId);
    }

    public static updateMapFeatures(
        mapId: string,
        mapFeatures: MapFeature[],
        datasourceId: string) {

        FeatureManager.updateFeatures(mapId, mapFeatures, datasourceId);
    }
}
