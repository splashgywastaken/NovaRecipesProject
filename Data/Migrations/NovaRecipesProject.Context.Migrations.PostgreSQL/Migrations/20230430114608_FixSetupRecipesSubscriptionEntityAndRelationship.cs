using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovaRecipesProject.Context.Migrations.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class FixSetupRecipesSubscriptionEntityAndRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userRecipesSubscription_users_SubscriberId",
                table: "userRecipesSubscription");

            migrationBuilder.DropIndex(
                name: "IX_userRecipesSubscription_SubscriberId_AuthorId",
                table: "userRecipesSubscription");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_userRecipesSubscription_SubscriberId_AuthorId",
                table: "userRecipesSubscription",
                columns: new[] { "SubscriberId", "AuthorId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_userRecipesSubscription_users_SubscriberId",
                table: "userRecipesSubscription",
                column: "SubscriberId",
                principalTable: "users",
                principalColumn: "EntryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
