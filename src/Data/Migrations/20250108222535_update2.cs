using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apiCatedra3.src.Data.Migrations
{
    /// <inheritdoc />
    public partial class update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "posts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "posts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "linkImage",
                table: "posts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "publicationDate",
                table: "posts",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Title",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "linkImage",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "publicationDate",
                table: "posts");
        }
    }
}
