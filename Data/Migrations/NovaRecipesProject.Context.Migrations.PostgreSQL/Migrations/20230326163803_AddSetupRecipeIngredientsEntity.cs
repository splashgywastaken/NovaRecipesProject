using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NovaRecipesProject.Context.Migrations.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class AddSetupRecipeIngredientsEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Portion",
                table: "ingredients");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "ingredients");

            migrationBuilder.CreateTable(
                name: "recipeIngredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RecipeId = table.Column<int>(type: "integer", nullable: false),
                    IngredientId = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    Portion = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Uid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recipeIngredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_recipeIngredients_ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_recipeIngredients_recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_recipeIngredients_IngredientId",
                table: "recipeIngredients",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_recipeIngredients_RecipeId",
                table: "recipeIngredients",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_recipeIngredients_Uid",
                table: "recipeIngredients",
                column: "Uid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "recipeIngredients");

            migrationBuilder.AddColumn<string>(
                name: "Portion",
                table: "ingredients",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "Weight",
                table: "ingredients",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
