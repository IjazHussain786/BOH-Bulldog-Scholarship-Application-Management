module BohFoundation.Dtos.Common.AzureQueuryObjects {
    export class AzureTableStorageEntityKeyModel {
        constructor(
            public partitionKey: string,
            public rowKey: string) { }
    }
}