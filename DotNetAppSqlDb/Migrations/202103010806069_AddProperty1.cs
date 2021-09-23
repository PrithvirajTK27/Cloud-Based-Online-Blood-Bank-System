namespace DotNetAppSqlDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProperty1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Todoes", "requst", c => c.Boolean(nullable: false));
            AddColumn("dbo.Todoes", "rply", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Todoes", "rply");
            DropColumn("dbo.Todoes", "requst");
        }
    }
}
