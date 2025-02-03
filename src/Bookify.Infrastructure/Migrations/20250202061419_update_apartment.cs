using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_apartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "amenities",
                table: "apartments",
                newName: "Amenities");

            migrationBuilder.AlterColumn<string>(
                name: "Amenities",
                table: "apartments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(int[]),
                oldType: "integer[]",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amenities",
                table: "apartments",
                newName: "amenities");

            migrationBuilder.AlterColumn<int[]>(
                name: "amenities",
                table: "apartments",
                type: "integer[]",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
