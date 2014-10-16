module BohFoundation.Common.Models {
    export class DropZoneConfigModel {

        constructor(urlToApi: string, nameOfFile: string, authHeader: string, typeOfFile: string, maxNumberOfFiles?: number, maxFileSizeInMegs?: number) {

            this.options = {};
            
            this.options.url = urlToApi;
            this.options.paramName = nameOfFile;
            this.options.headers = { 'Authorization': authHeader };
            this.options.acceptedFiles = typeOfFile;
            this.options.maxFiles = maxNumberOfFiles;
            this.options.maxFilesize = maxFileSizeInMegs;
        }

        public options;
    }
}