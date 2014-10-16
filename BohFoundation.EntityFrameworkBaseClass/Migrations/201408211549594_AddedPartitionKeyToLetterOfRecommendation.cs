namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPartitionKeyToLetterOfRecommendation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LetterOfRecommendations", "LetterOfRecommendationRowKey", c => c.String());
            AddColumn("dbo.LetterOfRecommendations", "LetterOfRecommendationPartitionKey", c => c.String());
            DropColumn("dbo.LetterOfRecommendations", "LetterOfRecommendationKeyValue");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LetterOfRecommendations", "LetterOfRecommendationKeyValue", c => c.String());
            DropColumn("dbo.LetterOfRecommendations", "LetterOfRecommendationPartitionKey");
            DropColumn("dbo.LetterOfRecommendations", "LetterOfRecommendationRowKey");
        }
    }
}
