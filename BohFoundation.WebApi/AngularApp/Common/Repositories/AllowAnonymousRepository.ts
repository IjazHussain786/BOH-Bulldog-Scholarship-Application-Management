module BohFoundation.Common.Repositories {
    'use strict';
    export interface IAllowAnonymousRepository {
        post(data: any, path: string): ng.resource.IResource<any>;
        get(path:string):ng.resource.IResource<any>;
    }

    export class AllowAnonymousRepository implements IAllowAnonymousRepository {
        static $inject = ['$resource'];
        constructor(private $resource: ng.resource.IResourceService) { }

        post(data: any, path: string) {
            return this.$resource(path).save(data);
        }

        get(path: string) {
            return this.$resource(path).get();
        }
    }
}

module BohFoundation.Main {
    Register.Common.service("AllowAnonymousRepository", Common.Repositories.AllowAnonymousRepository);
}