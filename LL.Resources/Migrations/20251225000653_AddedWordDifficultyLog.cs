using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LL.Resources.Migrations
{
    /// <inheritdoc />
    public partial class AddedWordDifficultyLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WordDifficulty_ImportanceRatings_DifficultyId",
                table: "WordDifficulty");

            migrationBuilder.DropForeignKey(
                name: "FK_WordDifficulty_Users_UserId",
                table: "WordDifficulty");

            migrationBuilder.DropForeignKey(
                name: "FK_WordDifficulty_Words_WordId",
                table: "WordDifficulty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WordDifficulty",
                table: "WordDifficulty");

            migrationBuilder.RenameTable(
                name: "WordDifficulty",
                newName: "WordDifficulties");

            migrationBuilder.RenameIndex(
                name: "IX_WordDifficulty_WordId",
                table: "WordDifficulties",
                newName: "IX_WordDifficulties_WordId");

            migrationBuilder.RenameIndex(
                name: "IX_WordDifficulty_UserId",
                table: "WordDifficulties",
                newName: "IX_WordDifficulties_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_WordDifficulty_DifficultyId",
                table: "WordDifficulties",
                newName: "IX_WordDifficulties_DifficultyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WordDifficulties",
                table: "WordDifficulties",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "WordDifficultyLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WordDifficultyId = table.Column<int>(type: "integer", nullable: false),
                    WordId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    DifficultyId = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedById = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordDifficultyLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordDifficultyLogs_ImportanceRatings_DifficultyId",
                        column: x => x.DifficultyId,
                        principalTable: "ImportanceRatings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WordDifficultyLogs_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WordDifficultyLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WordDifficultyLogs_WordDifficulties_WordDifficultyId",
                        column: x => x.WordDifficultyId,
                        principalTable: "WordDifficulties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WordDifficultyLogs_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WordDifficultyLogs_CreatedById",
                table: "WordDifficultyLogs",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_WordDifficultyLogs_DifficultyId",
                table: "WordDifficultyLogs",
                column: "DifficultyId");

            migrationBuilder.CreateIndex(
                name: "IX_WordDifficultyLogs_UserId",
                table: "WordDifficultyLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WordDifficultyLogs_WordDifficultyId",
                table: "WordDifficultyLogs",
                column: "WordDifficultyId");

            migrationBuilder.CreateIndex(
                name: "IX_WordDifficultyLogs_WordId",
                table: "WordDifficultyLogs",
                column: "WordId");

            migrationBuilder.AddForeignKey(
                name: "FK_WordDifficulties_ImportanceRatings_DifficultyId",
                table: "WordDifficulties",
                column: "DifficultyId",
                principalTable: "ImportanceRatings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WordDifficulties_Users_UserId",
                table: "WordDifficulties",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WordDifficulties_Words_WordId",
                table: "WordDifficulties",
                column: "WordId",
                principalTable: "Words",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WordDifficulties_ImportanceRatings_DifficultyId",
                table: "WordDifficulties");

            migrationBuilder.DropForeignKey(
                name: "FK_WordDifficulties_Users_UserId",
                table: "WordDifficulties");

            migrationBuilder.DropForeignKey(
                name: "FK_WordDifficulties_Words_WordId",
                table: "WordDifficulties");

            migrationBuilder.DropTable(
                name: "WordDifficultyLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WordDifficulties",
                table: "WordDifficulties");

            migrationBuilder.RenameTable(
                name: "WordDifficulties",
                newName: "WordDifficulty");

            migrationBuilder.RenameIndex(
                name: "IX_WordDifficulties_WordId",
                table: "WordDifficulty",
                newName: "IX_WordDifficulty_WordId");

            migrationBuilder.RenameIndex(
                name: "IX_WordDifficulties_UserId",
                table: "WordDifficulty",
                newName: "IX_WordDifficulty_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_WordDifficulties_DifficultyId",
                table: "WordDifficulty",
                newName: "IX_WordDifficulty_DifficultyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WordDifficulty",
                table: "WordDifficulty",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WordDifficulty_ImportanceRatings_DifficultyId",
                table: "WordDifficulty",
                column: "DifficultyId",
                principalTable: "ImportanceRatings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WordDifficulty_Users_UserId",
                table: "WordDifficulty",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WordDifficulty_Words_WordId",
                table: "WordDifficulty",
                column: "WordId",
                principalTable: "Words",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
