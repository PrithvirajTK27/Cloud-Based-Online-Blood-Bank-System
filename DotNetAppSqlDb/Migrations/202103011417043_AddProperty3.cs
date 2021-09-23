namespace DotNetAppSqlDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProperty3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Todoes", "type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Todoes", "type");
        }
    }
}
