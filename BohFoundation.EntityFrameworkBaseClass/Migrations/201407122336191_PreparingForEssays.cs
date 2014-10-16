namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class PreparingForEssays : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GraduatingClasses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GraduatingYear = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.GraduatingYear, unique: true, name: "Index");
            
            CreateTable(
                "dbo.EssayTopics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TitleOfEssay = c.String(),
                        EssayPrompt = c.String(),
                        Timestamp = c.Binary(),
                        RevisionDateTime = c.DateTime(nullable: false),
                        LastRevisionAuthor_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admins", t => t.LastRevisionAuthor_Id)
                .Index(t => t.LastRevisionAuthor_Id);
            
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationEvaluators",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.EssayTopicGraduatingClasses",
                c => new
                    {
                        EssayTopic_Id = c.Int(nullable: false),
                        GraduatingClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EssayTopic_Id, t.GraduatingClass_Id })
                .ForeignKey("dbo.EssayTopics", t => t.EssayTopic_Id, cascadeDelete: true)
                .ForeignKey("dbo.GraduatingClasses", t => t.GraduatingClass_Id, cascadeDelete: true)
                .Index(t => t.EssayTopic_Id)
                .Index(t => t.GraduatingClass_Id);
            
            AddColumn("dbo.ApplicantPersonalInformations", "GraduatingClass_Id", c => c.Int());
            CreateIndex("dbo.ApplicantPersonalInformations", "GraduatingClass_Id");
            AddForeignKey("dbo.ApplicantPersonalInformations", "GraduatingClass_Id", "dbo.GraduatingClasses", "Id");
            DropColumn("dbo.ApplicantPersonalInformations", "GraduatingYear");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApplicantPersonalInformations", "GraduatingYear", c => c.Int(nullable: false));
            DropForeignKey("dbo.ApplicantPersonalInformations", "GraduatingClass_Id", "dbo.GraduatingClasses");
            DropForeignKey("dbo.ApplicationEvaluators", "Id", "dbo.People");
            DropForeignKey("dbo.Admins", "Id", "dbo.People");
            DropForeignKey("dbo.EssayTopics", "LastRevisionAuthor_Id", "dbo.Admins");
            DropForeignKey("dbo.EssayTopicGraduatingClasses", "GraduatingClass_Id", "dbo.GraduatingClasses");
            DropForeignKey("dbo.EssayTopicGraduatingClasses", "EssayTopic_Id", "dbo.EssayTopics");
            DropIndex("dbo.EssayTopicGraduatingClasses", new[] { "GraduatingClass_Id" });
            DropIndex("dbo.EssayTopicGraduatingClasses", new[] { "EssayTopic_Id" });
            DropIndex("dbo.ApplicationEvaluators", new[] { "Id" });
            DropIndex("dbo.Admins", new[] { "Id" });
            DropIndex("dbo.EssayTopics", new[] { "LastRevisionAuthor_Id" });
            DropIndex("dbo.GraduatingClasses", "Index");
            DropIndex("dbo.ApplicantPersonalInformations", new[] { "GraduatingClass_Id" });
            DropColumn("dbo.ApplicantPersonalInformations", "GraduatingClass_Id");
            DropTable("dbo.EssayTopicGraduatingClasses");
            DropTable("dbo.ApplicationEvaluators");
            DropTable("dbo.Admins");
            DropTable("dbo.EssayTopics");
            DropTable("dbo.GraduatingClasses");
        }
    }
}
