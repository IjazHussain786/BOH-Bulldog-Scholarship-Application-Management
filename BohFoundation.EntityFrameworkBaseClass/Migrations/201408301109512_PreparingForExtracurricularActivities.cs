namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PreparingForExtracurricularActivities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExtracurricularActivities",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        PaidWork = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Applicants", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ShortSummaryOfWhatIsInvolved = c.String(),
                        ExtracurricularActivities_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExtracurricularActivities", t => t.ExtracurricularActivities_Id)
                .Index(t => t.ExtracurricularActivities_Id);
            
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Employer = c.String(),
                        Position = c.String(),
                        ShortSummaryOfWorkResponsibilities = c.String(),
                        ExtracurricularActivities_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExtracurricularActivities", t => t.ExtracurricularActivities_Id)
                .Index(t => t.ExtracurricularActivities_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExtracurricularActivities", "Id", "dbo.Applicants");
            DropForeignKey("dbo.Jobs", "ExtracurricularActivities_Id", "dbo.ExtracurricularActivities");
            DropForeignKey("dbo.Activities", "ExtracurricularActivities_Id", "dbo.ExtracurricularActivities");
            DropIndex("dbo.Jobs", new[] { "ExtracurricularActivities_Id" });
            DropIndex("dbo.Activities", new[] { "ExtracurricularActivities_Id" });
            DropIndex("dbo.ExtracurricularActivities", new[] { "Id" });
            DropTable("dbo.Jobs");
            DropTable("dbo.Activities");
            DropTable("dbo.ExtracurricularActivities");
        }
    }
}
