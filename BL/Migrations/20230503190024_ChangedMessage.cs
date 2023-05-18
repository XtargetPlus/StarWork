using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BL.Migrations
{
    /// <inheritdoc />
    public partial class ChangedMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChildId",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChildId",
                table: "Messages",
                column: "ChildId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Messages_ChildId",
                table: "Messages",
                column: "ChildId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Messages_ChildId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ChildId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ChildId",
                table: "Messages");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Messages_FatherId",
                table: "Messages",
                column: "FatherId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
