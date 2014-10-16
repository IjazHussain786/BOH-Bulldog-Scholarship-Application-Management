namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingAnApplicantRating : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicantRatings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OverallScore = c.Int(),
                        Explanation = c.String(),
                        Applicant_Id = c.Int(),
                        ApplicationEvaluator_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Applicants", t => t.Applicant_Id)
                .ForeignKey("dbo.ApplicationEvaluators", t => t.ApplicationEvaluator_Id)
                .Index(t => t.Applicant_Id)
                .Index(t => t.ApplicationEvaluator_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicantRatings", "ApplicationEvaluator_Id", "dbo.ApplicationEvaluators");
            DropForeignKey("dbo.ApplicantRatings", "Applicant_Id", "dbo.Applicants");
            DropIndex("dbo.ApplicantRatings", new[] { "ApplicationEvaluator_Id" });
            DropIndex("dbo.ApplicantRatings", new[] { "Applicant_Id" });
            DropTable("dbo.ApplicantRatings");
        }
    }
}
