namespace oiat.saferinternetbot.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class LoggingTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "dbo.McLog",
                    c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Application = c.String(nullable: false, maxLength: 50),
                        Logged = c.DateTime(nullable: false),
                        Level = c.String(nullable: false, maxLength: 50),
                        Message = c.String(nullable: false),
                        UserName = c.String(nullable: true, maxLength: 250),
                        ServerName = c.String(nullable: true),
                        Port = c.String(nullable: true),
                        Url = c.String(nullable: true),
                        Https = c.Boolean(nullable: true),
                        ServerAddress = c.String(nullable: true, maxLength: 100),
                        RemoteAddress = c.String(nullable: true, maxLength: 100),
                        Logger = c.String(nullable: true, maxLength: 250),
                        Callsite = c.String(nullable: true),
                        Exception = c.String(nullable: true)
                    })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("dbo.McLog");
        }
    }
}
