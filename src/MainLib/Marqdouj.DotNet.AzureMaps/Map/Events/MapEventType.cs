namespace Marqdouj.DotNet.AzureMaps.Map.Events
{
    /// <summary>
    /// Represents the various types of map events that can be subscribed to.
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

        //Layer
        LayerAdded,
        LayerRemoved,

        //Mouse
        Click,
        ContextMenu,
        DblClick,
        MouseDown,
        MouseMove,
        MouseOut,
        MouseOver,
        MouseUp,
        Wheel,

        //Source
        SourceAdded,
        SourceRemoved,

        //Style
        StyleChanged,
        StyleImageMissing,
        StyleSelected,

        //Touch
        TouchCancel,
        TouchEnd,
        TouchMove,
        TouchStart
    }
}
