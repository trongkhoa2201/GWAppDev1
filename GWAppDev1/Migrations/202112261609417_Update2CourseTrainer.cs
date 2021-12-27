namespace GWAppDev1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update2CourseTrainer : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.CourseTrainers", name: "UserId", newName: "TrainerId");
            RenameIndex(table: "dbo.CourseTrainers", name: "IX_UserId", newName: "IX_TrainerId");
            DropPrimaryKey("dbo.CourseTrainers");
            AddColumn("dbo.CourseTrainers", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.CourseTrainers", "TrainerName", c => c.String());
            AddColumn("dbo.CourseTrainers", "CourseName", c => c.String());
            AddPrimaryKey("dbo.CourseTrainers", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.CourseTrainers");
            DropColumn("dbo.CourseTrainers", "CourseName");
            DropColumn("dbo.CourseTrainers", "TrainerName");
            DropColumn("dbo.CourseTrainers", "Id");
            AddPrimaryKey("dbo.CourseTrainers", new[] { "CourseId", "UserId" });
            RenameIndex(table: "dbo.CourseTrainers", name: "IX_TrainerId", newName: "IX_UserId");
            RenameColumn(table: "dbo.CourseTrainers", name: "TrainerId", newName: "UserId");
        }
    }
}
