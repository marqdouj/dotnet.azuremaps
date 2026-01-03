export enum LogLevel {
    Trace = 0,
    Debug = 1,
    Information = 2,
    Warn = 3,
    Error = 4,
    Critical = 5,
    None = 6
}

export class Logger {
    static currentLevel: LogLevel = LogLevel.Information;

    static GetMapHeader(mapId: string): string {
        return `Map with Id '${mapId}'`;
    }

    static logMessage(mapId: string, level: LogLevel, message: string, ...optionalParams: any[]): void {
        if (level < this.currentLevel)
            return;

        const logOutput = `${this.GetMapHeader(mapId)} ${message}`;

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