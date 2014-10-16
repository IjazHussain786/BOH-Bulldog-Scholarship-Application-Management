namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MadeTranscriptReferenceMoreAccurate : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Transcripts", newName: "TranscriptBlobReferences");
            AddColumn("dbo.TranscriptBlobReferences", "ReferenceToTranscriptPdf", c => c.String());
            AddColumn("dbo.TranscriptBlobReferences", "BlobContainerName", c => c.String());
            DropColumn("dbo.TranscriptBlobReferences", "LinkToTranscript");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TranscriptBlobReferences", "LinkToTranscript", c => c.String());
            DropColumn("dbo.TranscriptBlobReferences", "BlobContainerName");
            DropColumn("dbo.TranscriptBlobReferences", "ReferenceToTranscriptPdf");
            RenameTable(name: "dbo.TranscriptBlobReferences", newName: "Transcripts");
        }
    }
}
