namespace GWAppDev1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateCourseTable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Teams", "UniqueName");
            DropTable("dbo.Teams");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Teams", "Name", unique: true, name: "UniqueName");
        }
    }
}
