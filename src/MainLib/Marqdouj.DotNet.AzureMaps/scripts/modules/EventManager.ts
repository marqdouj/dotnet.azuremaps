import { MapEventDef } from "./typings";
import { LayerEvents } from "./events/LayerEvents"
import { MapFactory } from "./MapFactory"
import { SourceEvents } from "./events/SourceEvents";
import { MapEvents } from "./events/MapEvents";
import { MarkerEvents } from "./events/MarkerEvents";
import { PopupEvents } from "./events/PopupEvents";
import { ShapeEvents } from "./events/ShapeEvents";
import { StyleControlEvents } from "./events/StyleControlEvents";

export class EventManager  {
    static readonly layers: LayerEvents = new LayerEvents()
    static readonly maps: MapEvents = new MapEvents();
    static readonly markers: MarkerEvents = new MarkerEvents();
    static readonly popups: PopupEvents = new PopupEvents();
    static readonly shapes: ShapeEvents = new ShapeEvents();
    static readonly sources: SourceEvents = new SourceEvents();
    static readonly styleControls: StyleControlEvents = new StyleControlEvents();

    public static add(mapId: string, events: MapEventDef[]): void {
        const mapRef = MapFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        events ??= [];

        this.layers.add(mapRef, events);
        this.maps.add(mapRef, events);
        this.markers.add(mapRef, events);
        this.popups.add(mapRef, events);
        this.shapes.add(mapRef, events);
        this.sources.add(mapRef, events);
        this.styleControls.add(mapRef, events);
    }

    public static remove(mapId: string, events: MapEventDef[]): void {
        const mapRef = MapFactory.getMapReference(mapId);
        if (!mapRef)
            return;

        this.layers.remove(mapRef, events);
        this.maps.remove(mapRef, events);
        this.markers.remove(mapRef, events);
        this.popups.remove(mapRef, events);
        this.shapes.remove(mapRef, events);
        this.sources.remove(mapRef, events);
        this.styleControls.remove(mapRef, events);
    }
}

export enum EventNotification {
    MapError = 'NotifyMapEventError',
    MapReady = 'NotifyMapReady',

    NotifyMapEvent = 'NotifyMapEvent',
    NotifyMapEventConfig = 'NotifyMapEventConfig',
    NotifyMapEventData = 'NotifyMapEventData',
    NotifyMapEventDataSource = 'NotifyMapEventDataSource',
    NotifyMapEventGeneral = 'NotifyMapEventGeneral',
    NotifyMapEventHtmlMarker = 'NotifyMapEventHtmlMarker',
    NotifyMapEventLayer = 'NotifyMapEventLayer',
    NotifyMapEventMouse = 'NotifyMapEventMouse',
    NotifyMapEventPopup = "NotifyMapEventPopup",
    NotifyMapEventShape = 'NotifyMapEventShape',
    NotifyMapEventSource = 'NotifyMapEventSource',
    NotifyMapEventStyle = 'NotifyMapEventStyle',
    NotifyMapEventTouch = 'NotifyMapEventTouch',
    NotifyMapEventWheel = 'NotifyMapEventWheel',
}

export enum MapEventMouse {
    Click = 'click',
    ContextMenu = 'contextmenu',
    DblClick = 'dblclick',
    MouseDown = 'mousedown',
    MouseMove = 'mousemove',
    MouseOut = 'mouseout',
    MouseOver = 'mouseover',
    MouseUp = 'mouseup',
}

export enum MapEventTouch {
    TouchCancel = 'touchcancel',
    TouchEnd = 'touchend',
    TouchMove = 'touchmove',
    TouchStart = 'touchstart'
}

export enum MapEventWheel {
    Wheel = 'wheel',
}