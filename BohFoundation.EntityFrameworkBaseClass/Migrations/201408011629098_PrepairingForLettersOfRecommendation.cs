namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrepairingForLettersOfRecommendation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.References",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Occupation = c.String(),
                        HasMembershipRebootAccount = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.LetterOfRecommendations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LetterOfRecommendationKeyValue = c.String(),
                        ReferenceRelationshipToApplicant = c.String(),
                        LastUpdated = c.DateTime(),
                        GuidSentToReference = c.Guid(nullable: false),
                        Applicant_Id = c.Int(),
                        Reference_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Applicants", t => t.Applicant_Id)
                .ForeignKey("dbo.References", t => t.Reference_Id)
                .Index(t => t.GuidSentToReference, name: "LetterOfRecommenationGuidIndex")
                .Index(t => t.Applicant_Id)
                .Index(t => t.Reference_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.References", "Id", "dbo.People");
            DropForeignKey("dbo.LetterOfRecommendations", "Reference_Id", "dbo.References");
            DropForeignKey("dbo.LetterOfRecommendations", "Applicant_Id", "dbo.Applicants");
            DropIndex("dbo.LetterOfRecommendations", new[] { "Reference_Id" });
            DropIndex("dbo.LetterOfRecommendations", new[] { "Applicant_Id" });
            DropIndex("dbo.LetterOfRecommendations", "LetterOfRecommenationGuidIndex");
            DropIndex("dbo.References", new[] { "Id" });
            DropTable("dbo.LetterOfRecommendations");
            DropTable("dbo.References");
        }
    }
}
