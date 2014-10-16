module BohFoundation.Common.Spec.Filters.Enums {
    describe('IncomeRangeEnumFilter', () => {
        beforeEach(module('BohFoundation.Common'));

        it('should return LT20000 in string format.', (inject((incomeRangeEnumFilter) => {
            var result = incomeRangeEnumFilter(Common.Enums.IncomeRangeEnum.Lt20000);
            expect(result).toBe("Less than $20,000");
        })));

        it('should return Mt20000Lt30000 in string format.', (inject((incomeRangeEnumFilter) => {
            var result = incomeRangeEnumFilter(Common.Enums.IncomeRangeEnum.Mt20000Lt30000);
            expect(result).toBe("$20,000 - $30,000");
        })));

        it('should return Mt30000Lt40000 in string format.', (inject((incomeRangeEnumFilter) => {
            var result = incomeRangeEnumFilter(Common.Enums.IncomeRangeEnum.Mt30000Lt40000);
            expect(result).toBe("$30,000 - $40,000");
        })));

        it('should return Mt40000Lt50000 in string format.', (inject((incomeRangeEnumFilter) => {
            var result = incomeRangeEnumFilter(Common.Enums.IncomeRangeEnum.Mt40000Lt50000);
            expect(result).toBe("$40,000 - $50,000");
        })));

        it('should return Mt50000Lt60000 in string format.', (inject((incomeRangeEnumFilter) => {
            var result = incomeRangeEnumFilter(Common.Enums.IncomeRangeEnum.Mt50000Lt60000);
            expect(result).toBe("$50,000 - $60,000");
        })));

        it('should return Mt60000Lt70000 in string format.', (inject((incomeRangeEnumFilter) => {
            var result = incomeRangeEnumFilter(Common.Enums.IncomeRangeEnum.Mt60000Lt70000);
            expect(result).toBe("$60,000 - $70,000");
        })));

        it('should return Mt70000Lt80000 in string format.', (inject((incomeRangeEnumFilter) => {
            var result = incomeRangeEnumFilter(Common.Enums.IncomeRangeEnum.Mt70000Lt80000);
            expect(result).toBe("$70,000 - $80,000");
        })));

        it('should return Mt80000Lt90000 in string format.', (inject((incomeRangeEnumFilter) => {
            var result = incomeRangeEnumFilter(Common.Enums.IncomeRangeEnum.Mt80000Lt90000);
            expect(result).toBe("$80,000 - $90,000");
        })));

        it('should return Mt90000Lt100000 in string format.', (inject((incomeRangeEnumFilter) => {
            var result = incomeRangeEnumFilter(Common.Enums.IncomeRangeEnum.Mt90000Lt100000);
            expect(result).toBe("$90,000 - $100,000");
        })));

        it('should return Mt100000 in string format.', (inject((incomeRangeEnumFilter) => {
            var result = incomeRangeEnumFilter(Common.Enums.IncomeRangeEnum.Mt100000);
            expect(result).toBe("More than $100,000");
        })));
    });
}