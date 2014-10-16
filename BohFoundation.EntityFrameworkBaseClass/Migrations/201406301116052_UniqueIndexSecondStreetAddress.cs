namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UniqueIndexSecondStreetAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Addresses", "StreetAddress1", c => c.String());
            AddColumn("dbo.Addresses", "StreetAddress2", c => c.String());
            AlterColumn("dbo.ContantInformations", "EmailAddress", c => c.String(nullable: false, maxLength: 90));
            CreateIndex("dbo.ContantInformations", "EmailAddress", unique: true, name: "Index");
            DropColumn("dbo.Addresses", "StreetAddress");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Addresses", "StreetAddress", c => c.String());
            DropIndex("dbo.ContantInformations", "Index");
            AlterColumn("dbo.ContantInformations", "EmailAddress", c => c.String());
            DropColumn("dbo.Addresses", "StreetAddress2");
            DropColumn("dbo.Addresses", "StreetAddress1");
        }
    }
}
