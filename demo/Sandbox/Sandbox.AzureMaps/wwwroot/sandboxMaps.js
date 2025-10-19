export function mapExists(mapId) {
    var azmap = getMap(mapId);
    return azmap != undefined;
}

export function addControls(mapId) {
    var azmap = getMap(mapId);
    let msg;
    if (azmap) {
        removeControls(mapId);
        azmap.controls.add([
            new atlas.control.ZoomControl(),
            new atlas.control.PitchControl(),
            new atlas.control.CompassControl(),
            new atlas.control.StyleControl(),
            new atlas.control.FullscreenControl(),
            new atlas.control.ScaleControl(),
        ], {
            position: atlas.ControlPosition.TopRight,
        });
        msg = `${logHeader(mapId)}. Controls were added via custom interop!`;
    }
    else {
        msg = `${logHeader(mapId)}. Map was NOT found via custom interop!`;
    }
    console.debug(msg);
    return msg;
}

export function removeControls(mapId) {
    var azmap = getMap(mapId);
    let msg;
    if (azmap) {
        var controls = azmap.controls.getControls();
        azmap.controls.remove(controls);
        msg = `${logHeader(mapId)}. Controls were removed via custom interop!`;
    }
    else {
        msg = `${logHeader(mapId)}. Map was NOT found via custom interop!`;
    }
    console.debug(msg);
    return msg;
}

function getMap(mapId) {
    return marqdoujAzureMaps.MapFactory.getMap(mapId);
}

function logHeader(mapId) {
    return `Map with ID '${mapId}'`;
}
