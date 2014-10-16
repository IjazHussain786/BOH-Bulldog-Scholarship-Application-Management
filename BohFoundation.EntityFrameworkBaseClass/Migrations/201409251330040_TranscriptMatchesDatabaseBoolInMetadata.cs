namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TranscriptMatchesDatabaseBoolInMetadata : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicantMetadatas", "TranscriptMatchesDatabaseValues", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicantMetadatas", "TranscriptMatchesDatabaseValues");
        }
    }
}
