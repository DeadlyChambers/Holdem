namespace Holdem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Playing = c.Boolean(nullable: false),
                        Position = c.Int(nullable: false),
                        CardsJson = c.String(),
                        Chips = c.Int(nullable: false),
                        HandEnum = c.Int(nullable: false),
                        HandStrength = c.Int(nullable: false),
                        WinningHand = c.Boolean(nullable: false),
                        Table_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tables", t => t.Table_Id)
                .Index(t => t.Table_Id);
            
            CreateTable(
                "dbo.Tables",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DeckJson = c.String(),
                        Pot = c.Int(nullable: false),
                        CardsJson = c.String(),
                        DealOver = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Players", "Table_Id", "dbo.Tables");
            DropIndex("dbo.Players", new[] { "Table_Id" });
            DropTable("dbo.Tables");
            DropTable("dbo.Players");
        }
    }
}
