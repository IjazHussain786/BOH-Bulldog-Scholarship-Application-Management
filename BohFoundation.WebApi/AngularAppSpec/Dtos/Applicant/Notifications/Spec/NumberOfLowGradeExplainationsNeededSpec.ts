module BohFoundation.Dtos.Applicant.Notifications.Spec {
    describe('NumberOfLowGradeExplainationsNeededModel', () => {
        var numberOfLowGradeExplainationsNeeded: Notifications.INumberOfLowGradeExplainationsNeededModel;

        it('should return 0 for 4.0.', () => {
            setLowGradeObject(4.0);
            expect(numberOfLowGradeExplainationsNeeded.totalExplanationsNeeded).toBe(0);
        });

        it('should return 0 for 3.71.', () => {
            setLowGradeObject(3.71);
            expect(numberOfLowGradeExplainationsNeeded.totalExplanationsNeeded).toBe(0);
        });

        it('should return 1 for 3.7.', () => {
            setLowGradeObject(3.7);
            expect(numberOfLowGradeExplainationsNeeded.totalExplanationsNeeded).toBe(1);
        });

        it('should return 2 for 3.4.', () => {
            setLowGradeObject(3.4);
            expect(numberOfLowGradeExplainationsNeeded.totalExplanationsNeeded).toBe(2);
        });

        it('should return 3 for 3.1.', () => {
            setLowGradeObject(3.1);
            expect(numberOfLowGradeExplainationsNeeded.totalExplanationsNeeded).toBe(3);
        });

        it('should return 4 for 2.8.', () => {
            setLowGradeObject(2.8);
            expect(numberOfLowGradeExplainationsNeeded.totalExplanationsNeeded).toBe(4);
        });

        it('should return 5 for 2.5.', () => {
            setLowGradeObject(2.5);
            expect(numberOfLowGradeExplainationsNeeded.totalExplanationsNeeded).toBe(5);
        });

        it('should return 5 for 0.1.', () => {
            setLowGradeObject(0.1);
            expect(numberOfLowGradeExplainationsNeeded.totalExplanationsNeeded).toBe(5);
        });

        
        function setLowGradeObject(gpa: number) {
            numberOfLowGradeExplainationsNeeded = new Notifications.NumberOfLowGradeExplainationsNeededModel(gpa);
        }
    });
}