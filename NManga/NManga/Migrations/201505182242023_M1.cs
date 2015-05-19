namespace NManga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comics", "TimePublished", c => c.DateTime(nullable: false));
            AddColumn("dbo.Comics", "FileName", c => c.String());
            AddColumn("dbo.Comics", "ContentType", c => c.String());
            AddColumn("dbo.Comics", "Content", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comics", "Content");
            DropColumn("dbo.Comics", "ContentType");
            DropColumn("dbo.Comics", "FileName");
            DropColumn("dbo.Comics", "TimePublished");
        }
    }
}
