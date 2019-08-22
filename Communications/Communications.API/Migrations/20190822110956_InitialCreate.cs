using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Communications.API.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MailTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    From = table.Column<string>(nullable: true),
                    To = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    Processed = table.Column<bool>(nullable: false),
                    ProcessedTimestamp = table.Column<DateTime>(nullable: true),
                    ReceivedTimestamp = table.Column<DateTime>(nullable: false),
                    VendorId = table.Column<Guid>(nullable: false),
                    ClientId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DefaultFromAddress = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    RegisteredTimestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MailTasks");

            migrationBuilder.DropTable(
                name: "Vendors");
        }
    }
}
