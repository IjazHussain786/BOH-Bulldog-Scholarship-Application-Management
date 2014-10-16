module BohFoundation.Common.Services.Utilities {
    export interface ICreateYearArrayService {
        getThisYearPlusFourArray():Array<number>;
    }

    export class CreateYearArrayService implements ICreateYearArrayService {
        getThisYearPlusFourArray() : Array<number> {
            var arrayToReturn = [];
            var thisYear = new Date().getFullYear();

            for (var i = 0; i < 5; i++) {
                arrayToReturn.push(thisYear + i);
            }

            return arrayToReturn;
        }
    }
}

module BohFoundation.Main {
    Register.Common.service("CreateYearArrayService", Common.Services.Utilities.CreateYearArrayService);
}