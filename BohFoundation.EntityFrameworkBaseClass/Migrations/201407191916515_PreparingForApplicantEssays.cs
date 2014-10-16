namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PreparingForApplicantEssays : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Essays",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EssayKeyValue = c.String(),
                        RevisionDateTime = c.DateTime(nullable: false),
                        CharacterLength = c.Int(nullable: false),
                        EssayTopic_Id = c.Int(),
                        Applicant_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EssayTopics", t => t.EssayTopic_Id)
                .ForeignKey("dbo.Applicants", t => t.Applicant_Id)
                .Index(t => t.EssayTopic_Id)
                .Index(t => t.Applicant_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Essays", "Applicant_Id", "dbo.Applicants");
            DropForeignKey("dbo.Essays", "EssayTopic_Id", "dbo.EssayTopics");
            DropIndex("dbo.Essays", new[] { "Applicant_Id" });
            DropIndex("dbo.Essays", new[] { "EssayTopic_Id" });
            DropTable("dbo.Essays");
        }
    }
}
