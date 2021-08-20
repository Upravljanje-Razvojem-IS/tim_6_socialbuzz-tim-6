namespace UserService_IT59_2017.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiredRoleName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Roles", "Role_Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Roles", "Role_Name", c => c.String());
        }
    }
}
