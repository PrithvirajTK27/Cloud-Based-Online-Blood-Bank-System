namespace DotNetAppSqlDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Todoes", "SUBJECTID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Todoes", "SUBJECTID");
        }
    }
}
