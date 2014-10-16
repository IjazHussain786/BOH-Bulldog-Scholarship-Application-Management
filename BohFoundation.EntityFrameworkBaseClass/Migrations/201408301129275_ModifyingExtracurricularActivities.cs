namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyingExtracurricularActivities : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ExtracurricularActivities", "PaidWork");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ExtracurricularActivities", "PaidWork", c => c.Boolean(nullable: false));
        }
    }
}
