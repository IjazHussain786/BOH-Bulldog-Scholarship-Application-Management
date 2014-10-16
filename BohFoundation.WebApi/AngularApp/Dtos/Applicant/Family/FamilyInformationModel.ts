 module BohFoundation.Dtos.Applicant.Family {
     export class FamilyInformationModel {
         constructor(
             private numberOfPeopleInHousehold?: number,
             private yearlyHouseholdIncomeRange?: BohFoundation.Common.Enums.IncomeRangeEnum,
             private highestAttainedDegreeInHome?: BohFoundation.Common.Enums.EducationalDegreesEnum,
             private lastUpdated?: Date) { }
     }
 }