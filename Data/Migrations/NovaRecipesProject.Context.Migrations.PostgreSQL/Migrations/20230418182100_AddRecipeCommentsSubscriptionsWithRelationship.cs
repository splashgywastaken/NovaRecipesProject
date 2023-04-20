using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NovaRecipesProject.Context.Migrations.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class AddRecipeCommentsSubscriptionsWithRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "recipeCommentsSubscription",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RecipeId = table.Column<int>(type: "integer", nullable: false),
                    SubscriberId = table.Column<int>(type: "integer", nullable: false),
                    Uid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recipeCommentsSubscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_recipeCommentsSubscription_recipes_SubscriberId",
                        column: x => x.SubscriberId,
                        principalTable: "recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_recipeCommentsSubscription_users_SubscriberId",
                        column: x => x.SubscriberId,
                        principalTable: "users",
                        principalColumn: "EntryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_recipeCommentsSubscription_SubscriberId_RecipeId",
                table: "recipeCommentsSubscription",
                columns: new[] { "SubscriberId", "RecipeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_recipeCommentsSubscription_Uid",
                table: "recipeCommentsSubscription",
                column: "Uid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "recipeCommentsSubscription");
        }
    }
}
