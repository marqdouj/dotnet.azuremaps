import * as atlas from "azure-maps-control";
import { Logger } from "../common"

interface JsInteropDef {
    id: string,
    interopId: string,
    type: string,
    options: object;
    controlOptions: object;
}

interface MapLayerDef {
    type: 'Bubble' | 'HeatMap' | 'Image' | 'Line' | 'Polygon' | 'PolygonExtrusion' | 'Symbol' | 'Tile';
    id: string;
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

type DataSourceDef = {
    id: string;
    url: string;
    options?: atlas.DataSourceOptions;
}

interface MapControl {
    type: 'compass' | 'fullscreen' | 'pitch' | 'scale' | 'style' | 'zoom';
    id: string;
    interopId: string;
    position: atlas.ControlPosition;
    options: atlas.CompassControlOptions
    | atlas.FullscreenControlOptions
    | atlas.PitchControlOptions
    | atlas.ScaleControlOptions
    | atlas.StyleControlOptions
    | atlas.ZoomControlOptions;
    controlOptions: atlas.ControlOptions;
}

interface MapOptions {
    camera?: atlas.CameraOptions;
    cameraBounds?: atlas.CameraBoundsOptions;
    service?: atlas.ServiceOptions;
    style?: atlas.StyleOptions;
    userInteraction?: atlas.UserInteractionOptions;
}

interface MapSettings {
    authOptions: atlas.AuthenticationOptions;
    options?: MapOptions;
    logLevel?: Logger.LogLevel;
    inDevelopment?: boolean;
}

type TMapOptions = atlas.ServiceOptions & atlas.StyleOptions & atlas.UserInteractionOptions & (atlas.CameraOptions | atlas.CameraBoundsOptions);
type TCameraOptionsSet = (atlas.CameraOptions | (atlas.CameraBoundsOptions & { pitch?: number, bearing?: number })) & atlas.AnimationOptions;

interface CameraOptionsSet {
    camera?: atlas.CameraOptions;
    cameraBounds?: atlas.CameraBoundsOptions & { pitch?: number, bearing?: number };
    animation?: atlas.AnimationOptions;
}

interface MapOptionsSet {
    camera?: CameraOptionsSet;
    service?: atlas.ServiceOptions;
    style?: atlas.StyleOptions;
    userInteraction?: atlas.UserInteractionOptions;
}

type TProperties = { [key: string]: any };

type TMapFeature = {
    id?: string;
    interopId?: string,
    geometry: any;
    bbox?: atlas.data.BoundingBox;
    properties?: TProperties;
    asShape: boolean;
}