import * as atlas from "azure-maps-control"
import { Helpers } from "./common/Helpers";
import { Logger, LogLevel } from "./common/Logger";
import { MapFactory } from "./MapFactory";
import { MapEventDef, JsInteropDef, IMapReference } from "./typings";
import { EventManager } from "./EventManager"
import { FeatureManager, MapFeatureDef } from "./FeatureManager";

export class SourceManager {
    // #region Add
    public static add(mapId: string, sources: SourceDef[], events?: MapEventDef[], features?: MapFeatureDef[]): void {
        const mapRef = MapFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        sources ??= [];

        sources.forEach((sourceDef) => {
            this.#doAdd(mapRef, sourceDef, events, features);
        });
    }

    static #doAdd(mapRef: IMapReference, def: SourceDef, events?: MapEventDef[], features?: MapFeatureDef[]): void {
        switch (def.type) {
            case 'DataSource':
                this.#doAddDataSource(mapRef, def as DataSourceDef, events, features);
                break;
            default:
                Logger.logMessage(mapRef.mapId, LogLevel.Warn, `add: unsupported source type: ${def.type}`);
                return;
        }
    }

    static #doAddDataSource(mapRef: IMapReference, def: DataSourceDef, events?: MapEventDef[], features?: MapFeatureDef[]): void {
        if (Helpers.isEmptyOrNull(def.id)) {
            Logger.logMessage(mapRef.mapId, LogLevel.Error, `addDataSource: missing ID.`, def);
            return;
        }

        let ds = mapRef.map.sources.getById(def.id);

        if (ds) {
            Logger.logMessage(mapRef.mapId, LogLevel.Warn, `addDataSource: source with ID '${def.id}' already exists.`);
            return;
        }

        const newDs = new atlas.source.DataSource(def.id, def.options);
        const jsInterop: JsInteropDef = { id: def.id, interopId: def.interopId };
        (newDs as any).jsInterop = jsInterop;

        if (events) {
            EventManager.sources.add(mapRef, events, newDs);
        }

        mapRef.map.sources.add(newDs);

        if (def.url) {
            newDs.importDataFromUrl(def.url);
        }

        if (features) {
            FeatureManager.add(mapRef.mapId, features, def.id);
        }
    }
    // #endregion

    // #region Remove
    public static remove(mapId: string, sources: SourceDef[]): void {
        const mapRef = MapFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        const idList: string[] = [];
        sources.forEach((sourceDef) => {
            idList.push(sourceDef.id);
        });

        this.#doRemoveById(mapRef, idList);
    }

    public static removeById(mapId: string, sources: string[]): void {
        if (sources.length == 0) return;

        const mapRef = MapFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        this.#doRemoveById(mapRef, sources);
    }

    static #doRemoveById(mapRef: IMapReference, sources: string[]): void {
        sources.forEach((id) => {
            const source = mapRef.map.sources.getById(id);
            if (source) {
                mapRef.map.sources.remove(source);
                Logger.logMessage(mapRef.mapId, LogLevel.Debug, `remove: source with ID '${id}' was removed.`);
            }
            else {
                Logger.logMessage(mapRef.mapId, LogLevel.Warn, `remove: source with ID '${id}' was not found.`);
            }
        });
    }
    // #endregion

    // #region Clear
    public static clear(mapId: string, sources: SourceDef[]): void {
        const mapRef = MapFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        const idList: string[] = [];
        sources.forEach((sourceDef) => {
            idList.push(sourceDef.id);
        });

        this.#doClearById(mapRef, idList);
    }

    public static clearById(mapId: string, sources: string[]): void {
        const mapRef = MapFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        this.#doClearById(mapRef, sources);
    }

    static #doClearById(mapRef: IMapReference, sources: string[]): void {
        sources.forEach((id) => {
            const ds = mapRef.map.sources.getById(id);
            if (ds) {
                if ((ds as any).clear != undefined) {
                    (ds as any).clear();
                    Logger.logMessage(mapRef.mapId, LogLevel.Debug, `clear: source with ID '${id}' was cleared.`);
                }
                else {
                    Logger.logMessage(mapRef.mapId, LogLevel.Warn, `clear: source with ID '${id}' does not support 'clear'.`);
                }
            }
            else {
                Logger.logMessage(mapRef.mapId, LogLevel.Warn, `clear: source with ID '${id}' was not found.`);
            }
        });
    }
    // #endregion

    public static getShapes(mapId: string, id: string) {
        const mapRef = MapFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        let shapes: object[] = [];

        const ds = SourceHelper.getSource(mapRef, id);
        if (ds) {
            if (ds instanceof atlas.source.DataSource) {
                Helpers.buildShapeResults(ds.getShapes());
            }
            else {
                Logger.logMessage(mapId, LogLevel.Warn, `getShapes: source with ID '${id}' does not support 'getShapes'.`);
            }
        }

        return shapes;
    }
}

export class SourceHelper {
    static getSource(mapRef: IMapReference, id: string): atlas.source.Source | undefined {
        const source = mapRef.map.sources.getById(id);

        if (!source) {
            Logger.logMessage(mapRef.mapId, LogLevel.Warn, `get: source with ID '${id}' was not found.`);
        }

        return source;
    }

    static isDataSource(obj: any): obj is atlas.source.DataSource {
        return obj && (obj instanceof atlas.source.DataSource);
    }
}

interface SourceDef extends JsInteropDef {
    type: 'DataSource' | 'ElevationTile' | 'VectorTile';
}

export interface DataSourceDef extends SourceDef {
    url: string;
    options?: atlas.DataSourceOptions;
}