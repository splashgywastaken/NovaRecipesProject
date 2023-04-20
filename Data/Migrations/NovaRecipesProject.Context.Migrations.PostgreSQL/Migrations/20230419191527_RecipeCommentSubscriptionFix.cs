using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovaRecipesProject.Context.Migrations.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class RecipeCommentSubscriptionFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recipeCommentsSubscription_recipes_SubscriberId",
                table: "recipeCommentsSubscription");

            migrationBuilder.DropIndex(
                name: "IX_recipeCommentsSubscription_SubscriberId_RecipeId",
                table: "recipeCommentsSubscription");

            migrationBuilder.AddColumn<int>(
                name: "RecipeId1",
                table: "recipeCommentsSubscription",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_recipeCommentsSubscription_RecipeId_SubscriberId",
                table: "recipeCommentsSubscription",
                columns: new[] { "RecipeId", "SubscriberId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_recipeCommentsSubscription_RecipeId1",
                table: "recipeCommentsSubscription",
                column: "RecipeId1");

            migrationBuilder.CreateIndex(
                name: "IX_recipeCommentsSubscription_SubscriberId",
                table: "recipeCommentsSubscription",
                column: "SubscriberId");

            migrationBuilder.AddForeignKey(
                name: "FK_recipeCommentsSubscription_recipes_RecipeId",
                table: "recipeCommentsSubscription",
                column: "RecipeId",
                principalTable: "recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_recipeCommentsSubscription_recipes_RecipeId1",
                table: "recipeCommentsSubscription",
                column: "RecipeId1",
                principalTable: "recipes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recipeCommentsSubscription_recipes_RecipeId",
                table: "recipeCommentsSubscription");

            migrationBuilder.DropForeignKey(
                name: "FK_recipeCommentsSubscription_recipes_RecipeId1",
                table: "recipeCommentsSubscription");

            migrationBuilder.DropIndex(
                name: "IX_recipeCommentsSubscription_RecipeId_SubscriberId",
                table: "recipeCommentsSubscription");

            migrationBuilder.DropIndex(
                name: "IX_recipeCommentsSubscription_RecipeId1",
                table: "recipeCommentsSubscription");

            migrationBuilder.DropIndex(
                name: "IX_recipeCommentsSubscription_SubscriberId",
                table: "recipeCommentsSubscription");

            migrationBuilder.DropColumn(
                name: "RecipeId1",
                table: "recipeCommentsSubscription");

            migrationBuilder.CreateIndex(
                name: "IX_recipeCommentsSubscription_SubscriberId_RecipeId",
                table: "recipeCommentsSubscription",
                columns: new[] { "SubscriberId", "RecipeId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_recipeCommentsSubscription_recipes_SubscriberId",
                table: "recipeCommentsSubscription",
                column: "SubscriberId",
                principalTable: "recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
