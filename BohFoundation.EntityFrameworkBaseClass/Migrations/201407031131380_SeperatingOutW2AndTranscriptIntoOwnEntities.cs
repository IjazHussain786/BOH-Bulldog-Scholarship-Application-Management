namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeperatingOutW2AndTranscriptIntoOwnEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.W2",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ListToW2 = c.String(),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FamilyInformations", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Transcripts",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        LinkToTranscript = c.String(),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AcademicInformations", t => t.Id)
                .Index(t => t.Id);
            
            DropColumn("dbo.AcademicInformations", "LinkToTranscript");
            DropColumn("dbo.AcademicInformations", "TranscriptUploaded");
            DropColumn("dbo.FamilyInformations", "W2Uploaded");
            DropColumn("dbo.FamilyInformations", "W2Link");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FamilyInformations", "W2Link", c => c.String());
            AddColumn("dbo.FamilyInformations", "W2Uploaded", c => c.Boolean(nullable: false));
            AddColumn("dbo.AcademicInformations", "TranscriptUploaded", c => c.Boolean(nullable: false));
            AddColumn("dbo.AcademicInformations", "LinkToTranscript", c => c.String());
            DropForeignKey("dbo.Transcripts", "Id", "dbo.AcademicInformations");
            DropForeignKey("dbo.W2", "Id", "dbo.FamilyInformations");
            DropIndex("dbo.Transcripts", new[] { "Id" });
            DropIndex("dbo.W2", new[] { "Id" });
            DropTable("dbo.Transcripts");
            DropTable("dbo.W2");
        }
    }
}
