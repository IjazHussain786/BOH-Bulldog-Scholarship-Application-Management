namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MadeRatingsMoreGenericAndFilledOutTheRestOfTheDomain : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EssayRatings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Essay_Id = c.Int(),
                        Rating_Id = c.Int(),
                        ApplicantRating_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Essays", t => t.Essay_Id)
                .ForeignKey("dbo.GenericRatings", t => t.Rating_Id)
                .ForeignKey("dbo.ApplicantRatings", t => t.ApplicantRating_Id)
                .Index(t => t.Essay_Id)
                .Index(t => t.Rating_Id)
                .Index(t => t.ApplicantRating_Id);
            
            CreateTable(
                "dbo.GenericRatings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rating = c.Int(nullable: false),
                        Explanation = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ApplicantRatings", "FirstImpressionRating_Id", c => c.Int());
            AddColumn("dbo.ApplicantRatings", "OverallRating_Id", c => c.Int());
            CreateIndex("dbo.ApplicantRatings", "FirstImpressionRating_Id");
            CreateIndex("dbo.ApplicantRatings", "OverallRating_Id");
            AddForeignKey("dbo.ApplicantRatings", "FirstImpressionRating_Id", "dbo.GenericRatings", "Id");
            AddForeignKey("dbo.ApplicantRatings", "OverallRating_Id", "dbo.GenericRatings", "Id");
            DropColumn("dbo.ApplicantRatings", "OverallScore");
            DropColumn("dbo.ApplicantRatings", "Explanation");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApplicantRatings", "Explanation", c => c.String());
            AddColumn("dbo.ApplicantRatings", "OverallScore", c => c.Int());
            DropForeignKey("dbo.ApplicantRatings", "OverallRating_Id", "dbo.GenericRatings");
            DropForeignKey("dbo.ApplicantRatings", "FirstImpressionRating_Id", "dbo.GenericRatings");
            DropForeignKey("dbo.EssayRatings", "ApplicantRating_Id", "dbo.ApplicantRatings");
            DropForeignKey("dbo.EssayRatings", "Rating_Id", "dbo.GenericRatings");
            DropForeignKey("dbo.EssayRatings", "Essay_Id", "dbo.Essays");
            DropIndex("dbo.EssayRatings", new[] { "ApplicantRating_Id" });
            DropIndex("dbo.EssayRatings", new[] { "Rating_Id" });
            DropIndex("dbo.EssayRatings", new[] { "Essay_Id" });
            DropIndex("dbo.ApplicantRatings", new[] { "OverallRating_Id" });
            DropIndex("dbo.ApplicantRatings", new[] { "FirstImpressionRating_Id" });
            DropColumn("dbo.ApplicantRatings", "OverallRating_Id");
            DropColumn("dbo.ApplicantRatings", "FirstImpressionRating_Id");
            DropTable("dbo.GenericRatings");
            DropTable("dbo.EssayRatings");
        }
    }
}
