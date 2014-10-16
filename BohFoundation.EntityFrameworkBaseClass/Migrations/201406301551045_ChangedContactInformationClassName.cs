namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedContactInformationClassName : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ContantInformations", newName: "ContactInformations");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.ContactInformations", newName: "ContantInformations");
        }
    }
}
