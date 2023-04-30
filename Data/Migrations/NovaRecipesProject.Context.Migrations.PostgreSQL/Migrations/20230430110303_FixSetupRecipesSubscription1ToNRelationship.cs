using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovaRecipesProject.Context.Migrations.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class FixSetupRecipesSubscription1ToNRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "userRecipesSubscription",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_userRecipesSubscription_UserId",
                table: "userRecipesSubscription",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_userRecipesSubscription_users_UserId",
                table: "userRecipesSubscription",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userRecipesSubscription_users_UserId",
                table: "userRecipesSubscription");

            migrationBuilder.DropIndex(
                name: "IX_userRecipesSubscription_UserId",
                table: "userRecipesSubscription");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "userRecipesSubscription");
        }
    }
}
