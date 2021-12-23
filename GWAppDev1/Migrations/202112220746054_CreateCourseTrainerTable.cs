namespace GWAppDev1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateCourseTrainerTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourseTrainers",
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
            DropForeignKey("dbo.CourseTrainers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CourseTrainers", "CourseId", "dbo.Courses");
            DropIndex("dbo.CourseTrainers", new[] { "UserId" });
            DropIndex("dbo.CourseTrainers", new[] { "CourseId" });
            DropTable("dbo.CourseTrainers");
        }
    }
}
