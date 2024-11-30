namespace proiect_ProgramareAvansataPePlatforma.NET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RegexBook : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Books", "Title", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Books", "Author", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Books", "Author", c => c.String());
            AlterColumn("dbo.Books", "Title", c => c.String());
        }
    }
}
