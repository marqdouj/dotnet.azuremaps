export namespace Logger {
    export enum LogLevel {
        Trace = 0,
        Debug = 1,
        Information = 2,
        Warn = 3,
        Error = 4,
        Critical = 5,
        None = 6
    }

    export var currentLevel: LogLevel = LogLevel.Information;

    function GetMapHeader(mapId: string): string {
        return `Map with Id '${mapId}'`;
    }

    export function logMessage(mapId:string, level: LogLevel, message: string, ...optionalParams: any[]): void {
        if (level < currentLevel)
            return;

        const logOutput = `${GetMapHeader(mapId)} ${message}`;

        switch (level) {
            case LogLevel.Trace:
                console.trace(logOutput, ...optionalParams);
                break;
            case LogLevel.Debug:
                console.debug(logOutput, ...optionalParams);
                break;
            case LogLevel.Information:
                console.info(logOutput, ...optionalParams);
                break;
            case LogLevel.Warn:
                console.warn(logOutput, ...optionalParams);
                break;
            case LogLevel.Error:
                console.error(logOutput, ...optionalParams);
                break;
            case LogLevel.Critical:
                console.error(`CRITICAL: ${logOutput}`, ...optionalParams);
                break;

        }
    }
}

export class Extensions {
    static isEmptyOrNull(str: string | null | undefined): boolean {
        return str?.trim() === "";
    }

    static isNotEmptyOrNull(str: string | null | undefined): boolean {
        return !this.isEmptyOrNull(str);
    }
}
