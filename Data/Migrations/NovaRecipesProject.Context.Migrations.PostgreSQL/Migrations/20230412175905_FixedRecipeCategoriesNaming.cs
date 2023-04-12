using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovaRecipesProject.Context.Migrations.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class FixedRecipeCategoriesNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeCategories_categories_CategoriesId",
                table: "RecipeCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeCategories_recipes_RecipesId",
                table: "RecipeCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeCategories",
                table: "RecipeCategories");

            migrationBuilder.RenameTable(
                name: "RecipeCategories",
                newName: "recipeCategories");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeCategories_RecipesId",
                table: "recipeCategories",
                newName: "IX_recipeCategories_RecipesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_recipeCategories",
                table: "recipeCategories",
                columns: new[] { "CategoriesId", "RecipesId" });

            migrationBuilder.AddForeignKey(
                name: "FK_recipeCategories_categories_CategoriesId",
                table: "recipeCategories",
                column: "CategoriesId",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_recipeCategories_recipes_RecipesId",
                table: "recipeCategories",
                column: "RecipesId",
                principalTable: "recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recipeCategories_categories_CategoriesId",
                table: "recipeCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_recipeCategories_recipes_RecipesId",
                table: "recipeCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_recipeCategories",
                table: "recipeCategories");

            migrationBuilder.RenameTable(
                name: "recipeCategories",
                newName: "RecipeCategories");

            migrationBuilder.RenameIndex(
                name: "IX_recipeCategories_RecipesId",
                table: "RecipeCategories",
                newName: "IX_RecipeCategories_RecipesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeCategories",
                table: "RecipeCategories",
                columns: new[] { "CategoriesId", "RecipesId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeCategories_categories_CategoriesId",
                table: "RecipeCategories",
                column: "CategoriesId",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeCategories_recipes_RecipesId",
                table: "RecipeCategories",
                column: "RecipesId",
                principalTable: "recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
