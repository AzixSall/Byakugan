using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkerService.Migrations
{
    /// <inheritdoc />
    public partial class tableIpDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IpDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    countryCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    regionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    zip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lat = table.Column<float>(type: "real", nullable: true),
                    lon = table.Column<float>(type: "real", nullable: true),
                    timezone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    org = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _as = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    query = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IpDetails", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IpDetails");
        }
    }
}
