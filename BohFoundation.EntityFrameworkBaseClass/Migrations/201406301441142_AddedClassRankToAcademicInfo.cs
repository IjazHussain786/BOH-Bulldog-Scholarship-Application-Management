namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedClassRankToAcademicInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClassRanks",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Rank = c.String(),
                        Percentile = c.Double(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AcademicInformations", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClassRanks", "Id", "dbo.AcademicInformations");
            DropIndex("dbo.ClassRanks", new[] { "Id" });
            DropTable("dbo.ClassRanks");
        }
    }
}
