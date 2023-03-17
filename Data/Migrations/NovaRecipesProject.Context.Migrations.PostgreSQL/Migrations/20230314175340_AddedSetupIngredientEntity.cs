using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovaRecipesProject.Context.Migrations.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class AddedSetupIngredientEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Ingredients",
                table: "Ingredients");

            migrationBuilder.RenameTable(
                name: "Ingredients",
                newName: "ingredients");

            migrationBuilder.RenameIndex(
                name: "IX_Ingredients_Uid",
                table: "ingredients",
                newName: "IX_ingredients_Uid");

            migrationBuilder.AlterColumn<string>(
                name: "Portion",
                table: "ingredients",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ingredients",
                table: "ingredients",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ingredients",
                table: "ingredients");

            migrationBuilder.RenameTable(
                name: "ingredients",
                newName: "Ingredients");

            migrationBuilder.RenameIndex(
                name: "IX_ingredients_Uid",
                table: "Ingredients",
                newName: "IX_Ingredients_Uid");

            migrationBuilder.AlterColumn<string>(
                name: "Portion",
                table: "Ingredients",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ingredients",
                table: "Ingredients",
                column: "Id");
        }
    }
}
