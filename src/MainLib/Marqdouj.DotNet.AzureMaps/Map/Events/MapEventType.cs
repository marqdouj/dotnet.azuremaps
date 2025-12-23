using Marqdouj.DotNet.AzureMaps.Map.Interop;

namespace Marqdouj.DotNet.AzureMaps.Map.Events
{
    /// <summary>
    /// Represents all types of events that can be subscribed to.
    /// </summary>
    public enum MapEventType
    {
        //General
        BoxZoomEnd,
        BoxZoomStart,
        Drag,
        DragEnd,
        DragStart,
        [Obsolete("Error is now always subscribed and no longer optional.")]
        Error,
        Idle,
        /// <summary>
        /// Applies to target HtmlMarker only.
        /// </summary>
        KeyDown,
        /// <summary>
        /// Applies to target HtmlMarker only.
        /// </summary>
        KeyPress,
        /// <summary>
        /// Applies to target HtmlMarker only.
        /// </summary>
        KeyUp, 
        Load,
        Move,
        MoveEnd,
        MoveStart,
        Pitch,
        PitchEnd,
        PitchStart,
        Render,
        Resize,
        Rotate,
        RotateEnd,
        RotateStart,
        TokenAcquired,
        Zoom,
        ZoomEnd,
        ZoomStart,

        //Config
        MapConfigurationChanged,

        //Data
        Data,
        SourceData,
        StyleData,

        //DataSource
        /// <summary>
        /// Applies to target DataSource only.
        /// </summary>
        DataSourceUpdated,
        /// <summary>
        /// Applies to target DataSource only.
        /// </summary>
        DataAdded,
        /// <summary>
        /// Applies to target DataSource only.
        /// </summary>
        DataRemoved,

        //Layer
        /// <summary>
        /// This event must be subscribed to when creating the layer.
        /// Once the layer has been added to the map, this event will not fire.
        /// <see cref="IAzureMapsLayers.CreateLayer"/>."/>
        /// </summary>
        LayerAdded,
        LayerRemoved,

        //Mouse
        Click,
        ContextMenu,
        DblClick,
        MouseDown,
        /// <summary>
        /// Applies to target Layer only.
        /// </summary>
        MouseEnter,
        /// <summary>
        /// Applies to target Layer only.
        /// </summary>
        MouseLeave,
        MouseMove,
        MouseOut,
        MouseOver,
        MouseUp,
        Wheel,

        //Popup
        /// <summary>
        /// Applies to target Popup only.
        /// </summary>
        Open,
        /// <summary>
        /// Applies to target Popup only.
        /// </summary>
        Close,

        //Source
        SourceAdded,
        SourceRemoved,

        //Shape
        /// <summary>
        /// Applies to target Shape only.
        /// </summary>
        ShapeChanged,

        //Style
        StyleChanged,
        StyleImageMissing,
        /// <summary>
        /// Applies to target StyleControl only.
        /// </summary>
        StyleSelected,

        //Touch
        TouchCancel,
        TouchEnd,
        TouchMove,
        TouchStart
    }

    /// <summary>
    /// Enum that represents all MapEventType's that apply to MapEventTarget.Map. Castable to MapEventType.
    /// </summary>
    public enum MapEventTypeMap
    {
        //Config
        MapConfigurationChanged = MapEventType.MapConfigurationChanged,

        //Data
        Data = MapEventType.Data,
        SourceData = MapEventType.SourceData,
        StyleData = MapEventType.StyleData,

        //General
        BoxZoomEnd = MapEventType.BoxZoomEnd,
        BoxZoomStart = MapEventType.BoxZoomStart,
        Drag = MapEventType.Drag,
        DragEnd = MapEventType.DragEnd,
        DragStart = MapEventType.DragStart,
        Idle = MapEventType.Idle,
        Load = MapEventType.Load,
        Move = MapEventType.Move,
        MoveEnd = MapEventType.MoveEnd,
        MoveStart = MapEventType.MoveStart,
        Pitch = MapEventType.Pitch,
        PitchEnd = MapEventType.PitchEnd,
        PitchStart = MapEventType.PitchStart,
        Render = MapEventType.Render,
        Resize = MapEventType.Resize,
        Rotate = MapEventType.Rotate,
        RotateEnd = MapEventType.RotateEnd,
        RotateStart = MapEventType.RotateStart,
        TokenAcquired = MapEventType.TokenAcquired,
        Zoom = MapEventType.Zoom,
        ZoomEnd = MapEventType.ZoomEnd,
        ZoomStart = MapEventType.ZoomStart,

