namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakingIncomeAndDegreeAttainmentEnumerables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FamilyInformations", "YearlyHouseholdIncomeRange", c => c.Int(nullable: false));
            AlterColumn("dbo.FamilyInformations", "HighestAttainedDegreeInHome", c => c.Int(nullable: false));
            DropColumn("dbo.FamilyInformations", "YearlyHouseholdIncome");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FamilyInformations", "YearlyHouseholdIncome", c => c.Double(nullable: false));
            AlterColumn("dbo.FamilyInformations", "HighestAttainedDegreeInHome", c => c.String());
            DropColumn("dbo.FamilyInformations", "YearlyHouseholdIncomeRange");
        }
    }
}
