using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SheetSeller.Migrations
{
    /// <inheritdoc />
    public partial class addSheets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "SheetID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Sheets",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sheets", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Sheets_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SheetID",
                table: "AspNetUsers",
                column: "SheetID");

            migrationBuilder.CreateIndex(
                name: "IX_Sheets_AuthorId",
                table: "Sheets",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Sheets_SheetID",
                table: "AspNetUsers",
                column: "SheetID",
                principalTable: "Sheets",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Sheets_SheetID",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Sheets");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SheetID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SheetID",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
