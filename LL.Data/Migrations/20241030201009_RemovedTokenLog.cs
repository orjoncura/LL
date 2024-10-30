using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LL.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedTokenLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTokenLogs");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "UserTokens");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "UserTokens",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2024, 10, 30, 20, 10, 9, 580, DateTimeKind.Unspecified).AddTicks(8788), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2024, 10, 30, 20, 10, 9, 580, DateTimeKind.Unspecified).AddTicks(8808), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "MessageStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2024, 10, 30, 20, 10, 9, 580, DateTimeKind.Unspecified).AddTicks(7579), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "MessageStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2024, 10, 30, 20, 10, 9, 580, DateTimeKind.Unspecified).AddTicks(7725), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "MessageStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2024, 10, 30, 20, 10, 9, 580, DateTimeKind.Unspecified).AddTicks(7728), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "SeminarWordRank",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2024, 10, 30, 20, 10, 9, 581, DateTimeKind.Unspecified).AddTicks(7766), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "SeminarWordRank",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2024, 10, 30, 20, 10, 9, 581, DateTimeKind.Unspecified).AddTicks(7780), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "SeminarWordRank",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2024, 10, 30, 20, 10, 9, 581, DateTimeKind.Unspecified).AddTicks(7782), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2024, 10, 30, 20, 10, 9, 581, DateTimeKind.Unspecified).AddTicks(2798), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2024, 10, 30, 20, 10, 9, 581, DateTimeKind.Unspecified).AddTicks(2811), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2024, 10, 30, 20, 10, 9, 581, DateTimeKind.Unspecified).AddTicks(2814), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2024, 10, 30, 20, 10, 9, 581, DateTimeKind.Unspecified).AddTicks(2816), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2024, 10, 30, 20, 10, 9, 581, DateTimeKind.Unspecified).AddTicks(2818), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Token",
                table: "UserTokens",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "UserTokens",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "UserTokenLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UserTokenId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Expiration = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Token = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokenLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTokenLogs_UserTokens_UserTokenId",
                        column: x => x.UserTokenId,
                        principalTable: "UserTokens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTokenLogs_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2024, 10, 30, 13, 58, 33, 427, DateTimeKind.Unspecified).AddTicks(6906), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2024, 10, 30, 13, 58, 33, 427, DateTimeKind.Unspecified).AddTicks(6916), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "MessageStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2024, 10, 30, 13, 58, 33, 427, DateTimeKind.Unspecified).AddTicks(5882), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "MessageStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2024, 10, 30, 13, 58, 33, 427, DateTimeKind.Unspecified).AddTicks(5968), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "MessageStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2024, 10, 30, 13, 58, 33, 427, DateTimeKind.Unspecified).AddTicks(5970), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "SeminarWordRank",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2024, 10, 30, 13, 58, 33, 428, DateTimeKind.Unspecified).AddTicks(5430), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "SeminarWordRank",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2024, 10, 30, 13, 58, 33, 428, DateTimeKind.Unspecified).AddTicks(5455), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "SeminarWordRank",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2024, 10, 30, 13, 58, 33, 428, DateTimeKind.Unspecified).AddTicks(5458), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2024, 10, 30, 13, 58, 33, 428, DateTimeKind.Unspecified).AddTicks(506), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2024, 10, 30, 13, 58, 33, 428, DateTimeKind.Unspecified).AddTicks(524), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2024, 10, 30, 13, 58, 33, 428, DateTimeKind.Unspecified).AddTicks(526), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2024, 10, 30, 13, 58, 33, 428, DateTimeKind.Unspecified).AddTicks(528), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "WordTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2024, 10, 30, 13, 58, 33, 428, DateTimeKind.Unspecified).AddTicks(530), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "IX_UserTokenLogs_CreatedById",
                table: "UserTokenLogs",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserTokenLogs_UserTokenId",
                table: "UserTokenLogs",
                column: "UserTokenId");
        }
    }
}
