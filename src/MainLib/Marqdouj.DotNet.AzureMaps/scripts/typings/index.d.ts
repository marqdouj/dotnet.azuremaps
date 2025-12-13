import * as atlas from "azure-maps-control";
import { Logger } from "../common"

interface JsInteropObject {
    id: string,
    interopId: string,
}

interface JsInteropDef extends JsInteropObject {
    type: string,
    options: object;
    controlOptions: object;
}


interface HtmlMarkerDef extends JsInteropObject {
    options: atlas.HtmlMarkerOptions;
    togglePopupOnClick: boolean;
}

interface PopupDef extends JsInteropObject {
    options: atlas.PopupOptions;
}

interface MapLayerDef extends JsInteropObject {
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

interface DataSourceDef extends JsInteropObject {
    url: string;
    options?: atlas.DataSourceOptions;
}

interface MapControl extends JsInteropObject {
    type: 'compass' | 'fullscreen' | 'pitch' | 'scale' | 'style' | 'zoom';
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

interface MapFeature extends JsInteropObject {
    geometry: any;
    bbox?: atlas.data.BoundingBox;
    properties?: TProperties;
    asShape: boolean;
}