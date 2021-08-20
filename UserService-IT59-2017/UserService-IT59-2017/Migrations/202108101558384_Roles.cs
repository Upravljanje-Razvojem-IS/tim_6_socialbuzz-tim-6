namespace UserService_IT59_2017.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Roles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "RolaId", c => c.Int(nullable: false));
            CreateIndex("dbo.Accounts", "RolaId");
            AddForeignKey("dbo.Accounts", "RolaId", "dbo.Roles", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accounts", "RolaId", "dbo.Roles");
            DropIndex("dbo.Accounts", new[] { "RolaId" });
            DropColumn("dbo.Accounts", "RolaId");
        }
    }
}
