namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingPartitionAndRowKeysToDb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Essays", "RowKey", c => c.String());
            AddColumn("dbo.Essays", "PartitionKey", c => c.String());
            DropColumn("dbo.Essays", "EssayKeyValue");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Essays", "EssayKeyValue", c => c.String());
            DropColumn("dbo.Essays", "PartitionKey");
            DropColumn("dbo.Essays", "RowKey");
        }
    }
}
