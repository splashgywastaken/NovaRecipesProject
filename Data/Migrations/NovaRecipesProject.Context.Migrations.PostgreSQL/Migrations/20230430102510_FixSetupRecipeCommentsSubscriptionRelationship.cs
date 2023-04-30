using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovaRecipesProject.Context.Migrations.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class FixSetupRecipeCommentsSubscriptionRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recipeCommentsSubscription_recipes_RecipeId",
                table: "recipeCommentsSubscription");

            migrationBuilder.DropIndex(
                name: "IX_recipeCommentsSubscription_RecipeId",
                table: "recipeCommentsSubscription");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "recipeCommentsSubscription");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "recipeCommentsSubscription",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_recipeCommentsSubscription_RecipeId",
                table: "recipeCommentsSubscription",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_recipeCommentsSubscription_recipes_RecipeId",
                table: "recipeCommentsSubscription",
                column: "RecipeId",
                principalTable: "recipes",
                principalColumn: "Id");
        }
    }
}
