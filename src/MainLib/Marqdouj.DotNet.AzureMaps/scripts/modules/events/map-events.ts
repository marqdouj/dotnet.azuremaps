import * as atlas from "azure-maps-control"
import { Helpers } from "../helpers"
import { EventNotifications, MapEventLogger } from "./common"
import { EventFactoryBase, MapEventMouse, MapEventTouch, MapEventWheel } from "./common"

export class MapEventFactory extends EventFactoryBase {
    constructor(mapId: string) {
        super(mapId);
    }
   
    addEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#addMapEvents(Object.values(events).filter(value => value.target === "map"));
    }

    removeEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#removeMapEvents(Object.values(events).filter(value => value.target === "map"));
    }

    #addMapEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#addMapEventConfig(events);
        this.#addMapEventData(events);
        this.#addMapEventGeneral(events);
        this.#addMapEventMouse(events);
        this.#addMapEventSource(events);
        this.#addMapEventStyle(events);
        this.#addMapEventTouch(events);
        this.#addMapEventWheel(events);
    }

    #removeMapEvents(events: MapEventDef[]) {
        if (events.length == 0) return;

        this.#removeMapEventConfig(events);
        this.#removeMapEventData(events);
        this.#removeMapEventGeneral(events);
        this.#removeMapEventMouse(events);
        this.#removeMapEventSource(events);
        this.#removeMapEventStyle(events);
        this.#removeMapEventTouch(events);
        this.#removeMapEventWheel(events);
    }

    // #region Config
    #addMapEventConfig(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "addMapEventConfig";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventConfig, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            let callback: any;

            switch (value.type.toLowerCase()) {
                case MapEventConfig.MapConfigChanged:
                    callback = this.#notifyMapEventConfig_MapConfigChanged;
                    break;
                default:
            }

            if (callback) {
                if (value.once) {
                    MapEventLogger.logOnceNotSupported(this.mapId, eventName, value);
                }
                else {
                    azmap.events.add(value.type as MapEventConfig, callback);
                    wasAdded = true;
                }
            }
            MapEventLogger.logEventAdd(this.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventConfig(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "removeMapEventConfig";

        events.forEach((value) => {
            let wasRemoved: boolean = false;
            let callback: any;

            switch (value.type.toLowerCase()) {
                case MapEventConfig.MapConfigChanged:
                    callback = this.#notifyMapEventConfig_MapConfigChanged;
                    break;
                default:
            }

            if (callback) {
                azmap.events.remove(value.type, callback);
                wasRemoved = true;
            }
            MapEventLogger.logEventRemoved(this.mapId, eventName, wasRemoved, value);
        });
    }

    #notifyNotifyMapEventConfig = (config: atlas.MapConfiguration, event: MapEventConfig) => {
        let result = Helpers.buildEventResult(this.mapId, event, config);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventConfig, result);
    };

    #notifyMapEventConfig_MapConfigChanged = (config: atlas.MapConfiguration) => this.#notifyNotifyMapEventConfig(config, MapEventConfig.MapConfigChanged);
    // #endregion

    // #region Data
    #addMapEventData(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "addMapEventData";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventData, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            let callback: any;

            switch (value.type.toLowerCase()) {
                case MapEventData.Data:
                    callback = this.#notifyMapEventData_Data;
                    break;
                case MapEventData.SourceData:
                    callback = this.#notifyMapEventData_SourceData;
                    break;
                case MapEventData.StyleData:
                    callback = this.#notifyMapEventData_StyleData;
                    break;
                default:
            }

            if (callback) {
                if (value.once) {
                    azmap.events.addOnce(value.type as MapEventData, callback);
                }
                else {
                    azmap.events.add(value.type as MapEventData, callback);
                }
                wasAdded = true;
            }

            MapEventLogger.logEventAdd(this.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventData(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "removeMapEventData";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventData, value.type)).forEach((value) => {
            let wasRemoved: boolean = false;
            let callback: any;

            switch (value.type.toLowerCase()) {
                case MapEventData.Data:
                    callback = this.#notifyMapEventData_Data;
                    break;
                case MapEventData.SourceData:
                    callback = this.#notifyMapEventData_SourceData;
                    break;
                case MapEventData.StyleData:
                    callback = this.#notifyMapEventData_StyleData;
                    break;
                default:
            }

            if (callback) {
                azmap.events.remove(value.type, callback);
                wasRemoved = true;
            }

            MapEventLogger.logEventRemoved(this.mapId, eventName, wasRemoved, value);
        });
    }

    #notifyMapEventData = (dataEvent: atlas.MapDataEvent, event: MapEventData) => {
        let result = Helpers.buildEventResult(this.mapId, event, this.#buildMapDataEventPayload(dataEvent));
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventData, result);
    };

    #notifyMapEventData_Data = (dataEvent: atlas.MapDataEvent) => this.#notifyMapEventData(dataEvent, MapEventData.Data);

    #notifyMapEventData_SourceData = (dataEvent: atlas.MapDataEvent) => this.#notifyMapEventData(dataEvent, MapEventData.SourceData);

    #notifyMapEventData_StyleData = (dataEvent: atlas.MapDataEvent) => this.#notifyMapEventData(dataEvent, MapEventData.StyleData);

    #buildMapDataEventPayload(dataEvent: atlas.MapDataEvent) {
        return {
            dataType: dataEvent.dataType,
            isSourceLoaded: dataEvent.isSourceLoaded,
            source: dataEvent.source?.getId(),
            sourceDataType: dataEvent.sourceDataType,
            tile: dataEvent.tile
        };
    }
    // #endregion

    // #region General
    #addMapEventGeneral(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "addMapEventGeneral";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventGeneral, value.type)).forEach((value) => {
            if (value.once) {
                azmap.events.addOnce(value.type as MapEventGeneral, () => {
                    this.#notifyMapEventGeneral(value.type as MapEventGeneral);
                });
            }
            else {
                azmap.events.add(value.type as MapEventGeneral, () => {
                    this.#notifyMapEventGeneral(value.type as MapEventGeneral);
                });
            }

            MapEventLogger.logEventAdd(this.mapId, eventName, true, value);
        });
    }

    #removeMapEventGeneral(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "removeMapEventGeneral";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventGeneral, value.type)).forEach((value) => {
            azmap.events.remove(value.type, () => { });
            MapEventLogger.logEventRemoved(this.mapId, eventName, true, value);
        });
    }

    #notifyMapEventGeneral = (event: MapEventGeneral) => {
        let result = Helpers.buildEventResult(this.mapId, event, null);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEvent, result);
    };
    // #endregion

    // #region Mouse
    #addMapEventMouse(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "addMapEventMouse";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventMouse, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            let callback = this.#getCallBack(value);

            if (callback) {
                if (value.once) {
                    azmap.events.addOnce(value.type as MapEventMouse, callback);
                }
                else {
                    azmap.events.add(value.type as MapEventMouse, callback);
                }
                wasAdded = true;
            }

            MapEventLogger.logEventAdd(this.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventMouse(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "removeMapEventMouse";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventMouse, value.type)).forEach((value) => {
            let wasRemoved: boolean = false;
            let callback = this.#getCallBack(value);

            if (callback) {
                azmap.events.remove(value.type, callback);
                wasRemoved = true;
            }

            MapEventLogger.logEventRemoved(this.mapId, eventName, wasRemoved, value);
        });
    }

    #getCallBack(value: MapEventDef) {
        let callback: any;

        switch (value.type as MapEventMouse) {
            case MapEventMouse.Click:
                callback = this.#notifyMapEventMouse_Click;
                break;
            case MapEventMouse.ContextMenu:
                callback = this.#notifyMapEventMouse_ContextMenu;
                break;
            case MapEventMouse.DblClick:
                callback = this.#notifyMapEventMouse_DblClick;
                break;
            case MapEventMouse.MouseDown:
                callback = this.#notifyMapEventMouse_MouseDown;
                break;
            case MapEventMouse.MouseMove:
                callback = this.#notifyMapEventMouse_MouseMove;
                break;
            case MapEventMouse.MouseOut:
                callback = this.#notifyMapEventMouse_MouseOut;
                break;
            case MapEventMouse.MouseOver:
                callback = this.#notifyMapEventMouse_MouseOver;
                break;
            case MapEventMouse.MouseUp:
                callback = this.#notifyMapEventMouse_MouseUp;
                break;
            default:
        }

        return callback;
    }

    #notifyMapEventMouse = (callback: atlas.MapMouseEvent, event: MapEventMouse) => {
        let payload = Helpers.buildMouseEventPayload(callback);
        let result = Helpers.buildEventResult(this.mapId, event, payload);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventMouse, result);
    };

    #notifyMapEventMouse_Click = (callback: atlas.MapMouseEvent) => this.#notifyMapEventMouse(callback, MapEventMouse.Click);
    #notifyMapEventMouse_ContextMenu = (callback: atlas.MapMouseEvent) => this.#notifyMapEventMouse(callback, MapEventMouse.ContextMenu);
    #notifyMapEventMouse_DblClick = (callback: atlas.MapMouseEvent) => this.#notifyMapEventMouse(callback, MapEventMouse.DblClick);
    #notifyMapEventMouse_MouseDown = (callback: atlas.MapMouseEvent) => this.#notifyMapEventMouse(callback, MapEventMouse.MouseDown);
    #notifyMapEventMouse_MouseMove = (callback: atlas.MapMouseEvent) => this.#notifyMapEventMouse(callback, MapEventMouse.MouseMove);
    #notifyMapEventMouse_MouseOut = (callback: atlas.MapMouseEvent) => this.#notifyMapEventMouse(callback, MapEventMouse.MouseOut);
    #notifyMapEventMouse_MouseOver = (callback: atlas.MapMouseEvent) => this.#notifyMapEventMouse(callback, MapEventMouse.MouseOver);
    #notifyMapEventMouse_MouseUp = (callback: atlas.MapMouseEvent) => this.#notifyMapEventMouse(callback, MapEventMouse.MouseUp);

    // #endregion

    // #region Source
    #addMapEventSource(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "addMapEventSource";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventSource, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            let callback: any;

            switch (value.type.toLowerCase()) {
                case MapEventSource.SourceAdded:
                    callback = this.#notifyMapEventSource_SourceAdded;
                    break;
                case MapEventSource.SourceRemoved:
                    callback = this.#notifyMapEventSource_SourceRemoved;
                    break;
                default:
            }

            if (callback) {
                if (value.once) {
                    azmap.events.addOnce(value.type as MapEventSource, callback);
                }
                else {
                    azmap.events.add(value.type as MapEventSource, callback);
                }
                wasAdded = true;
            }

            MapEventLogger.logEventAdd(this.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventSource(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "removeMapEventSource";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventSource, value.type)).forEach((value) => {
            let wasRemoved: boolean = false;
            let callback: any;

            switch (value.type.toLowerCase()) {
                case MapEventSource.SourceAdded:
                    callback = this.#notifyMapEventSource_SourceAdded;
                    break;
                case MapEventSource.SourceRemoved:
                    callback = this.#notifyMapEventSource_SourceRemoved;
                    break;
                default:
            }

            if (callback) {
                azmap.events.remove(value.type, callback);
                wasRemoved = true;
            }

            MapEventLogger.logEventRemoved(this.mapId, eventName, wasRemoved, value);
        });
    }

    #notifyMapEventSource(source: atlas.source.Source, event: MapEventSource) {
        let payload = { id: source.getId(), jsInterop: Helpers.getJsInterop(source) };
        let result = Helpers.buildEventResult(this.mapId, event, payload);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventSource, result);
    }

    #notifyMapEventSource_SourceAdded = (source: atlas.source.Source) => this.#notifyMapEventSource(source, MapEventSource.SourceAdded);
    #notifyMapEventSource_SourceRemoved = (source: atlas.source.Source) => this.#notifyMapEventSource(source, MapEventSource.SourceRemoved);

    // #endregion

    // #region Style
    #addMapEventStyle(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "addMapEventStyle";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventStyle, value.type)).forEach((value) => {
            let wasAdded: boolean = true;

            switch (value.type.toLowerCase()) {
                case MapEventStyle.StyleChanged:
                    if (value.once) {
                        azmap.events.addOnce(MapEventStyle.StyleChanged, this.#notifyMapEventStyle_StyleChanged);
                    }
                    else {
                        azmap.events.add(MapEventStyle.StyleChanged, this.#notifyMapEventStyle_StyleChanged);
                    }
                    break;
                case MapEventStyle.StyleImageMissing:
                    if (value.once) {
                        azmap.events.addOnce(MapEventStyle.StyleImageMissing, this.#notifyMapEventStyle_StyleImageMissing);
                    }
                    else {
                        azmap.events.add(MapEventStyle.StyleImageMissing, this.#notifyMapEventStyle_StyleImageMissing);
                    }
                    break;
                default:
                    wasAdded = false;
            }

            MapEventLogger.logEventAdd(this.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventStyle(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "removeMapEventStyle";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventStyle, value.type)).forEach((value) => {
            let wasRemoved: boolean = true;

            switch (value.type.toLowerCase()) {
                case MapEventStyle.StyleChanged:
                    azmap.events.remove(value.type, this.#notifyMapEventStyle_StyleChanged);
                    break;
                case MapEventStyle.StyleImageMissing:
                    azmap.events.remove(value.type, this.#notifyMapEventStyle_StyleImageMissing);
                    break;
                default:
                    wasRemoved = false;
            }

            MapEventLogger.logEventRemoved(this.mapId, eventName, wasRemoved, value);
        });
    }

    #notifyMapEventStyle(style: string, event: MapEventStyle) {
        let payload = { style: style };
        let result = Helpers.buildEventResult(this.mapId, event, payload);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventStyle, result);
    }

    #notifyMapEventStyle_StyleChanged = (source: atlas.StyleChangedEvent) => this.#notifyMapEventStyle(source.style, MapEventStyle.StyleChanged);
    #notifyMapEventStyle_StyleImageMissing = (style: string) => this.#notifyMapEventStyle(style, MapEventStyle.StyleImageMissing);
    // #endregion

    // #region Touch
    #addMapEventTouch(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "addMapEventTouch";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventTouch, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            let callback: any;

            switch (value.type.toLowerCase()) {
                case MapEventTouch.TouchCancel:
                    callback = this.#notifyMapEventTouch_TouchCancel;
                    break;
                case MapEventTouch.TouchEnd:
                    callback = this.#notifyMapEventTouch_TouchEnd;
                    break;
                case MapEventTouch.TouchMove:
                    callback = this.#notifyMapEventTouch_TouchMove;
                    break;
                case MapEventTouch.TouchStart:
                    callback = this.#notifyMapEventTouch_TouchStart;
                    break;
                default:
            }

            if (callback) {
                if (value.once) {
                    azmap.events.addOnce(value.type as MapEventTouch, callback);
                }
                else {
                    azmap.events.add(value.type as MapEventTouch, callback);
                }
                wasAdded = true;
            }

            MapEventLogger.logEventAdd(this.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventTouch(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "removeMapEventTouch";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventTouch, value.type)).forEach((value) => {
            let wasRemoved: boolean = false;
            let callback: any;

            switch (value.type.toLowerCase()) {
                case MapEventTouch.TouchCancel:
                    callback = this.#notifyMapEventTouch_TouchCancel;
                    break;
                case MapEventTouch.TouchEnd:
                    callback = this.#notifyMapEventTouch_TouchEnd;
                    break;
                case MapEventTouch.TouchMove:
                    callback = this.#notifyMapEventTouch_TouchMove;
                    break;
                case MapEventTouch.TouchStart:
                    callback = this.#notifyMapEventTouch_TouchStart;
                    break;
                default:
            }

            if (callback) {
                azmap.events.remove(value.type, callback);
                wasRemoved = true;
            }

            MapEventLogger.logEventRemoved(this.mapId, eventName, wasRemoved, value);
        });
    }

    #notifyMapEventTouch = (callback: atlas.MapTouchEvent, type: MapEventTouch) => {
        let payload = Helpers.buildTouchEventPayload(callback);
        let result = Helpers.buildEventResult(this.mapId, type, payload);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventTouch, result);
    };

    #notifyMapEventTouch_TouchCancel = (callback: atlas.MapTouchEvent) => this.#notifyMapEventTouch(callback, MapEventTouch.TouchCancel);

    #notifyMapEventTouch_TouchEnd = (callback: atlas.MapTouchEvent) => this.#notifyMapEventTouch(callback, MapEventTouch.TouchEnd);

    #notifyMapEventTouch_TouchMove = (callback: atlas.MapTouchEvent) => this.#notifyMapEventTouch(callback, MapEventTouch.TouchMove);

    #notifyMapEventTouch_TouchStart = (callback: atlas.MapTouchEvent) => this.#notifyMapEventTouch(callback, MapEventTouch.TouchStart);
    // #endregion

    // #region Wheel
    #addMapEventWheel(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "addMapEventWheel";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventWheel, value.type)).forEach((value) => {
            let wasAdded: boolean = false;
            let callback: any;

            switch (value.type.toLowerCase()) {
                case MapEventWheel.Wheel:
                    callback = this.#notifyMapEventWheel_Wheel;
                    break;
                default:
            }

            if (callback) {
                if (value.once) {
                    azmap.events.addOnce(value.type as MapEventWheel, callback);
                }
                else {
                    azmap.events.add(value.type as MapEventWheel, callback);
                }
                wasAdded = true;
            }

            MapEventLogger.logEventAdd(this.mapId, eventName, wasAdded, value);
        });
    }

    #removeMapEventWheel(events: MapEventDef[]) {
        if (events.length == 0) return;

        const azmap = this.getMap();
        const eventName = "removeMapEventWheel";

        Object.values(events).filter(value => Helpers.isValueInEnum(MapEventWheel, value.type)).forEach((value) => {
            let wasRemoved: boolean = false;
            let callback: any;

            switch (value.type.toLowerCase()) {
                case MapEventWheel.Wheel:
                    callback = this.#notifyMapEventWheel_Wheel;
                    break;
                default:
            }

            if (callback) {
                azmap.events.remove(value.type, callback);
                wasRemoved = true;
            }

            MapEventLogger.logEventRemoved(this.mapId, eventName, wasRemoved, value);
        });
    }

    #notifyMapEventWheel = (callback: atlas.MapMouseWheelEvent, type: MapEventWheel) => {
        let payload = Helpers.buildWheelEventPayload(callback);
        let result = Helpers.buildEventResult(this.mapId, type, payload);
        this.getDotNetRef().invokeMethodAsync(EventNotifications.NotifyMapEventWheel, result);
    };

    #notifyMapEventWheel_Wheel = (callback: atlas.MapMouseWheelEvent) => this.#notifyMapEventWheel(callback, MapEventWheel.Wheel);

    // #endregion
}

