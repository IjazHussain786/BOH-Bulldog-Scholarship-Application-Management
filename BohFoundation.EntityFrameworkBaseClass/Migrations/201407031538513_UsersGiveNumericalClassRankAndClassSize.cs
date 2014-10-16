namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsersGiveNumericalClassRankAndClassSize : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.W2", "LinkToW2", c => c.String());
            AddColumn("dbo.ClassRanks", "ClassNumericalRank", c => c.Int(nullable: false));
            AddColumn("dbo.ClassRanks", "GraduatingClassSize", c => c.Int(nullable: false));
            DropColumn("dbo.W2", "ListToW2");
            DropColumn("dbo.ClassRanks", "Rank");
            DropColumn("dbo.ClassRanks", "Percentile");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ClassRanks", "Percentile", c => c.Double(nullable: false));
            AddColumn("dbo.ClassRanks", "Rank", c => c.String());
            AddColumn("dbo.W2", "ListToW2", c => c.String());
            DropColumn("dbo.ClassRanks", "GraduatingClassSize");
            DropColumn("dbo.ClassRanks", "ClassNumericalRank");
            DropColumn("dbo.W2", "LinkToW2");
        }
    }
}
