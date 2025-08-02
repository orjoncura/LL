using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LL.Resources.Migrations
{
    /// <inheritdoc />
    public partial class Modules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseWords_Courses_CourseId",
                table: "CourseWords");

            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_CourseWords_CourseWordId",
                table: "Exercises");

            migrationBuilder.RenameColumn(
                name: "CourseWordId",
                table: "Exercises",
                newName: "ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_Exercises_CourseWordId",
                table: "Exercises",
                newName: "IX_Exercises_ModuleId");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "CourseWords",
                newName: "ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseWords_CourseId",
                table: "CourseWords",
                newName: "IX_CourseWords_ModuleId");

            migrationBuilder.CreateTable(
                name: "ModuleTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CourseId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    TypeId = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    Unlocked = table.Column<bool>(type: "boolean", nullable: false),
                    Completed = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedById = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modules_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Modules_ModuleTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ModuleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Modules_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ModuleLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModuleId = table.Column<int>(type: "integer", nullable: false),
                    CourseId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    TypeId = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    Unlocked = table.Column<bool>(type: "boolean", nullable: false),
                    Completed = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedById = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleLogs_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModuleLogs_ModuleTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ModuleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModuleLogs_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModuleLogs_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "ImportanceRatings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 1, 23, 17, 55, 717, DateTimeKind.Unspecified).AddTicks(5899));

            migrationBuilder.UpdateData(
                table: "ImportanceRatings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 1, 23, 17, 55, 717, DateTimeKind.Unspecified).AddTicks(6070));

            migrationBuilder.UpdateData(
                table: "ImportanceRatings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 1, 23, 17, 55, 717, DateTimeKind.Unspecified).AddTicks(6099));

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 1, 23, 17, 55, 716, DateTimeKind.Unspecified).AddTicks(9792));

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 1, 23, 17, 55, 716, DateTimeKind.Unspecified).AddTicks(9949));

            migrationBuilder.UpdateData(
                table: "MessageStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 1, 23, 17, 55, 709, DateTimeKind.Unspecified).AddTicks(7339));

            migrationBuilder.UpdateData(
                table: "MessageStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 1, 23, 17, 55, 715, DateTimeKind.Unspecified).AddTicks(7491));

            migrationBuilder.UpdateData(
                table: "MessageStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 1, 23, 17, 55, 715, DateTimeKind.Unspecified).AddTicks(7687));

            migrationBuilder.InsertData(
                table: "ModuleTypes",
                columns: new[] { "Id", "CreatedDate", "Value" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 8, 1, 23, 17, 55, 723, DateTimeKind.Unspecified).AddTicks(5804), "Flashcards" },
                    { 2, new DateTime(2025, 8, 1, 23, 17, 55, 723, DateTimeKind.Unspecified).AddTicks(5962), "Exercises" }
                });

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 1, 23, 17, 55, 721, DateTimeKind.Unspecified).AddTicks(5126));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 1, 23, 17, 55, 721, DateTimeKind.Unspecified).AddTicks(5289));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 1, 23, 17, 55, 721, DateTimeKind.Unspecified).AddTicks(5315));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 1, 23, 17, 55, 721, DateTimeKind.Unspecified).AddTicks(5318));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 1, 23, 17, 55, 721, DateTimeKind.Unspecified).AddTicks(5319));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 1, 23, 17, 55, 721, DateTimeKind.Unspecified).AddTicks(5323));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 1, 23, 17, 55, 721, DateTimeKind.Unspecified).AddTicks(5324));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 1, 23, 17, 55, 721, DateTimeKind.Unspecified).AddTicks(5326));

            migrationBuilder.CreateIndex(
                name: "IX_ModuleLogs_CourseId",
                table: "ModuleLogs",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleLogs_CreatedById",
                table: "ModuleLogs",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleLogs_ModuleId",
                table: "ModuleLogs",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleLogs_TypeId",
                table: "ModuleLogs",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_CourseId",
                table: "Modules",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_CreatedById",
                table: "Modules",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_TypeId",
                table: "Modules",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseWords_Modules_ModuleId",
                table: "CourseWords",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Modules_ModuleId",
                table: "Exercises",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseWords_Modules_ModuleId",
                table: "CourseWords");

            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Modules_ModuleId",
                table: "Exercises");

            migrationBuilder.DropTable(
                name: "ModuleLogs");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "ModuleTypes");

            migrationBuilder.RenameColumn(
                name: "ModuleId",
                table: "Exercises",
                newName: "CourseWordId");

            migrationBuilder.RenameIndex(
                name: "IX_Exercises_ModuleId",
                table: "Exercises",
                newName: "IX_Exercises_CourseWordId");

            migrationBuilder.RenameColumn(
                name: "ModuleId",
                table: "CourseWords",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseWords_ModuleId",
                table: "CourseWords",
                newName: "IX_CourseWords_CourseId");

            migrationBuilder.UpdateData(
                table: "ImportanceRatings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 18, 5, 57, 56, 359, DateTimeKind.Unspecified).AddTicks(2860));

            migrationBuilder.UpdateData(
                table: "ImportanceRatings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 18, 5, 57, 56, 359, DateTimeKind.Unspecified).AddTicks(3187));

            migrationBuilder.UpdateData(
                table: "ImportanceRatings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 18, 5, 57, 56, 359, DateTimeKind.Unspecified).AddTicks(3215));

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 18, 5, 57, 56, 352, DateTimeKind.Unspecified).AddTicks(6132));

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 18, 5, 57, 56, 352, DateTimeKind.Unspecified).AddTicks(6290));

            migrationBuilder.UpdateData(
                table: "MessageStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 18, 5, 57, 56, 345, DateTimeKind.Unspecified).AddTicks(1328));

            migrationBuilder.UpdateData(
                table: "MessageStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 18, 5, 57, 56, 351, DateTimeKind.Unspecified).AddTicks(2417));

            migrationBuilder.UpdateData(
                table: "MessageStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 18, 5, 57, 56, 351, DateTimeKind.Unspecified).AddTicks(2542));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 18, 5, 57, 56, 356, DateTimeKind.Unspecified).AddTicks(4726));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 18, 5, 57, 56, 356, DateTimeKind.Unspecified).AddTicks(4883));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 18, 5, 57, 56, 356, DateTimeKind.Unspecified).AddTicks(4915));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 18, 5, 57, 56, 356, DateTimeKind.Unspecified).AddTicks(4917));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 18, 5, 57, 56, 356, DateTimeKind.Unspecified).AddTicks(4919));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 18, 5, 57, 56, 356, DateTimeKind.Unspecified).AddTicks(4923));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 18, 5, 57, 56, 356, DateTimeKind.Unspecified).AddTicks(4925));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 18, 5, 57, 56, 356, DateTimeKind.Unspecified).AddTicks(4926));

            migrationBuilder.AddForeignKey(
                name: "FK_CourseWords_Courses_CourseId",
                table: "CourseWords",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_CourseWords_CourseWordId",
                table: "Exercises",
                column: "CourseWordId",
                principalTable: "CourseWords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
