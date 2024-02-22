namespace RealEstateProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addressForgotenLOl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Agencies", "Address", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Agencies", "Address");
        }
    }
}
