namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddIndexToPersonGuid : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.People", "Guid", name: "Index");
        }
        
        public override void Down()
        {
            DropIndex("dbo.People", "Index");
        }
    }
}
