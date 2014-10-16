//not unit tested.
module BohFoundation.Main {
    'use strict';
    Register.Common.config([
        'localStorageServiceProvider', ((localStorageServiceProvider) => {
            localStorageServiceProvider.setPrefix('BohFoundation');
        })
    ]);
} 