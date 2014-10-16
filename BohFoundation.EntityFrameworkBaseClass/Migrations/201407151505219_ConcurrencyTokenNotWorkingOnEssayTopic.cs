namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConcurrencyTokenNotWorkingOnEssayTopic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EssayTopics", "ConcurrencyTimestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            DropColumn("dbo.EssayTopics", "Timestamp");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EssayTopics", "Timestamp", c => c.Binary());
            DropColumn("dbo.EssayTopics", "ConcurrencyTimestamp");
        }
    }
}
