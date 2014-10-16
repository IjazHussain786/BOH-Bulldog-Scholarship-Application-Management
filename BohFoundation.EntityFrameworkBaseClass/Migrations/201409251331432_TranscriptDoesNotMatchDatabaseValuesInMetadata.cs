namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TranscriptDoesNotMatchDatabaseValuesInMetadata : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicantMetadatas", "TranscriptDoesNotMatchDatabaseValues", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicantMetadatas", "TranscriptDoesNotMatchDatabaseValues");
        }
    }
}
