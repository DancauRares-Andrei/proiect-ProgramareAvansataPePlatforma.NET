namespace proiect_ProgramareAvansataPePlatforma.NET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookNameUnique : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderDetails", "BookId", "dbo.Books");
            DropIndex("dbo.OrderDetails", new[] { "BookId" });
            DropPrimaryKey("dbo.Books");
            AddColumn("dbo.OrderDetails", "Book_Title", c => c.String(maxLength: 100));
            AlterColumn("dbo.Books", "BookId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Books", "Title");
            CreateIndex("dbo.OrderDetails", "Book_Title");
            AddForeignKey("dbo.OrderDetails", "Book_Title", "dbo.Books", "Title");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDetails", "Book_Title", "dbo.Books");
            DropIndex("dbo.OrderDetails", new[] { "Book_Title" });
            DropPrimaryKey("dbo.Books");
            AlterColumn("dbo.Books", "BookId", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.OrderDetails", "Book_Title");
            AddPrimaryKey("dbo.Books", "BookId");
            CreateIndex("dbo.OrderDetails", "BookId");
            AddForeignKey("dbo.OrderDetails", "BookId", "dbo.Books", "BookId", cascadeDelete: true);
        }
    }
}
