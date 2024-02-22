namespace RealEstateProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class improvedRoleModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Properties", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Properties", "ApplicationUser_Id");
            AddForeignKey("dbo.Properties", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Properties", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Properties", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Properties", "ApplicationUser_Id");
        }
    }
}
