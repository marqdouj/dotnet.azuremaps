export namespace MapEvents {
    export enum EventNotifications {
        NotifyMapEvent = 'NotifyMapEvent',
        NotifyMapEventConfig = 'NotifyMapEventConfig',
        NotifyMapEventData = 'NotifyMapEventData',
        NotifyMapEventError = 'NotifyMapEventError',
        NotifyMapEventLayer = 'NotifyMapEventLayer',
        NotifyMapEventMouse = 'NotifyMapEventMouse',
        NotifyMapEventReady = 'NotifyMapEventReady',
        NotifyMapEventSource = 'NotifyMapEventSource',
        NotifyMapEventStyle = 'NotifyMapEventStyle',
        NotifyMapEventTouch = 'NotifyMapEventTouch',
    }

    export enum MapEventGeneral {
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

    export enum MapEventConfig {
        MapConfigChanged = 'mapconfigurationchanged',
    }

    export enum MapEventData {
        Data = 'data',
        SourceData = 'sourcedata',
        StyleData = 'styledata',
    }

    export enum MapEventLayer {
        LayerAdded = 'layeradded',
        LayerRemoved = 'layerremoved'
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

    export enum MapEventWheel {
        Wheel = 'wheel',
    }

    export enum MapEventSource {
        SourceAdded = 'sourceadded',
        SourceRemoved = 'sourceremoved'
    }

    export enum MapEventStyle {
        StyleImageMissing = 'styleimagemissing',
        StyleSelected = 'styleselected',
    }

    export enum MapEventStyleChanged {
        StyleChanged = 'stylechanged',
    }

    export enum MapEventTouch {
        TouchCancel = 'touchcancel',
        TouchEnd = 'touchend',
        TouchMove = 'touchmove',
        TouchStart = 'touchstart'
    }
}
