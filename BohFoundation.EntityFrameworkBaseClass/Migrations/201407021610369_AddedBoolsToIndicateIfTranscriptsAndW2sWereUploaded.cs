namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBoolsToIndicateIfTranscriptsAndW2sWereUploaded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AcademicInformations", "TranscriptUploaded", c => c.Boolean(nullable: false));
            AddColumn("dbo.FamilyInformations", "W2Uploaded", c => c.Boolean(nullable: false));
            AddColumn("dbo.FamilyInformations", "W2Link", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FamilyInformations", "W2Link");
            DropColumn("dbo.FamilyInformations", "W2Uploaded");
            DropColumn("dbo.AcademicInformations", "TranscriptUploaded");
        }
    }
}
