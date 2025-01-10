using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apiCatedra3.src.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePostUserRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_posts_AspNetUsers_AppUserId",
                table: "posts");

            migrationBuilder.DropIndex(
                name: "IX_posts_AppUserId",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "posts");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "posts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_posts_UserId",
                table: "posts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_posts_AspNetUsers_UserId",
                table: "posts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_posts_AspNetUsers_UserId",
                table: "posts");

            migrationBuilder.DropIndex(
                name: "IX_posts_UserId",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "posts");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "posts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_posts_AppUserId",
                table: "posts",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_posts_AspNetUsers_AppUserId",
                table: "posts",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
