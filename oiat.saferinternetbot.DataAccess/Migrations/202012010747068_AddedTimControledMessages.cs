namespace oiat.saferinternetbot.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTimControledMessages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.saferbotTimeControlledMessage",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        StartTime = c.DateTimeOffset(nullable: false, precision: 7),
                        EndTime = c.DateTimeOffset(nullable: false, precision: 7),
                        Enabled = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreateUser = c.String(),
                        CreateSource = c.String(),
                        UpdateDate = c.DateTime(),
                        UpdateUser = c.String(),
                        UpdateSource = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.saferbotDefaultAnswer", "TimeControlledMessageId", c => c.Guid());
            CreateIndex("dbo.saferbotDefaultAnswer", "TimeControlledMessageId");
            AddForeignKey("dbo.saferbotDefaultAnswer", "TimeControlledMessageId", "dbo.saferbotTimeControlledMessage", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.saferbotDefaultAnswer", "TimeControlledMessageId", "dbo.saferbotTimeControlledMessage");
            DropIndex("dbo.saferbotDefaultAnswer", new[] { "TimeControlledMessageId" });
            DropColumn("dbo.saferbotDefaultAnswer", "TimeControlledMessageId");
            DropTable("dbo.saferbotTimeControlledMessage");
        }
    }
}
