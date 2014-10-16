namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedRatingToRatingEnumInGenericRating : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GenericRatings", "RatingEnum", c => c.Int(nullable: false));
            DropColumn("dbo.GenericRatings", "Rating");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GenericRatings", "Rating", c => c.Int(nullable: false));
            DropColumn("dbo.GenericRatings", "RatingEnum");
        }
    }
}
