namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ModifyingLowGradeEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LowGrades", "ClassTitle", c => c.String());
            AddColumn("dbo.LowGrades", "Explanation", c => c.String());
            DropColumn("dbo.LowGrades", "Class");
            DropColumn("dbo.LowGrades", "Explaination");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LowGrades", "Explaination", c => c.String());
            AddColumn("dbo.LowGrades", "Class", c => c.String());
            DropColumn("dbo.LowGrades", "Explanation");
            DropColumn("dbo.LowGrades", "ClassTitle");
        }
    }
}
