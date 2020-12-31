using Microsoft.EntityFrameworkCore.Migrations;

namespace Glader.ASP.ServiceDiscovery.Migrations
{
    public partial class ChangedServiceEntryModelToServiceType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_services_ServiceName",
                table: "services");

            migrationBuilder.DropColumn(
                name: "ServiceName",
                table: "services");

            migrationBuilder.AddColumn<string>(
                name: "ServiceType",
                table: "services",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_services_ServiceType",
                table: "services",
                column: "ServiceType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_services_ServiceType",
                table: "services");

            migrationBuilder.DropColumn(
                name: "ServiceType",
                table: "services");

            migrationBuilder.AddColumn<string>(
                name: "ServiceName",
                table: "services",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_services_ServiceName",
                table: "services",
                column: "ServiceName");
        }
    }
}
