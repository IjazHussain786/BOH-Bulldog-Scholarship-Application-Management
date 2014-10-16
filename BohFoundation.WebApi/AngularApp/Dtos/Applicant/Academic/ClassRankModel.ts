 module BohFoundation.Dtos.Applicant.Academic {
     export class ClassRankModel {
         constructor(
             public classNumericalRank?: number,
             public graduatingClassSize?: number,
             public classPercentile?: number,
             public lastUpdated?: Date) { }
     }
 }