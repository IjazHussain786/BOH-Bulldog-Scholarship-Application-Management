namespace BohFoundation.EntityFrameworkBaseClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImprovingPhoneNumberStorage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PhoneInformations",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        PhoneNumber = c.Long(nullable: false),
                        BestTimeToContactByPhone = c.Int(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContactInformations", t => t.Id)
                .Index(t => t.Id);
            
            DropColumn("dbo.ContactInformations", "PhoneNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ContactInformations", "PhoneNumber", c => c.String());
            DropForeignKey("dbo.PhoneInformations", "Id", "dbo.ContactInformations");
            DropIndex("dbo.PhoneInformations", new[] { "Id" });
            DropTable("dbo.PhoneInformations");
        }
    }
}
