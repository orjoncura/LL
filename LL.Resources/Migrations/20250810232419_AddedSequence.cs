using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LL.Resources.Migrations
{
    /// <inheritdoc />
    public partial class AddedSequence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sequence",
                table: "Modules",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "ImportanceRatings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 10, 23, 24, 19, 477, DateTimeKind.Unspecified).AddTicks(8213));

            migrationBuilder.UpdateData(
                table: "ImportanceRatings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 10, 23, 24, 19, 477, DateTimeKind.Unspecified).AddTicks(8375));

            migrationBuilder.UpdateData(
                table: "ImportanceRatings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 10, 23, 24, 19, 477, DateTimeKind.Unspecified).AddTicks(8406));

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 10, 23, 24, 19, 477, DateTimeKind.Unspecified).AddTicks(1766));

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 10, 23, 24, 19, 477, DateTimeKind.Unspecified).AddTicks(1949));

            migrationBuilder.UpdateData(
                table: "MessageStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 10, 23, 24, 19, 469, DateTimeKind.Unspecified).AddTicks(5182));

            migrationBuilder.UpdateData(
                table: "MessageStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 10, 23, 24, 19, 475, DateTimeKind.Unspecified).AddTicks(8048));

            migrationBuilder.UpdateData(
                table: "MessageStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 10, 23, 24, 19, 475, DateTimeKind.Unspecified).AddTicks(8215));

            migrationBuilder.UpdateData(
                table: "ModuleTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 10, 23, 24, 19, 484, DateTimeKind.Unspecified).AddTicks(1285));

            migrationBuilder.UpdateData(
                table: "ModuleTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 10, 23, 24, 19, 484, DateTimeKind.Unspecified).AddTicks(1476));

            migrationBuilder.InsertData(
                table: "ModuleTypes",
                columns: new[] { "Id", "CreatedDate", "Value" },
                values: new object[] { 3, new DateTime(2025, 8, 10, 23, 24, 19, 484, DateTimeKind.Unspecified).AddTicks(1511), "Multiselect" });

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 10, 23, 24, 19, 481, DateTimeKind.Unspecified).AddTicks(9651));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 10, 23, 24, 19, 481, DateTimeKind.Unspecified).AddTicks(9815));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 10, 23, 24, 19, 481, DateTimeKind.Unspecified).AddTicks(9853));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 10, 23, 24, 19, 481, DateTimeKind.Unspecified).AddTicks(9855));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 10, 23, 24, 19, 481, DateTimeKind.Unspecified).AddTicks(9857));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 10, 23, 24, 19, 481, DateTimeKind.Unspecified).AddTicks(9861));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 10, 23, 24, 19, 481, DateTimeKind.Unspecified).AddTicks(9863));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 10, 23, 24, 19, 481, DateTimeKind.Unspecified).AddTicks(9864));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ModuleTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "Sequence",
                table: "Modules");

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

            migrationBuilder.UpdateData(
                table: "ModuleTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 1, 23, 17, 55, 723, DateTimeKind.Unspecified).AddTicks(5804));

            migrationBuilder.UpdateData(
                table: "ModuleTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 8, 1, 23, 17, 55, 723, DateTimeKind.Unspecified).AddTicks(5962));

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
        }
    }
}
