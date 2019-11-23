namespace GigConnect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bands", "town", c => c.String());
            AddColumn("dbo.Venues", "town", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Venues", "town");
            DropColumn("dbo.Bands", "town");
        }
    }
}
