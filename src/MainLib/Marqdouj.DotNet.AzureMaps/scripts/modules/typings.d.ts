import * as atlas from "azure-maps-control";
import { LogLevel } from "./logger"

interface JsInteropDef {
    id: string,
    interopId: string,
}

interface JsInteropControl extends JsInteropDef {
    type: string,
}

interface DataSourceDef extends JsInteropDef {
    url: string;
    options?: atlas.DataSourceOptions;
}

interface HtmlMarkerDef extends JsInteropDef {
    options: atlas.HtmlMarkerOptions;
    togglePopupOnClick: boolean;
}

interface PopupDef extends JsInteropDef {
    options: atlas.PopupOptions;
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
    logLevel?: LogLevel;
    inDevelopment?: boolean;
}

interface MapControlDef extends JsInteropDef {
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

