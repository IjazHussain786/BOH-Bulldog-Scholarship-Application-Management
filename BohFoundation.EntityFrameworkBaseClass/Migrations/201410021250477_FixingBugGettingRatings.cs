namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixingBugGettingRatings : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GenericRatings", "RatingEnum", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GenericRatings", "RatingEnum", c => c.Int(nullable: false));
        }
    }
}
