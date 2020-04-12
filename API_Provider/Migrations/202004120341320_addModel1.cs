namespace API_Provider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addModel1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tb_M_UserRole", "Role_Id", "dbo.Tb_M_Role");
            DropForeignKey("dbo.Tb_M_UserRole", "User_Id", "dbo.User");
            DropForeignKey("dbo.Tb_M_Customer", "Id", "dbo.User");
            DropIndex("dbo.Tb_M_Customer", new[] { "Id" });
            DropIndex("dbo.Tb_M_UserRole", new[] { "Role_Id" });
            DropIndex("dbo.Tb_M_UserRole", new[] { "User_Id" });
            DropTable("dbo.User");
            DropTable("dbo.Tb_M_UserRole");
            DropTable("dbo.Tb_M_Role");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Tb_M_Role",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tb_M_UserRole",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Role_Id = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        PasswordHash = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Tb_M_UserRole", "User_Id");
            CreateIndex("dbo.Tb_M_UserRole", "Role_Id");
            CreateIndex("dbo.Tb_M_Customer", "Id");
            AddForeignKey("dbo.Tb_M_Customer", "Id", "dbo.User", "Id");
            AddForeignKey("dbo.Tb_M_UserRole", "User_Id", "dbo.User", "Id");
            AddForeignKey("dbo.Tb_M_UserRole", "Role_Id", "dbo.Tb_M_Role", "Id", cascadeDelete: true);
        }
    }
}
