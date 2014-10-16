namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingApplicantMetadata : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicantMetadatas",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ApplicationFinalized = c.Boolean(nullable: false),
                        AcceptanceNonSelectionLetterSent = c.Boolean(nullable: false),
                        Finalist = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Applicants", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicantMetadatas", "Id", "dbo.Applicants");
            DropIndex("dbo.ApplicantMetadatas", new[] { "Id" });
            DropTable("dbo.ApplicantMetadatas");
        }
    }
}
