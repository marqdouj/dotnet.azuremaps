import * as atlas from "azure-maps-control"
import * as anims from "azure-maps-animations"
import { MapFactory } from "./MapFactory";
import { Logger, LogLevel } from "./common/Logger";
import { Helpers } from "./common/Helpers";
import { SourceHelper } from "./SourceManager"
import { FeatureManager, MapFeatureDef } from "./FeatureManager"

export class Animations {
    static getEasingNames(mapId: string): string[] {
        if (this.#animationsNotFound(mapId))
            return [];

        return anims.animations.getEasingNames();
    }

    static async animateShape(mapId: string, jsonOptions: string): Promise<void> {
        const options: AnimateShapeOptions = Helpers.parseJsonString(jsonOptions);

        switch (options.action) {
            case "SetCoordinates":
                await this.setCoordinates(mapId, options);
                break;
            default:
                Logger.logMessage(mapId, LogLevel.Error, "Animations.animateShape: action not supported.", options);
        }
    }

    static async setCoordinates(mapId: string, options: AnimateShapeOptions): Promise<void> {
        if (this.#animationsNotFound(mapId)) return;
        
        const map = MapFactory.getMap(mapId);
        if (!map) return;

        const eventName = "Animations.animateShape: ";

        const ds = map.sources.getById(options.dataSourceId);
        if (!ds) {
            Logger.logMessage(mapId, LogLevel.Error, `${eventName}DataSource not found.`, options);
            return;
        }

        if (SourceHelper.isDataSource(ds)) {
            let shape = ds.getShapeById(options.feature.id);
            if (!shape) {
                Logger.logMessage(mapId, LogLevel.Error, `${eventName}Shape not found where shapeId = '${options.feature.id}'.`, options);
                return;
            }

            anims.animations.setCoordinates(shape as any, options.feature.geometry.coordinates, options.animationOptions);
            shape.addProperty("heading", options.feature.properties["heading"]);
        } else {
            Logger.logMessage(mapId, LogLevel.Error, `${eventName}DataSource not found where id = '${options.dataSourceId}'.`, options);
        }
    }

    static #animationsNotFound(mapId: string): boolean {
        if (anims.animations) {
            return false;
        }
        else {
            Logger.logMessage(mapId, LogLevel.Trace, "Animations.setCoordinates: atlas.animations module not found.");
            return true;
        }
    }
}

type TAnimateAction = "SetCoordinates";

interface AnimateShapeOptions {
    action: TAnimateAction;
    feature: MapFeatureDef;
    dataSourceId: string;
    animationOptions: anims.PlayableAnimationOptions;
}