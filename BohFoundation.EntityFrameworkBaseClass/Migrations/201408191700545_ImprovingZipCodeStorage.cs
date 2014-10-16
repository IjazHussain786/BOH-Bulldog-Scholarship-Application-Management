namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImprovingZipCodeStorage : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Addresses", "ZipCode", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Addresses", "ZipCode", c => c.String());
        }
    }
}
