using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NovaRecipesProject.Context.Migrations.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class SetupRecipesSubscriptionsRelatedStuff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "userRecipesSubscription",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubscriberId = table.Column<int>(type: "integer", nullable: false),
                    AuthorId = table.Column<int>(type: "integer", nullable: false),
                    Uid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userRecipesSubscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_userRecipesSubscription_users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "users",
                        principalColumn: "EntryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userRecipesSubscription_users_SubscriberId",
                        column: x => x.SubscriberId,
                        principalTable: "users",
                        principalColumn: "EntryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_userRecipesSubscription_AuthorId",
                table: "userRecipesSubscription",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_userRecipesSubscription_SubscriberId_AuthorId",
                table: "userRecipesSubscription",
                columns: new[] { "SubscriberId", "AuthorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_userRecipesSubscription_Uid",
                table: "userRecipesSubscription",
                column: "Uid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "userRecipesSubscription");
        }
    }
}
