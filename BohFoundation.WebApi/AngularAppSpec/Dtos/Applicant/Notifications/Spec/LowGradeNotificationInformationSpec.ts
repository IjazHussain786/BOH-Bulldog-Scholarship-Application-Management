module BohFoundation.Dtos.Applicant.Notifications.Spec {
    describe('LowGradeNotificationInformation', () => {
        var result;
        var date = new Date();

        it('should have 0 lowGradesOutstanding.', () => {
            result = new Notifications.LowGradeNotificationInformationModel(0, 4.0, date);
            expect(result.lowGradesNeededOutstanding).toBe(0);
        });

        it('should have 0 lowGradesOutstanding.', () => {
            result = new Notifications.LowGradeNotificationInformationModel(0, 3.71, date);
            expect(result.lowGradesNeededOutstanding).toBe(0);
        });

        it('should have 0 lowGradesOutstanding.', () => {
            result = new Notifications.LowGradeNotificationInformationModel(1, 3.7, date);
            expect(result.lowGradesNeededOutstanding).toBe(0);
        });

        it('should have 0 lowGradesOutstanding.', () => {
            result = new Notifications.LowGradeNotificationInformationModel(5, 2.5, date);
            expect(result.lowGradesNeededOutstanding).toBe(0);
        });

        it('should have 5 lowGradesOutstanding.', () => {
            result = new Notifications.LowGradeNotificationInformationModel(0, 2.5, date);
            expect(result.lowGradesNeededOutstanding).toBe(5);
        });
    });
}