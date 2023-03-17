using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovaRecipesProject.Context.Migrations.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class AddedSetupUser1ToRecipesNRelationShipFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recipes_users_UserId",
                table: "recipes");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "recipes",
                newName: "RecipeUserId");

            migrationBuilder.RenameIndex(
                name: "IX_recipes_UserId",
                table: "recipes",
                newName: "IX_recipes_RecipeUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_recipes_users_RecipeUserId",
                table: "recipes",
                column: "RecipeUserId",
                principalTable: "users",
                principalColumn: "EntryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recipes_users_RecipeUserId",
                table: "recipes");

            migrationBuilder.RenameColumn(
                name: "RecipeUserId",
                table: "recipes",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_recipes_RecipeUserId",
                table: "recipes",
                newName: "IX_recipes_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_recipes_users_UserId",
                table: "recipes",
                column: "UserId",
                principalTable: "users",
                principalColumn: "EntryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
