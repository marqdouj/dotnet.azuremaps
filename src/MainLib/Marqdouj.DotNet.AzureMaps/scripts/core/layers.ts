import * as atlas from "azure-maps-control"
import { Logger, LogLevel } from "../modules/logger"
import { MapFactory } from "./factory"
import { DataSourceDef, JsInteropDef } from "../modules/typings"
import { Helpers } from "../modules/helpers"
import { FeatureManager, MapFeatureDef } from "../modules/features"
import { SourceManager } from "../modules/source"
import { Maps } from "./maps"

export class Layers {
    public static createLayer(mapId: string, def: MapLayerDef, dsDef?: DataSourceDef, events?: MapEventDef[]) {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        const eventName = "createLayer";

        if (Helpers.isEmptyOrNull(def.type)) {
            Logger.logMessage(mapId, LogLevel.Error, `${eventName}: layer type is missing`);
            return;
        }

        const layerId = def.id;

        if (Helpers.isNotEmptyOrNull(layerId)) {
            const lyr = map.layers.getLayerById(layerId);
            if (lyr) {
                Logger.logMessage(mapId, LogLevel.Error, `${eventName}: layer already exists where ID=${layerId}`);
                return;
            }
        }

        let ds: atlas.source.DataSource;

        if (dsDef) {
            SourceManager.createDatasource(mapId, dsDef, events);
            ds = SourceManager.getDataSource(map, mapId, dsDef.id);
            if (!ds) {
                Logger.logMessage(mapId, LogLevel.Error, `${eventName}: Unable to create datasource.`, dsDef);
                return;
            }
        }
        else {
            ds = SourceManager.getDataSource(map, mapId, dsDef.id);
            if (!ds) {
                Logger.logMessage(mapId, LogLevel.Error, `${eventName}: Unable to find datasource where ID=${def.sourceId}.`);
                return;
            }
        }

        let layer: atlas.layer.Layer;
        const layerOptions = (def.options || {}) as any;

        switch (def.type) {
            case 'Bubble':
                layer = new atlas.layer.BubbleLayer(ds, layerId, layerOptions);
                break;
            case 'HeatMap':
                layer = new atlas.layer.HeatMapLayer(ds, layerId, layerOptions);
                break;
            case 'Image':
                layer = new atlas.layer.ImageLayer(layerOptions, layerId);
                break;
            case 'Line':
                layer = new atlas.layer.LineLayer(ds, layerId, layerOptions);
                break;
            case 'Polygon':
                layer = new atlas.layer.PolygonLayer(ds, layerId, layerOptions);
                break;
            case 'PolygonExtrusion':
                layer = new atlas.layer.PolygonExtrusionLayer(ds, layerId, layerOptions);
                break;
            case 'Symbol':
                layer = new atlas.layer.SymbolLayer(ds, layerId, layerOptions);
                break;
            case 'Tile':
                layer = new atlas.layer.TileLayer(layerOptions, layerId);
                break;
            default:
               
                break;
        }

        let wasAdded = false;
        if (layer) {
            const jsInterop: JsInteropDef = { id: def.id, interopId: def.interopId };
            (layer as any).jsInterop = jsInterop;

            if (events) {
                const mapContainer = MapFactory.getMapContainer(mapId);
                mapContainer.events.addLayerEvents(events, layer);
            }

            map.layers.add(layer, def.before);
            wasAdded = true;
        }

        if (wasAdded) {
            Logger.logMessage(mapId, LogLevel.Trace, `${eventName}: layer added:`, layer);
        } else {
            Logger.logMessage(mapId, LogLevel.Error, `${eventName}: layer type '${def.type}' is not supported.`, def);
        }
    }

    public static removeLayer(mapId: string, id: string) {
        const map = MapFactory.getMap(mapId);
        if (!map) {
            return;
        }

        const eventName = "removeLayer";

        const lyr = map.layers.getLayerById(id);
        if (lyr) {
            map.layers.remove(lyr);
            Logger.logMessage(mapId, LogLevel.Debug, `${eventName}: layer with id '${id}' was removed.`);
        }
    }

    public static removeLayerAndDataSource(mapId: string, def: MapLayerDef, dsDef?: DataSourceDef) {

        this.removeLayer(mapId, def.id);
        SourceManager.removeDatasource(mapId, dsDef.id);
    }

    public static addMapFeature(
        mapId: string,
        mapFeature: MapFeatureDef,
        datasourceId: string,
        replace?: boolean) {

        FeatureManager.addFeature(mapId, mapFeature, datasourceId, replace);
    }

    public static addMapFeatures(
        mapId: string,
        mapFeatures: MapFeatureDef[],
        datasourceId: string,
        replace?: boolean) {

        FeatureManager.addFeatures(mapId, mapFeatures, datasourceId, replace);
    }

    public static updateMapFeature(
        mapId: string,
        mapFeature: MapFeatureDef,
        datasourceId: string) {

        FeatureManager.updateFeature(mapId, mapFeature, datasourceId);
    }

    public static updateMapFeatures(
        mapId: string,
        mapFeatures: MapFeatureDef[],
        datasourceId: string) {

        FeatureManager.updateFeatures(mapId, mapFeatures, datasourceId);
    }
}


interface MapLayerDef extends JsInteropDef {
    type: 'Bubble' | 'HeatMap' | 'Image' | 'Line' | 'Polygon' | 'PolygonExtrusion' | 'Symbol' | 'Tile';
    sourceId: string;
    before?: string;
    options?:
    atlas.BubbleLayerOptions |
    atlas.HeatMapLayerOptions |
    atlas.ImageLayerOptions |
    atlas.LineLayerOptions |
    atlas.PolygonLayerOptions |
    atlas.PolygonExtrusionLayerOptions |
    atlas.SymbolLayerOptions |
    atlas.TileLayerOptions |
    atlas.WebGLLayerOptions;
}
