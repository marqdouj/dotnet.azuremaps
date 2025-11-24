import * as atlas from "azure-maps-control";

type DataSourceDef = {
    id: string;
    url: string;
    options?: atlas.DataSourceOptions;
}

type TMapFeature = {
    id?: string | number;
    geometry: any;
    bbox?: atlas.data.BoundingBox;
    properties?: TProperties;
    asShape: boolean;
}
type TProperties = { [key: string]: any };
type TOptions = atlas.ServiceOptions & atlas.StyleOptions & atlas.UserInteractionOptions & (atlas.CameraOptions | atlas.CameraBoundsOptions);
type TControlOptions = atlas.CompassControlOptions
    & atlas.FullscreenControlOptions
    & atlas.PitchControlOptions
    & atlas.ScaleControlOptions
    & atlas.StyleControlOptions
    & atlas.ZoomControlOptions;

interface MapOptions {
    camera?: atlas.CameraOptions | atlas.CameraBoundsOptions,
    cameraBounds?: atlas.CameraBoundsOptions;
    service?: atlas.ServiceOptions;
    style?: atlas.StyleOptions;
    userInteraction?: atlas.UserInteractionOptions;
}

interface MapControl {
    type: 'compass' | 'fullscreen' | 'pitch' | 'scale' | 'style' | 'zoom';
    id: string;
    interopId: string;
    position: atlas.ControlPosition;
    options: TControlOptions;
    controlOptions: atlas.ControlOptions;
}

interface SetCameraOptions {
    animation: atlas.AnimationOptions;
    camera?: atlas.CameraOptions;
    cameraBounds?: (atlas.CameraBoundsOptions & { pitch?: number, bearing?: number });
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
