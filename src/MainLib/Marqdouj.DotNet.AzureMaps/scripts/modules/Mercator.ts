import * as atlas from "azure-maps-control";

export class Mercator {
    static fromPosition(position: atlas.data.Position): atlas.data.MercatorPoint {
        return atlas.data.MercatorPoint.fromPosition(position);
    }

    static fromPositions(positions: atlas.data.Position[]): atlas.data.MercatorPoint[] {
        return atlas.data.MercatorPoint.fromPositions(positions);
    }

    static toPosition(mercator: atlas.data.MercatorPoint): atlas.data.Position {
        return atlas.data.MercatorPoint.toPosition(mercator);
    }

    static toPositions(mercators: atlas.data.MercatorPoint[]): atlas.data.Position[] {
        return atlas.data.MercatorPoint.toPositions(mercators);
    }

    static toFloat32Array(positions: atlas.data.Position[]): Float32Array {
        return atlas.data.MercatorPoint.toFloat32Array(positions);
    }

    static mercatorScale(latitude: number): number {
        return atlas.data.MercatorPoint.mercatorScale(latitude);
    }

    static meterInMercatorUnits(latitude: number): number {
        return atlas.data.MercatorPoint.meterInMercatorUnits(latitude);
    }
}