        //Layer
        LayerAdded = MapEventType.LayerAdded,
        LayerRemoved = MapEventType.LayerRemoved,

        //Mouse
        Click = MapEventType.Click,
        ContextMenu = MapEventType.ContextMenu,
        DblClick = MapEventType.DblClick,
        MouseDown = MapEventType.MouseDown,
        MouseMove = MapEventType.MouseMove,
        MouseOut = MapEventType.MouseOut,
        MouseOver = MapEventType.MouseOver,
        MouseUp = MapEventType.MouseUp,
        Wheel = MapEventType.Wheel,

        //Source
        SourceAdded = MapEventType.SourceAdded,
        SourceRemoved = MapEventType.SourceRemoved,

        //Style
        StyleChanged = MapEventType.StyleChanged,
        StyleImageMissing = MapEventType.StyleImageMissing,

        //Touch
        TouchCancel = MapEventType.TouchCancel,
        TouchEnd = MapEventType.TouchEnd,
        TouchMove = MapEventType.TouchMove,
        TouchStart = MapEventType.TouchStart,
    }

    /// <summary>
    /// Subset of MapEventType that applies to MapEventTarget.DataSource. Castable to MapEventType.
    /// </summary>
    public enum MapEventTypeDataSource
    {
        DataSourceUpdated = MapEventType.DataSourceUpdated,
        DataAdded = MapEventType.DataAdded,
        DataRemoved = MapEventType.DataRemoved,
    }

    /// <summary>
    /// Subset of MapEventType that applies to MapEventTarget.Layer. Castable to MapEventType.
    /// </summary>
    public enum MapEventTypeLayer
    {
        LayerAdded = MapEventType.LayerAdded,
        LayerRemoved = MapEventType.LayerRemoved,

        Click = MapEventType.Click,
        ContextMenu = MapEventType.ContextMenu,
        DblClick = MapEventType.DblClick,
        MouseDown = MapEventType.MouseDown,
        MouseEnter = MapEventType.MouseEnter,
        MouseLeave = MapEventType.MouseLeave,
        MouseMove = MapEventType.MouseMove,
        MouseOut = MapEventType.MouseOut,
        MouseOver = MapEventType.MouseOver,
        MouseUp = MapEventType.MouseUp,

        TouchCancel = MapEventType.TouchCancel,
        TouchEnd = MapEventType.TouchEnd,
        TouchMove = MapEventType.TouchMove,
        TouchStart = MapEventType.TouchStart,

        Wheel = MapEventType.Wheel,
    }

    /// <summary>
    /// Subset of MapEventType that applies to MapEventTarget.HtmlMarker. Castable to MapEventType.
    /// </summary>
    public enum MapEventTypeHtmlMarker
    {
        Click = MapEventType.Click,
        ContextMenu = MapEventType.ContextMenu,
        DblClick = MapEventType.DblClick,
        MouseDown = MapEventType.MouseDown,
        MouseEnter = MapEventType.MouseEnter,
        MouseLeave = MapEventType.MouseLeave,
        MouseMove = MapEventType.MouseMove,
        MouseOut = MapEventType.MouseOut,
        MouseOver = MapEventType.MouseOver,
        MouseUp = MapEventType.MouseUp,

        Drag = MapEventType.Drag,
        DragEnd = MapEventType.DragEnd,
        DragStart = MapEventType.DragStart,

        KeyDown = MapEventType.KeyDown,
        KeyPress = MapEventType.KeyPress,
        KeyUp = MapEventType.KeyUp,
    }

    /// <summary>
    /// Subset of MapEventType that applies to MapEventTarget.Popup. Castable to MapEventType.
    /// </summary>
    public enum MapEventTypePopup
    {
        Drag = MapEventType.Drag,
        DragEnd = MapEventType.DragEnd,
        DragStart = MapEventType.DragStart,
        Open = MapEventType.Open,
        Close = MapEventType.Close,
    }

    /// <summary>
    /// Subset of MapEventType that applies to MapEventTarget.Shape. Castable to MapEventType.
    /// </summary>
    public enum MapEventTypeShape
    {
        ShapeChanged = MapEventType.ShapeChanged,
    }

    /// <summary>
    /// Subset of MapEventType that applies to MapEventTarget.StyleControl. Castable to MapEventType.
    /// </summary>
    public enum MapEventTypeStyleControl
    {
        StyleSelected = MapEventType.StyleSelected,
    }
}
