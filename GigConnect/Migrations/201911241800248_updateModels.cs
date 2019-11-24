namespace GigConnect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateModels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bands", "bandWebsite", c => c.String(nullable: false));
            DropColumn("dbo.Bands", "bandWebist");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bands", "bandWebist", c => c.String(nullable: false));
            DropColumn("dbo.Bands", "bandWebsite");
        }
    }
}
