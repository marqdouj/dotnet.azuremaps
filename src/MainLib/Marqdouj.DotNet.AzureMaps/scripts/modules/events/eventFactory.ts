import * as atlas from "azure-maps-control";
import { EventFactoryBase } from "./EventFactoryBase"
import { MapEventFactory } from "./map-events"
import { MarkerEventFactory } from "./marker-events"
import { SourceEventFactory } from "./source-events"
import { StyleControlEventFactory } from "./stylecontrol-events"
import { LayerEventFactory } from "./layer-events"
import { ShapeEventFactory } from "./shape-events"
import { PopupEventFactory } from "./popup-events"

export class EventFactory extends EventFactoryBase {
    mapEvents: MapEventFactory;
    markerEvents: MarkerEventFactory;
    sourceEvents: SourceEventFactory;
    styleControlEvents: StyleControlEventFactory;
    layerEvents: LayerEventFactory;
    shapeEvents: ShapeEventFactory;
    popupEvents: PopupEventFactory;

    constructor(mapId: string) {
        super(mapId);
        this.mapEvents = new MapEventFactory(mapId);
        this.markerEvents = new MarkerEventFactory(mapId);
        this.sourceEvents = new SourceEventFactory(mapId);
        this.styleControlEvents = new StyleControlEventFactory(mapId);
        this.layerEvents = new LayerEventFactory(mapId);
        this.shapeEvents = new ShapeEventFactory(mapId);
        this.popupEvents = new PopupEventFactory(mapId);
    }

    addEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        this.mapEvents.addEvents(events);
        this.markerEvents.addEvents(events);
        this.sourceEvents.addEvents(events);
        this.styleControlEvents.addEvents(events);
        this.layerEvents.addEvents(events);
        this.shapeEvents.addEvents(events);
        this.popupEvents.addEvents(events);
    }

    addLayerEvents(events: MapEventDef[], layer?: atlas.layer.Layer) {
        if (events.length == 0) return;
        this.layerEvents.addEvents(events, layer);
    }

    removeEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        this.mapEvents.removeEvents(events);
        this.markerEvents.removeEvents(events);
        this.sourceEvents.removeEvents(events);
        this.styleControlEvents.removeEvents(events);
        this.layerEvents.removeEvents(events);
        this.shapeEvents.removeEvents(events);
        this.popupEvents.removeEvents(events);
    }

    removeLayerEvents(events: MapEventDef[], layer?: atlas.layer.Layer) {
        if (events.length == 0) return;
        this.layerEvents.removeEvents(events, layer);
    }
}
