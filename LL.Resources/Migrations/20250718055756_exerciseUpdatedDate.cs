using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LL.Resources.Migrations
{
    /// <inheritdoc />
    public partial class exerciseUpdatedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                table: "Exercises",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Exercises",
                type: "timestamp",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_UpdatedById",
                table: "Exercises",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Users_UpdatedById",
                table: "Exercises",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Users_UpdatedById",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_UpdatedById",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Exercises");

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
        }
    }
}
