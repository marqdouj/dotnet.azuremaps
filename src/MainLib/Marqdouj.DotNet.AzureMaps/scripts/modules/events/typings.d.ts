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
