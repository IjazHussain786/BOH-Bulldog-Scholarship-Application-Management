namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllowNullsOnAcademicInformation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AcademicInformations", "LastUpdated", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AcademicInformations", "LastUpdated", c => c.DateTime(nullable: false));
        }
    }
}
