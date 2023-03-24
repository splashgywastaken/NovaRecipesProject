using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovaRecipesProject.Context.Migrations.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class AddSetupRecipes1ToRecipeParagraphsNRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "recipeParagraphs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_recipeParagraphs_RecipeId",
                table: "recipeParagraphs",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_recipeParagraphs_recipes_RecipeId",
                table: "recipeParagraphs",
                column: "RecipeId",
                principalTable: "recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recipeParagraphs_recipes_RecipeId",
                table: "recipeParagraphs");

            migrationBuilder.DropIndex(
                name: "IX_recipeParagraphs_RecipeId",
                table: "recipeParagraphs");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "recipeParagraphs");
        }
    }
}
