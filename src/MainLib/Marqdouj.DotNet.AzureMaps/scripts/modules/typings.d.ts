import * as atlas from "azure-maps-control"
import { EventsMap } from "./events/EventsMap";

interface IMapReference {
    readonly dotNetRef: any;
    readonly eventsMap: EventsMap;
    readonly map: atlas.Map;
    readonly mapId: string;
}

interface JsInteropDef {
    id: string,
    interopId: string,
}

interface JsInteropControl extends JsInteropDef {
    type: string,
}

interface MapControlDef extends JsInteropDef {
    type: 'compass' | 'fullscreen' | 'pitch' | 'scale' | 'style' | 'traffic' | 'trafficLegend' | 'zoom';
    position: atlas.ControlPosition;
    options: atlas.CompassControlOptions
    | atlas.FullscreenControlOptions
    | atlas.PitchControlOptions
    | atlas.ScaleControlOptions
    | atlas.StyleControlOptions
    | atlas.ZoomControlOptions;
    controlOptions: atlas.ControlOptions;
}

type TEventTarget = 'map' | 'datasource' | 'htmlmarker' | 'layer' | 'shape' | 'stylecontrol' | 'popup';

interface MapEventDef {
    type: string;
    once?: boolean;
    target: TEventTarget;
    targetId?: string;
    targetSourceId?: string;
    preventDefault?: boolean;
}

interface MapEventResult {
    mapId: string;
    type: string;
    target: TEventTarget;
    payload?: object;
}