namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingApplicantClasses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AcademicInformations",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Gpa = c.Double(nullable: false),
                        LinkToTranscript = c.String(),
                        CareerChoice = c.String(),
                        ProbableNextSchool = c.String(),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Applicants", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Applicants",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.ApplicantPersonalInformations",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        GraduatingYear = c.Int(nullable: false),
                        Birthdate = c.DateTime(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Applicants", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.FamilyInformations",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        NumberOfPeopleInHousehold = c.Int(nullable: false),
                        YearlyHouseholdIncome = c.Double(nullable: false),
                        HighestAttainedDegreeInHome = c.String(),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Applicants", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Guid = c.Guid(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ContantInformations",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        PhoneNumber = c.String(),
                        EmailAddress = c.String(),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        StreetAddress = c.String(),
                        City = c.String(),
                        State = c.String(),
                        ZipCode = c.String(),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContantInformations", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Names",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.LowGrades",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Grade = c.String(),
                        Class = c.String(),
                        WasAp = c.Boolean(nullable: false),
                        YearOfHighSchool = c.String(),
                        Explaination = c.String(),
                        LastUpdated = c.DateTime(nullable: false),
                        AcademicInformation_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AcademicInformations", t => t.AcademicInformation_Id, cascadeDelete: true)
                .Index(t => t.AcademicInformation_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LowGrades", "AcademicInformation_Id", "dbo.AcademicInformations");
            DropForeignKey("dbo.Names", "Id", "dbo.People");
            DropForeignKey("dbo.ContantInformations", "Id", "dbo.People");
            DropForeignKey("dbo.Addresses", "Id", "dbo.ContantInformations");
            DropForeignKey("dbo.Applicants", "Id", "dbo.People");
            DropForeignKey("dbo.FamilyInformations", "Id", "dbo.Applicants");
            DropForeignKey("dbo.ApplicantPersonalInformations", "Id", "dbo.Applicants");
            DropForeignKey("dbo.AcademicInformations", "Id", "dbo.Applicants");
            DropIndex("dbo.LowGrades", new[] { "AcademicInformation_Id" });
            DropIndex("dbo.Names", new[] { "Id" });
            DropIndex("dbo.Addresses", new[] { "Id" });
            DropIndex("dbo.ContantInformations", new[] { "Id" });
            DropIndex("dbo.FamilyInformations", new[] { "Id" });
            DropIndex("dbo.ApplicantPersonalInformations", new[] { "Id" });
            DropIndex("dbo.Applicants", new[] { "Id" });
            DropIndex("dbo.AcademicInformations", new[] { "Id" });
            DropTable("dbo.LowGrades");
            DropTable("dbo.Names");
            DropTable("dbo.Addresses");
            DropTable("dbo.ContantInformations");
            DropTable("dbo.People");
            DropTable("dbo.FamilyInformations");
            DropTable("dbo.ApplicantPersonalInformations");
            DropTable("dbo.Applicants");
            DropTable("dbo.AcademicInformations");
        }
    }
}
