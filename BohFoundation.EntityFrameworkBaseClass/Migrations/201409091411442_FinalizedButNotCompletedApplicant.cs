namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinalizedButNotCompletedApplicant : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicantMetadatas", "NotCompletedButFinalizedApplication", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicantMetadatas", "NotCompletedButFinalizedApplication");
        }
    }
}
