namespace DotNetAppSqlDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Todoes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        DonatedDate = c.DateTime(nullable: false),
                        DonorGender = c.Int(nullable: false),
                        UserBirthDate = c.DateTime(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Blood_Group = c.Int(nullable: false),
                        Pola = c.Int(nullable: false),
                        Storage_Area = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Todoes");
        }
    }
}
