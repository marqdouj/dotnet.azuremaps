import * as atlas from "azure-maps-control";
import { MapFactory } from "./MapFactory"
import { Logger, LogLevel } from "./common/Logger";
import { Helpers } from "./common/Helpers";

export class ImageSprite {
    static async createFromTemplate(mapId: string, templateDef: ImageTemplateDef): Promise<boolean> {
        const mapRef = MapFactory.getMapReference(mapId);
        if (!mapRef)
            return false;

        if (Helpers.isEmptyOrNull(templateDef.id)) {
            Logger.logMessage(mapId, LogLevel.Error, "ImageTemplateDef.id is null or empty.", templateDef);
            return false;
        }

        if (Helpers.isEmptyOrNull(templateDef.templateName)) {
            Logger.logMessage(mapId, LogLevel.Error, "ImageTemplateDef.templateName is null or empty.", templateDef);
            return false;
        }

        const map = mapRef.map;

        if (map.imageSprite.hasImage(templateDef.id)) {
            Logger.logMessage(mapId, LogLevel.Warn, "Image template already exists.", templateDef);
            return false;
        }

        await map.imageSprite.createFromTemplate(templateDef.id, templateDef.templateName, templateDef.color, templateDef.secondaryColor);
        return true;
    }

    static hasImage(mapId: string, id: string): boolean {
        const mapRef = MapFactory.getMapReference(mapId);
        if (!mapRef)
            return false;

        return mapRef.map.imageSprite.hasImage(id);
    }

    static async add(mapId: string, id: string, icon: string | ImageData, meta?: atlas.StyleImageMetadata): Promise<boolean> {
        const mapRef = MapFactory.getMapReference(mapId);
        if (!mapRef)
            return false;

        await mapRef.map.imageSprite.add(id, icon, meta);
        return true;
    }

    static clear(mapId: string) {
        const mapRef = MapFactory.getMapReference(mapId);
        if (!mapRef)
            return false;

        mapRef.map.imageSprite.clear();
    }

    static getImageIds(mapId: string): string[] {
        const mapRef = MapFactory.getMapReference(mapId);
        if (!mapRef)
            return [];

        return mapRef.map.imageSprite.getImageIds();
    }

    static remove(mapId: string, id: string) {
        const mapRef = MapFactory.getMapReference(mapId);
        if (!mapRef)
            return false;

        mapRef.map.imageSprite.remove(id);
    }
}

interface ImageTemplateDef {
    id: string,
    templateName: string,
    color?: string,
    secondaryColor?: string,
    scale?: number
}
