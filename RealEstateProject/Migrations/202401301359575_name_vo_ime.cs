namespace RealEstateProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class name_vo_ime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Ime", c => c.String());
            DropColumn("dbo.AspNetUsers", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
            DropColumn("dbo.AspNetUsers", "Ime");
        }
    }
}
