using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nile.Migrations
{
    /// <inheritdoc />
    public partial class GroupMember_Tuning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeenAt",
                table: "Messages");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "GroupMembers",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_GroupId_Role_JoinedAt",
                table: "GroupMembers",
                columns: new[] { "GroupId", "Role", "JoinedAt" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_GroupMember_Role",
                table: "GroupMembers",
                sql: "Role IN ('admin','mod','member')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GroupMembers_GroupId_Role_JoinedAt",
                table: "GroupMembers");

            migrationBuilder.DropCheckConstraint(
                name: "CK_GroupMember_Role",
                table: "GroupMembers");

            migrationBuilder.AddColumn<DateTime>(
                name: "SeenAt",
                table: "Messages",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "GroupMembers",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
