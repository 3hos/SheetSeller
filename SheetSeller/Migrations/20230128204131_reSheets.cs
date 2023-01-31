using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SheetSeller.Migrations
{
    /// <inheritdoc />
    public partial class reSheets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Sheets_SheetID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Sheets_AspNetUsers_AuthorId",
                table: "Sheets");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SheetID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SheetID",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Sheets",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "ApplicationUserSheet",
                columns: table => new
                {
                    OwnedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OwnedSheetsID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserSheet", x => new { x.OwnedById, x.OwnedSheetsID });
                    table.ForeignKey(
                        name: "FK_ApplicationUserSheet_AspNetUsers_OwnedById",
                        column: x => x.OwnedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserSheet_Sheets_OwnedSheetsID",
                        column: x => x.OwnedSheetsID,
                        principalTable: "Sheets",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserSheet_OwnedSheetsID",
                table: "ApplicationUserSheet",
                column: "OwnedSheetsID");

            migrationBuilder.AddForeignKey(
                name: "FK_Sheets_AspNetUsers_AuthorId",
                table: "Sheets",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sheets_AspNetUsers_AuthorId",
                table: "Sheets");

            migrationBuilder.DropTable(
                name: "ApplicationUserSheet");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Sheets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SheetID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SheetID",
                table: "AspNetUsers",
                column: "SheetID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Sheets_SheetID",
                table: "AspNetUsers",
                column: "SheetID",
                principalTable: "Sheets",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Sheets_AspNetUsers_AuthorId",
                table: "Sheets",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
