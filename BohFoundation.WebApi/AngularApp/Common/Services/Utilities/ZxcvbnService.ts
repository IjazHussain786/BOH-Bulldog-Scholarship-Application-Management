//Not Unit Tested.
module BohFoundation.Common.Services.Utilities {
    'use strict';
    export interface IZxcvbnService {
        getZxcvbn(): void;
        zxcvbnLoaded(): boolean;
        zxcvbnScore(password: string): number;
    }

    declare var zxcvbn;

    export class ZxcvbnService implements IZxcvbnService {

        private zxcvbnLoadedState = false;
        private skrollrLoadedState = false;

        static $inject = ['scriptjs'];
        constructor(private scriptjs) { }

        zxcvbnLoaded() {
            return this.zxcvbnLoadedState;
        }

        zxcvbnScore(password: string): number {
            return zxcvbn(password).score;
        }

        getZxcvbn() {
            this.scriptjs.get("https://cdnjs.cloudflare.com/ajax/libs/zxcvbn/1.0/zxcvbn.min.js", () => {
                if (zxcvbn != undefined) {
                    this.zxcvbnLoadedState = true;
                }
            });
        }
    }
}

module BohFoundation.Main {
    Register.Common.service("ZxcvbnService", Common.Services.Utilities.ZxcvbnService);
}