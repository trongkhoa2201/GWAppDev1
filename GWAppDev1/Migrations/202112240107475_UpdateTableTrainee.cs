namespace GWAppDev1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTableTrainee : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourseTrainees",
                c => new
                    {
                        CourseId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.CourseId, t.UserId })
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.CourseId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CourseTrainees", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CourseTrainees", "CourseId", "dbo.Courses");
            DropIndex("dbo.CourseTrainees", new[] { "UserId" });
            DropIndex("dbo.CourseTrainees", new[] { "CourseId" });
            DropTable("dbo.CourseTrainees");
        }
    }
}
