using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nile.Migrations
{
    /// <inheritdoc />
    public partial class Comment_Threading : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Keep existing single-column FK indexes to avoid MySQL FK dependency issues

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "FriendRelationships",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "ParentCommentId",
                table: "Comments",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRelationships_RequesterUserId_Status",
                table: "FriendRelationships",
                columns: new[] { "RequesterUserId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_FriendRelationships_TargetUserId_Status",
                table: "FriendRelationships",
                columns: new[] { "TargetUserId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_FriendRelationships_TargetUserId_Status_CreatedAt",
                table: "FriendRelationships",
                columns: new[] { "TargetUserId", "Status", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentCommentId",
                table: "Comments",
                column: "ParentCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_ParentCommentId",
                table: "Comments",
                column: "ParentCommentId",
                principalTable: "Comments",
                principalColumn: "CommentId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_ParentCommentId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_FriendRelationships_RequesterUserId_Status",
                table: "FriendRelationships");

            migrationBuilder.DropIndex(
                name: "IX_FriendRelationships_TargetUserId_Status",
                table: "FriendRelationships");

            migrationBuilder.DropIndex(
                name: "IX_FriendRelationships_TargetUserId_Status_CreatedAt",
                table: "FriendRelationships");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ParentCommentId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ParentCommentId",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "FriendRelationships",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            // Down: original single-column FK indexes were not dropped in Up, so no need to recreate here
        }
    }
}
