using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActivityRegistration.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTablePrefix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Activities",
                table: "Activities");

            migrationBuilder.RenameTable(
                name: "Activities",
                newName: "EAR_Activities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EAR_Activities",
                table: "EAR_Activities",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EAR_Activities",
                table: "EAR_Activities");

            migrationBuilder.RenameTable(
                name: "EAR_Activities",
                newName: "Activities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activities",
                table: "Activities",
                column: "Id");
        }
    }
}
