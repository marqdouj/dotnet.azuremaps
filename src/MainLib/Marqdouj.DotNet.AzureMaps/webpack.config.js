const path = require('path');

module.exports = {
    mode: 'production',
    entry: './tsgen/index.js',
    output: {
        filename: 'marqdouj-azuremaps.js',
        path: path.resolve(__dirname, 'wwwroot'),
        library: 'marqdoujAzureMaps',
    },
    externals: {
        "azure-maps-control": "atlas"
    }
};