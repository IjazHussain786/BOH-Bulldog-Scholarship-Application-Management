namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RethinkingExtracurriculars : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExtracurricularActivities", "PaidWork", c => c.Boolean(nullable: false));
            AddColumn("dbo.ExtracurricularActivities", "HasNonPaidActivities", c => c.Boolean(nullable: false));
            AddColumn("dbo.ExtracurricularActivities", "LastUpdated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ExtracurricularActivities", "LastUpdated");
            DropColumn("dbo.ExtracurricularActivities", "HasNonPaidActivities");
            DropColumn("dbo.ExtracurricularActivities", "PaidWork");
        }
    }
}
