using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkerService.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NetworkPackets",
                columns: table => new
                {
                    DestinationIp = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SourceIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DestinationPort = table.Column<int>(type: "int", nullable: false),
                    SourcePort = table.Column<int>(type: "int", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetworkPackets", x => x.DestinationIp);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NetworkPackets");
        }
    }
}
