using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LL.Resources.Migrations
{
    /// <inheritdoc />
    public partial class CourseWordUpdatedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                table: "CourseWords",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "CourseWords",
                type: "timestamp",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ImportanceRatings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 16, 22, 40, 10, 769, DateTimeKind.Unspecified).AddTicks(8949));

            migrationBuilder.UpdateData(
                table: "ImportanceRatings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 16, 22, 40, 10, 769, DateTimeKind.Unspecified).AddTicks(9110));

            migrationBuilder.UpdateData(
                table: "ImportanceRatings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 16, 22, 40, 10, 769, DateTimeKind.Unspecified).AddTicks(9135));

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 16, 22, 40, 10, 763, DateTimeKind.Unspecified).AddTicks(2028));

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 16, 22, 40, 10, 763, DateTimeKind.Unspecified).AddTicks(2206));

            migrationBuilder.UpdateData(
                table: "MessageStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 16, 22, 40, 10, 755, DateTimeKind.Unspecified).AddTicks(3692));

            migrationBuilder.UpdateData(
                table: "MessageStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 16, 22, 40, 10, 761, DateTimeKind.Unspecified).AddTicks(7719));

            migrationBuilder.UpdateData(
                table: "MessageStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 16, 22, 40, 10, 761, DateTimeKind.Unspecified).AddTicks(7857));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 16, 22, 40, 10, 767, DateTimeKind.Unspecified).AddTicks(2390));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 16, 22, 40, 10, 767, DateTimeKind.Unspecified).AddTicks(2561));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 16, 22, 40, 10, 767, DateTimeKind.Unspecified).AddTicks(2594));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 16, 22, 40, 10, 767, DateTimeKind.Unspecified).AddTicks(2597));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 16, 22, 40, 10, 767, DateTimeKind.Unspecified).AddTicks(2599));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 16, 22, 40, 10, 767, DateTimeKind.Unspecified).AddTicks(2603));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 16, 22, 40, 10, 767, DateTimeKind.Unspecified).AddTicks(2604));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 16, 22, 40, 10, 767, DateTimeKind.Unspecified).AddTicks(2606));

            migrationBuilder.CreateIndex(
                name: "IX_CourseWords_UpdatedById",
                table: "CourseWords",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseWords_Users_UpdatedById",
                table: "CourseWords",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseWords_Users_UpdatedById",
                table: "CourseWords");

            migrationBuilder.DropIndex(
                name: "IX_CourseWords_UpdatedById",
                table: "CourseWords");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "CourseWords");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "CourseWords");

            migrationBuilder.UpdateData(
                table: "ImportanceRatings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 13, 22, 44, 21, 938, DateTimeKind.Unspecified).AddTicks(8376));

            migrationBuilder.UpdateData(
                table: "ImportanceRatings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 13, 22, 44, 21, 938, DateTimeKind.Unspecified).AddTicks(8542));

            migrationBuilder.UpdateData(
                table: "ImportanceRatings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 13, 22, 44, 21, 938, DateTimeKind.Unspecified).AddTicks(8569));

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 13, 22, 44, 21, 932, DateTimeKind.Unspecified).AddTicks(3556));

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 13, 22, 44, 21, 932, DateTimeKind.Unspecified).AddTicks(3719));

            migrationBuilder.UpdateData(
                table: "MessageStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 13, 22, 44, 21, 924, DateTimeKind.Unspecified).AddTicks(9501));

            migrationBuilder.UpdateData(
                table: "MessageStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 13, 22, 44, 21, 931, DateTimeKind.Unspecified).AddTicks(409));

            migrationBuilder.UpdateData(
                table: "MessageStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 13, 22, 44, 21, 931, DateTimeKind.Unspecified).AddTicks(639));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 13, 22, 44, 21, 936, DateTimeKind.Unspecified).AddTicks(2930));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 13, 22, 44, 21, 936, DateTimeKind.Unspecified).AddTicks(3089));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 13, 22, 44, 21, 936, DateTimeKind.Unspecified).AddTicks(3117));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 13, 22, 44, 21, 936, DateTimeKind.Unspecified).AddTicks(3119));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 13, 22, 44, 21, 936, DateTimeKind.Unspecified).AddTicks(3121));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 13, 22, 44, 21, 936, DateTimeKind.Unspecified).AddTicks(3125));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 13, 22, 44, 21, 936, DateTimeKind.Unspecified).AddTicks(3127));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2025, 7, 13, 22, 44, 21, 936, DateTimeKind.Unspecified).AddTicks(3128));
        }
    }
}
