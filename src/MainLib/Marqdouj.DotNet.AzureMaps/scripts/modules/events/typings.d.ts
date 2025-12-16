interface MapEventDef {
    type: string;
    once?: boolean;
    target: 'map' | 'datasource' | 'htmlmarker' | 'layer' | 'shape' | 'stylecontrol' | 'popup';
    targetId?: string;
    targetSourceId?: string;
}

interface MapEventResult {
    mapId: string;
    type: string;
    payload?: object;
}
