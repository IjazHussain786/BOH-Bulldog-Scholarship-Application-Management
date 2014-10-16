module BohFoundation.Common.Filters.Enums {
    export class IncomeRangeEnumFilter {
        constructor() {
            return ((itemNumber: Common.Enums.IncomeRangeEnum) => {
                switch (itemNumber) {
                    case Common.Enums.IncomeRangeEnum.Lt20000:
                        return "Less than $20,000";
                    case Common.Enums.IncomeRangeEnum.Mt20000Lt30000:
                        return "$20,000 - $30,000";
                    case Common.Enums.IncomeRangeEnum.Mt30000Lt40000:
                        return "$30,000 - $40,000";
                    case Common.Enums.IncomeRangeEnum.Mt40000Lt50000:
                        return "$40,000 - $50,000";
                    case Common.Enums.IncomeRangeEnum.Mt50000Lt60000:
                        return "$50,000 - $60,000";
                    case Common.Enums.IncomeRangeEnum.Mt60000Lt70000:
                        return "$60,000 - $70,000";
                    case Common.Enums.IncomeRangeEnum.Mt70000Lt80000:
                        return "$70,000 - $80,000";
                    case Common.Enums.IncomeRangeEnum.Mt80000Lt90000:
                        return "$80,000 - $90,000";
                    case Common.Enums.IncomeRangeEnum.Mt90000Lt100000:
                        return "$90,000 - $100,000";
                    case Common.Enums.IncomeRangeEnum.Mt100000:
                        return "More than $100,000";
                    default:
                        return "ERROR";
                }
            });
        }
    }
}

module BohFoundation.Main {
    Register.Common.filter("incomeRangeEnum", Common.Filters.Enums.IncomeRangeEnumFilter);
} 