using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project_navigator.Migrations
{
    /// <inheritdoc />
    public partial class AddedAccessLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccessLevelId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AccessLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessLevels", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_AccessLevelId",
                table: "Users",
                column: "AccessLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AccessLevels_AccessLevelId",
                table: "Users",
                column: "AccessLevelId",
                principalTable: "AccessLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_AccessLevels_AccessLevelId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "AccessLevels");

            migrationBuilder.DropIndex(
                name: "IX_Users_AccessLevelId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AccessLevelId",
                table: "Users");
        }
    }
}
