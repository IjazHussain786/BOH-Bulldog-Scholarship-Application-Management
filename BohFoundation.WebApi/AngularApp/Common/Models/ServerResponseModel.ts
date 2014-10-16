module BohFoundation.Common.Models {
    'use strict';
    export class ServerResponseModel {
        constructor(public dataFromServer: any, public success: boolean) { }
    }
}  