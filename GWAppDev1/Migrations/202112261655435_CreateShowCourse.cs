namespace GWAppDev1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateShowCourse : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.CourseTrainers", name: "TrainerId", newName: "UserId");
            RenameIndex(table: "dbo.CourseTrainers", name: "IX_TrainerId", newName: "IX_UserId");
            DropPrimaryKey("dbo.CourseTrainers");
            AddPrimaryKey("dbo.CourseTrainers", new[] { "CourseId", "UserId" });
            DropColumn("dbo.CourseTrainers", "Id");
            DropColumn("dbo.CourseTrainers", "TrainerName");
            DropColumn("dbo.CourseTrainers", "CourseName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CourseTrainers", "CourseName", c => c.String());
            AddColumn("dbo.CourseTrainers", "TrainerName", c => c.String());
            AddColumn("dbo.CourseTrainers", "Id", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.CourseTrainers");
            AddPrimaryKey("dbo.CourseTrainers", "Id");
            RenameIndex(table: "dbo.CourseTrainers", name: "IX_UserId", newName: "IX_TrainerId");
            RenameColumn(table: "dbo.CourseTrainers", name: "UserId", newName: "TrainerId");
        }
    }
}
