import * as atlas from "azure-maps-control"
import { Helpers } from "./common/Helpers";
import { Logger, LogLevel } from "./common/Logger";
import { MapFactory } from "./MapFactory";
import { DataSourceDef, SourceHelper, SourceManager } from "./SourceManager";
import { MapEventDef, JsInteropDef, IMapReference } from "./typings";
import { EventManager } from "./EventManager"
import { MapFeatureDef, FeatureManager } from "./FeatureManager";

export class LayerManager {
    public static add(mapId: string, layers: MapLayerDef[], events?: MapEventDef[]): void {
        const mapRef = MapFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        layers ??= [];

        layers.forEach((def) => {
            this.#doAddLayer(mapRef, def, events);
        });
    }

    static #doAddLayer(mapRef: IMapReference, def: MapLayerDef, events?: MapEventDef[]): void {
        const eventName = "addLayer";

        if (Helpers.isEmptyOrNull(def.type)) {
            Logger.logMessage(mapRef.mapId, LogLevel.Error, `${eventName}: layer type is missing`, def);
            return;
        }

        const layerId = def.id;

        if (Helpers.isEmptyOrNull(layerId)) {
            Logger.logMessage(mapRef.mapId, LogLevel.Error, `${eventName}: layer Id is missing`, def);
            return;
        }

        const lyr = mapRef.map.layers.getLayerById(layerId);
        if (lyr) {
            Logger.logMessage(mapRef.mapId, LogLevel.Error, `${eventName}: layer already exists where layer ID=${layerId}`, def);
            return;
        }

        let src: atlas.source.Source;
        let dsDef: DataSourceDef = def.dataSource;

        if (dsDef && Helpers.isNotEmptyOrNull(dsDef.id)) {
            src = SourceHelper.getSource(mapRef, dsDef.id);

            if (!src) {
                SourceManager.add(mapRef.mapId, [dsDef], events);
                src = SourceHelper.getSource(mapRef, dsDef.id);
            }
        }

        let layer: atlas.layer.Layer;
        const layerOptions = (def.options || {}) as any;

        switch (def.type) {
            case 'Bubble':
                layer = new atlas.layer.BubbleLayer(src, layerId, layerOptions);
                break;
            case 'HeatMap':
                layer = new atlas.layer.HeatMapLayer(src, layerId, layerOptions);
                break;
            case 'Image':
                layer = new atlas.layer.ImageLayer(layerOptions, layerId);
                break;
            case 'Line':
                layer = new atlas.layer.LineLayer(src, layerId, layerOptions);
                break;
            case 'Polygon':
                layer = new atlas.layer.PolygonLayer(src, layerId, layerOptions);
                break;
            case 'PolygonExtrusion':
                layer = new atlas.layer.PolygonExtrusionLayer(src, layerId, layerOptions);
                break;
            case 'Symbol':
                layer = new atlas.layer.SymbolLayer(src, layerId, layerOptions);
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
                EventManager.layers.add(mapRef, events, layer);
            }

            mapRef.map.layers.add(layer, def.before);
            wasAdded = true;
        }

        if (wasAdded) {
            Logger.logMessage(mapRef.mapId, LogLevel.Trace, `${eventName}: layer added:`, layer);
        } else {
            Logger.logMessage(mapRef.mapId, LogLevel.Error, `${eventName}: layer type '${def.type}' is not supported.`, def);
        }
    }

    public static remove(mapId: string, layers: MapLayerDef[]): void {
        let idList: string[] = [];

        layers.forEach((layerDef) => {
            idList.push(layerDef.id);
        });

        this.removeById(mapId, idList);

        idList = [];
        layers.forEach((layerDef) => {
            if (layerDef.dataSource && Helpers.isNotEmptyOrNull(layerDef.dataSource.id)) {
                idList.push(layerDef.dataSource.id);
            }
        });

        SourceManager.removeById(mapId, idList);
    }

    public static removeById(mapId: string, layerIds: string[]): void {
        const mapRef = MapFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        const eventName = "removeLayer";

        layerIds.forEach((id) => {
            const lyr = mapRef.map.layers.getLayerById(id);
            if (lyr) {
                mapRef.map.layers.remove(lyr);
                Logger.logMessage(mapId, LogLevel.Debug, `${eventName}: layer with id '${id}' was removed.`);
            }
            else {
                Logger.logMessage(mapId, LogLevel.Warn, `${eventName}: layer with id '${id}' was not found.`);
            }
        });
    }
}

interface MapLayerDef extends JsInteropDef {
    type: 'Bubble' | 'HeatMap' | 'Image' | 'Line' | 'Polygon' | 'PolygonExtrusion' | 'Symbol' | 'Tile';
    dataSource: DataSourceDef;
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