enum MapEventConfig {
    MapConfigChanged = 'mapconfigurationchanged',
}

enum MapEventData {
    Data = 'data',
    SourceData = 'sourcedata',
    StyleData = 'styledata',
}

enum MapEventGeneral {
    BoxZoomEnd = 'boxzoomend',
    BoxZoomStart = 'boxzoomstart',
    Drag = 'drag',
    DragEnd = 'dragend',
    DragStart = 'dragstart',
    Idle = 'idle',
    Load = 'load',
    Move = 'move',
    MoveEnd = 'moveend',
    MoveStart = 'movestart',
    Pitch = 'pitch',
    PitchEnd = 'pitchend',
    PitchStart = 'pitchstart',
    Render = 'render',
    Resize = 'resize',
    Rotate = 'rotate',
    RotateEnd = 'rotateend',
    RotateStart = 'rotatestart',
    TokenAcquired = 'tokenacquired',
    Zoom = 'zoom',
    ZoomEnd = 'zoomend',
    ZoomStart = 'zoomstart'
}

enum MapEventSource {
    SourceAdded = 'sourceadded',
    SourceRemoved = 'sourceremoved'
}

enum MapEventStyle {
    StyleChanged = 'stylechanged',
    StyleImageMissing = 'styleimagemissing',
}

