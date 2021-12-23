namespace GWAppDev1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCourseTrainerTable : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.CourseTrainers", name: "UserId", newName: "TrainerId");
            RenameIndex(table: "dbo.CourseTrainers", name: "IX_UserId", newName: "IX_TrainerId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.CourseTrainers", name: "IX_TrainerId", newName: "IX_UserId");
            RenameColumn(table: "dbo.CourseTrainers", name: "TrainerId", newName: "UserId");
        }
    }
}
