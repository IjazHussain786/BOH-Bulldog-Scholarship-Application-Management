module BohFoundation.Admin.Spec.Services {
    describe('AdminNotificationService', () => {
        var service;
        var authRequiredRepo, alertHelper;

        beforeEach(() => {
            authRequiredRepo = sinon.stub({
                get:()=>{}
            });

            alertHelper = sinon.stub({
                addDangerAlert:()=>{}
            });

            service = new Admin.Services.AdminNotificationService(authRequiredRepo, alertHelper);
        });

        describe('getNotifications', () => {
            var result;
            var data = 1193;

            beforeEach(() => {
                authRequiredRepo.get.returns(data);
                result = service.getNotifications();
            });

            it('should call authRequiredRepo with /api/admin/notifications.', () => {
                sinon.assert.calledWith(authRequiredRepo.get, '/api/admin/notifications');
            });

            it('should call authRequiredRepo with /api/admin/notifications.', () => {
                expect(result).toBe(data);
            });
        });

        describe('getNotifications', () => {
            describe('error', () => {
                beforeEach(() => {
                    service.resolveNotifications(new Common.Models.ServerResponseModel(null, false));
                });

                it('should call the alert helper danger method.', () => {
                    sinon.assert.calledWith(alertHelper.addDangerAlert, "There was an error getting your admin notifications.");
                });
            });

            describe('success', () => {
                var pendingApplicationEvaluators = 2;
                var totalCount = pendingApplicationEvaluators;

                var dataFromServer = { pendingApplicationEvaluators: pendingApplicationEvaluators, totalCount: totalCount}

                beforeEach(() => {
                    service.resolveNotifications(new Common.Models.ServerResponseModel(dataFromServer, true));
                });

                it('should assign pendingApplicationEvaluators to adminNotificationCounts.', () => {
                    expect(service.adminNotificationCounts.pendingApplicationEvaluators).toBe(pendingApplicationEvaluators);
                });

                it('should assign totalCount to adminNotificationCounts.', () => {
                    expect(service.adminNotificationCounts.totalCount).toBe(totalCount);
                });
            });
        });

        describe('getNumberOfTotalNotifications', () => {
            var totalCounts = 291;

            it('should return 0 is adminNotificationCounts is undefined.', () => {
                service.adminNotificationCounts = undefined;
                expect(service.getNumberOfTotalNotifications()).toBe(0);
            });

            it('should return the total count.', () => {
                service.adminNotificationCounts = new Dtos.Admin.AdminNotificationCounts(totalCounts, null);
                expect(service.getNumberOfTotalNotifications()).toBe(totalCounts);
            });
        });

        describe('getNumberOfPendingApplicationEvaluators', () => {
            var pendingApplicationEvaluators = 29123;

            beforeEach(() => {
                service.adminNotificationCounts = new Dtos.Admin.AdminNotificationCounts(null, pendingApplicationEvaluators);
            });

            it('should return the total count.', () => {
                expect(service.getNumberOfPendingApplicationEvaluators()).toBe(pendingApplicationEvaluators);
            });

            it('should return 0 is adminNotificationCounts is undefined.', () => {
                service.adminNotificationCounts = undefined;
                expect(service.getNumberOfPendingApplicationEvaluators()).toBe(0);
            });
        });
    });
} 