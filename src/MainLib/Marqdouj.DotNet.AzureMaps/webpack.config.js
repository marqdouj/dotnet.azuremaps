const path = require('path');

module.exports = {
    mode: 'production',
    entry: './scripts/index.ts',
    module: {
        rules: [
            // all files with a `.ts`, `.cts`, `.mts` or `.tsx` extension will be handled by `ts-loader`
            {
                test: /\.([cm]?ts|tsx)$/,
                loader: "ts-loader",
                exclude: /node_modules/,
            }
        ],
    },
    resolve: {
        extensions: ['.tsx', '.ts', '.js'],
    },
    output: {
        filename: 'marqdouj-azuremaps.js',
        path: path.resolve(__dirname, 'wwwroot'),
        clean: true,
        library: 'marqdoujAzureMaps',
    },
    externals: {
        "azure-maps-control": "atlas"
    }
};