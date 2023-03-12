using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovaRecipesProject.Context.Migrations.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTablesNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_recipeParagraph",
                table: "recipeParagraph");

            migrationBuilder.DropPrimaryKey(
                name: "PK_category",
                table: "category");

            migrationBuilder.RenameTable(
                name: "recipeParagraph",
                newName: "recipeParagraphs");

            migrationBuilder.RenameTable(
                name: "category",
                newName: "categories");

            migrationBuilder.RenameIndex(
                name: "IX_recipeParagraph_Uid",
                table: "recipeParagraphs",
                newName: "IX_recipeParagraphs_Uid");

            migrationBuilder.RenameIndex(
                name: "IX_category_Uid",
                table: "categories",
                newName: "IX_categories_Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_recipeParagraphs",
                table: "recipeParagraphs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_categories",
                table: "categories",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_recipeParagraphs",
                table: "recipeParagraphs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_categories",
                table: "categories");

            migrationBuilder.RenameTable(
                name: "recipeParagraphs",
                newName: "recipeParagraph");

            migrationBuilder.RenameTable(
                name: "categories",
                newName: "category");

            migrationBuilder.RenameIndex(
                name: "IX_recipeParagraphs_Uid",
                table: "recipeParagraph",
                newName: "IX_recipeParagraph_Uid");

            migrationBuilder.RenameIndex(
                name: "IX_categories_Uid",
                table: "category",
                newName: "IX_category_Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_recipeParagraph",
                table: "recipeParagraph",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_category",
                table: "category",
                column: "Id");
        }
    }
}
