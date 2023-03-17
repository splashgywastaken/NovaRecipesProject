using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NovaRecipesProject.Context.Migrations.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class AddedSetupUser1ToRecipesNRelationShip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EntryId",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "recipes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_users_EntryId",
                table: "users",
                column: "EntryId");

            migrationBuilder.CreateIndex(
                name: "IX_recipes_UserId",
                table: "recipes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_recipes_users_UserId",
                table: "recipes",
                column: "UserId",
                principalTable: "users",
                principalColumn: "EntryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recipes_users_UserId",
                table: "recipes");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_users_EntryId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_recipes_UserId",
                table: "recipes");

            migrationBuilder.DropColumn(
                name: "EntryId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "recipes");
        }
    }
}
