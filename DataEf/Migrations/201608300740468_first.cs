namespace DataEf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Question",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Code = c.String(maxLength: 100),
                        Text = c.String(maxLength: 300),
                        QuestionGroup_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuestionGroup", t => t.QuestionGroup_Id)
                .Index(t => t.QuestionGroup_Id);
            
            CreateTable(
                "dbo.QuestionGroup",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Code = c.String(maxLength: 100),
                        Text = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Response",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ResponseType = c.Int(nullable: false),
                        ResponseId = c.String(maxLength: 50),
                        InputId = c.Long(),
                        Email = c.String(maxLength: 100),
                        CompletionDate = c.DateTime(nullable: false),
                        Answer = c.String(maxLength: 500),
                        Question_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Question", t => t.Question_Id, cascadeDelete: true)
                .Index(t => t.Question_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Response", "Question_Id", "dbo.Question");
            DropForeignKey("dbo.Question", "QuestionGroup_Id", "dbo.QuestionGroup");
            DropIndex("dbo.Response", new[] { "Question_Id" });
            DropIndex("dbo.Question", new[] { "QuestionGroup_Id" });
            DropTable("dbo.Response");
            DropTable("dbo.QuestionGroup");
            DropTable("dbo.Question");
        }
    }
}
