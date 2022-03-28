namespace oiat.saferinternetbot.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDefaultAnswers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.saferbotDefaultAnswer",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Text = c.String(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreateUser = c.String(),
                        CreateSource = c.String(),
                        UpdateDate = c.DateTime(),
                        UpdateUser = c.String(),
                        UpdateSource = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.saferbotDefaultAnswer");
        }
    }
}
