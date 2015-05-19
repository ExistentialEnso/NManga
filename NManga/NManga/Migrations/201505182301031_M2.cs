namespace NManga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comics", "Ordinal", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comics", "Ordinal");
        }
    }
}
