namespace proiect_ProgramareAvansataPePlatforma.NET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Orders3 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Orders");
            AlterColumn("dbo.Orders", "OrderId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Orders", "UserId", c => c.String());
            AddPrimaryKey("dbo.Orders", "OrderId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Orders");
            AlterColumn("dbo.Orders", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Orders", "OrderId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Orders", "OrderId");
        }
    }
}
