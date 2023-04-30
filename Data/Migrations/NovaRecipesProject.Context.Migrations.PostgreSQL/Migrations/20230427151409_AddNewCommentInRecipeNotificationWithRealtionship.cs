using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NovaRecipesProject.Context.Migrations.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class AddNewCommentInRecipeNotificationWithRealtionship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recipeComments_recipes_RecipeId",
                table: "recipeComments");

            migrationBuilder.DropForeignKey(
                name: "FK_recipeCommentsSubscription_recipes_RecipeId",
                table: "recipeCommentsSubscription");

            migrationBuilder.DropForeignKey(
                name: "FK_recipeCommentsSubscription_recipes_RecipeId1",
                table: "recipeCommentsSubscription");

            migrationBuilder.DropForeignKey(
                name: "FK_recipeCommentsSubscription_users_SubscriberId",
                table: "recipeCommentsSubscription");

            migrationBuilder.DropIndex(
                name: "IX_recipeCommentsSubscription_RecipeId_SubscriberId",
                table: "recipeCommentsSubscription");

            migrationBuilder.DropIndex(
                name: "IX_recipeCommentsSubscription_RecipeId1",
                table: "recipeCommentsSubscription");

            migrationBuilder.DropColumn(
                name: "RecipeId1",
                table: "recipeCommentsSubscription");

            migrationBuilder.RenameColumn(
                name: "SubscriberId",
                table: "recipeCommentsSubscription",
                newName: "SubscriptionSubscriberId");

            migrationBuilder.RenameIndex(
                name: "IX_recipeCommentsSubscription_SubscriberId",
                table: "recipeCommentsSubscription",
                newName: "IX_recipeCommentsSubscription_SubscriptionSubscriberId");

            migrationBuilder.RenameColumn(
                name: "RecipeId",
                table: "recipeComments",
                newName: "CommentRecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_recipeComments_RecipeId",
                table: "recipeComments",
                newName: "IX_recipeComments_CommentRecipeId");

            migrationBuilder.RenameColumn(
                name: "RequestCreationDataTime",
                table: "emailConfirmationRequests",
                newName: "RequestCreationDateTime");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeId",
                table: "recipeCommentsSubscription",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionRecipeId",
                table: "recipeCommentsSubscription",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "newCommentInRecipeNotification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubscriptionId = table.Column<int>(type: "integer", nullable: false),
                    NotificationCreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    Uid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_newCommentInRecipeNotification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_newCommentInRecipeNotification_recipeCommentsSubscription_S~",
                        column: x => x.SubscriptionId,
                        principalTable: "recipeCommentsSubscription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_recipeCommentsSubscription_RecipeId",
                table: "recipeCommentsSubscription",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_recipeCommentsSubscription_SubscriptionRecipeId_Subscriptio~",
                table: "recipeCommentsSubscription",
                columns: new[] { "SubscriptionRecipeId", "SubscriptionSubscriberId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_newCommentInRecipeNotification_SubscriptionId",
                table: "newCommentInRecipeNotification",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_newCommentInRecipeNotification_Uid",
                table: "newCommentInRecipeNotification",
                column: "Uid",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_recipeComments_recipes_CommentRecipeId",
                table: "recipeComments",
                column: "CommentRecipeId",
                principalTable: "recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_recipeCommentsSubscription_recipes_RecipeId",
                table: "recipeCommentsSubscription",
                column: "RecipeId",
                principalTable: "recipes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_recipeCommentsSubscription_recipes_SubscriptionRecipeId",
                table: "recipeCommentsSubscription",
                column: "SubscriptionRecipeId",
                principalTable: "recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_recipeCommentsSubscription_users_SubscriptionSubscriberId",
                table: "recipeCommentsSubscription",
                column: "SubscriptionSubscriberId",
                principalTable: "users",
                principalColumn: "EntryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recipeComments_recipes_CommentRecipeId",
                table: "recipeComments");

            migrationBuilder.DropForeignKey(
                name: "FK_recipeCommentsSubscription_recipes_RecipeId",
                table: "recipeCommentsSubscription");

            migrationBuilder.DropForeignKey(
                name: "FK_recipeCommentsSubscription_recipes_SubscriptionRecipeId",
                table: "recipeCommentsSubscription");

            migrationBuilder.DropForeignKey(
                name: "FK_recipeCommentsSubscription_users_SubscriptionSubscriberId",
                table: "recipeCommentsSubscription");

            migrationBuilder.DropTable(
                name: "newCommentInRecipeNotification");

            migrationBuilder.DropIndex(
                name: "IX_recipeCommentsSubscription_RecipeId",
                table: "recipeCommentsSubscription");

            migrationBuilder.DropIndex(
                name: "IX_recipeCommentsSubscription_SubscriptionRecipeId_Subscriptio~",
                table: "recipeCommentsSubscription");

            migrationBuilder.DropColumn(
                name: "SubscriptionRecipeId",
                table: "recipeCommentsSubscription");

            migrationBuilder.RenameColumn(
                name: "SubscriptionSubscriberId",
                table: "recipeCommentsSubscription",
                newName: "SubscriberId");

            migrationBuilder.RenameIndex(
                name: "IX_recipeCommentsSubscription_SubscriptionSubscriberId",
                table: "recipeCommentsSubscription",
                newName: "IX_recipeCommentsSubscription_SubscriberId");

            migrationBuilder.RenameColumn(
                name: "CommentRecipeId",
                table: "recipeComments",
                newName: "RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_recipeComments_CommentRecipeId",
                table: "recipeComments",
                newName: "IX_recipeComments_RecipeId");

            migrationBuilder.RenameColumn(
                name: "RequestCreationDateTime",
                table: "emailConfirmationRequests",
                newName: "RequestCreationDataTime");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeId",
                table: "recipeCommentsSubscription",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_recipeComments_recipes_RecipeId",
                table: "recipeComments",
                column: "RecipeId",
                principalTable: "recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_recipeCommentsSubscription_users_SubscriberId",
                table: "recipeCommentsSubscription",
                column: "SubscriberId",
                principalTable: "users",
                principalColumn: "EntryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
