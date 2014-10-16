namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class YearOfHighSchoolNowAnEnum : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LowGrades", "YearOfHighSchool", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LowGrades", "YearOfHighSchool", c => c.String());
        }
    }
}
